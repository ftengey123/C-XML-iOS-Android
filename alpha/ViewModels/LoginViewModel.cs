using System;
using System.IO;
using System.Net;
using Xamarin.Forms;
using alpha.Views;
using alpha.Services;
using SQLite;
using alpha.Tables;
using Xamarin.Auth;
using Xamarin.Essentials;

namespace alpha.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly ICredentialsService loginService;

        private string userName;
        private string password;
        private bool isRemember;

        public string UserName { get => userName; set => SetProperty(ref userName, value); }
        public string Password { get => password; set => SetProperty(ref password, value); }
        public bool IsRemember { get => isRemember; set => SetProperty(ref isRemember, value); }

        public Command LoginCommand { get; }
        public Command SignupCommand { get; }
        public Command LoadCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            SignupCommand = new Command(OnSignUpClicked);
            LoadCommand = new Command(ExecuteLoad);
            loginService = DependencyService.Get<ICredentialsService>();
        }

        private async void ExecuteLoad()
        {
            UserName = await SecureStorage.GetAsync(Constants.UserIdKey);
            Password = await SecureStorage.GetAsync(Constants.PwdKey);

            if (!string.IsNullOrEmpty(UserName))
                IsRemember = true;
        }

        private async void OnLoginClicked(object obj)
        {
            var database_path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDB.db");
            var db = new SQLiteConnection(database_path);
            var query = db.Table<RegisteredUserTable>().Where(user=>user.UserName.Equals(UserName) && user.Password.Equals(Password)).FirstOrDefault();
            
            if(query != null)
            {
                await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
            }
            else
            {
                await Shell.Current.DisplayAlert("Login Failed", "Invalid Email or Password.", "Ok");
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }

            await SecureStorage.SetAsync(Constants.UserIdKey, UserName);
            await SecureStorage.SetAsync(Constants.PwdKey, Password);

            IsRemember = true;

        }

        private async void OnSignUpClicked(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(SignupPage)}");
        }
    }
}