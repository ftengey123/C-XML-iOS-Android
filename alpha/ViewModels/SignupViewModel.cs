using System;
using System.IO;
using SQLite;
using Xamarin.Forms;
using alpha.Views;
using alpha.Services;
using System.Linq;
using Xamarin.Auth;
using Xamarin.Essentials;
using alpha.Tables;
using System.Text.RegularExpressions;
 

namespace alpha.ViewModels
{
    public class SignupViewModel : BaseViewModel
    {
        private readonly ICredentialsService signupService;

        private string userName;
        private string firstName;
        private string lastName;
        private string phoneNumber;
        private string password;
        private string password1;
        private bool isRemember;

        public const int PHONE_NUMBER_LENGTH = 10;
        public string UserName { get => userName; set => SetProperty(ref userName, value); }
        public string FirstName { get => firstName; set => SetProperty(ref firstName, value); }
        public string LastName { get => lastName; set => SetProperty(ref lastName, value); }
        public string PhoneNumber { get => phoneNumber; set => SetProperty(ref phoneNumber, value); }

        public string Password { get => password; set => SetProperty(ref password, value); }
        public string Password1 { get => password1; set => SetProperty(ref password1, value); }
        public bool IsRemember { get => isRemember; set => SetProperty(ref isRemember, value); }

        public Command CreateAcctCommand { get; }
        public Command LoadCommand { get; }

        public SignupViewModel()
        {
            CreateAcctCommand = new Command(OnSignupClicked);
            LoadCommand = new Command(ExecuteLoad);
            signupService = DependencyService.Get<ICredentialsService>();
        }

        private async void ExecuteLoad()
        {
            UserName = await SecureStorage.GetAsync(Constants.UserIdKey);
            Password = await SecureStorage.GetAsync(Constants.PwdKey);

            if (!string.IsNullOrEmpty(UserName))
                IsRemember = true;
        }

        private async void OnSignupClicked(object obj)
        {
            var database_path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDB.db");
            var db = new SQLiteConnection(database_path);
            db.CreateTable<RegisteredUserTable>();

            await SecureStorage.SetAsync(Constants.UserIdKey, UserName);
            await SecureStorage.SetAsync(Constants.PwdKey, Password);


            if (!FirstName.All(Char.IsLetter))
            {
                await AppShell.Current.DisplayAlert("Invalid First Name", "First name should be only letters.", "Ok");
                return;
            }
            if (!LastName.All(Char.IsLetter))
            {
                await AppShell.Current.DisplayAlert("Invalid Last Name", "Last name should be only letters.", "Ok");
                return;
            }

            Regex regex = new Regex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                                    + "@"
                                    + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$");
            Match match = regex.Match(UserName);
            if (!match.Success)
            {
                await AppShell.Current.DisplayAlert("Invalid Email", "Expected xxxx@xxx.xxx", "Ok");
                return;
            }
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMinimum6Chars = new Regex(@".{6,}");

            var isValidated = hasNumber.IsMatch(Password) && hasUpperChar.IsMatch(Password) && hasMinimum6Chars.IsMatch(Password);

            if (!isValidated)
            {
                await AppShell.Current.DisplayAlert("Invalid Password", "Password must have at least one capital letter, a number and must be at least 6 characters.", "Ok");
            }
            if (Password != Password1)
            {
                await AppShell.Current.DisplayAlert("Sign-up Failed", "Passwords do not match", "Ok");
                return;
            }

            if(!PhoneNumber.All(Char.IsDigit) || PhoneNumber.Length != PHONE_NUMBER_LENGTH)
            {
                await AppShell.Current.DisplayAlert("Invalid Phone number", "Must be 10 digits", "Ok");
                return;
            }
            var temp = new RegisteredUserTable()
            {
                UserName = UserName,
                Password = Password,
                PhoneNumber = PhoneNumber,
                FirstName = FirstName,
                LastName = LastName
            };

            db.Insert(temp);
            IsRemember = true;

            await Shell.Current.GoToAsync($"//{nameof(HomePage)}");

        }
    }
}