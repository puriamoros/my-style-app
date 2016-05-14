using MyStyleApp.Models;
using MyStyleApp.Services;
using MyStyleApp.Services.Backend;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinFormsAutofacMvvmStarterKit;

namespace MyStyleApp.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        public ICommand LoginCommand { get; private set; }
        public ICommand NewAccountCommand { get; private set; }

        private ILoginService _loginService;
        private ObjectStorageService<string> _localStorageService;

        private string _email;
        private string _password;
        private bool _rememberMe;
        private string _errorText;

        public LoginViewModel(
            INavigator navigator,
            LocalizedStringsService localizedStringsService,
            ILoginService loginService,
            ObjectStorageService<string> localStorageService) :
            base(navigator, localizedStringsService)
        {
            this._loginService = loginService;
            this._localStorageService = localStorageService;
            this.LoginCommand = new Command(async () => await Login());
            this.NewAccountCommand = new Command(async () => await NewAccount());

            // REMOVE!!!
            FillWithMockData();
        }

        private void FillWithMockData()
        {
            this.Email = "helio.huete@gmail.com";
            this.Password = "helio";
        }

        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }

        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        public bool RememberMe
        {
            get { return _rememberMe; }
            set { SetProperty(ref _rememberMe, value); }
        }

        public string ErrorText
        {
            get { return _errorText; }
            set { SetProperty(ref _errorText, value); }
        }

        private async Task Login()
        {
            ErrorText = "";
            this.IsBusy = true;

            try
            {
                await this._loginService.Login(this.Email, this.Password, this.RememberMe);
                await this.Navigator.PopToRootAsync();
                await this.Navigator.PushAsync<RegisteredStoresViewModel>();
            }
            catch (Exception ex)
            {
                ErrorText = this.LocalizedStrings["login_error"];
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        private async Task NewAccount()
        {
            await this.Navigator.PushAsync<RegisteredStoresViewModel>();
        }
    }
}
