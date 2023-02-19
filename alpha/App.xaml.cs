using alpha.Services;
using alpha.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace alpha
{
    public partial class App : Application
    {
        public static string AppName { get { return "Alpha Delivery"; } }
        public static ICredentialsService CredentialsService { get; private set; }
        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            DependencyService.Register<CredentialsService>();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            AutoLogin();
            //Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }

        protected override void OnSleep()
        {
            
        }

        protected override void OnResume()
        {
        }

        private async void AutoLogin()
        {
            var loginService = DependencyService.Get<ICredentialsService>();
            var userName = await SecureStorage.GetAsync(Constants.UserIdKey);
            var password = await SecureStorage.GetAsync(Constants.PwdKey);

            if (!string.IsNullOrEmpty(userName) && await loginService.CheckLogin(userName, password))
            {
                await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
                return;
            }

            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}
