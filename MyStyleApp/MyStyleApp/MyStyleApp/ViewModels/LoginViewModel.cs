using MyStyleApp.Constants;
using MyStyleApp.Models;
using MyStyleApp.Services;
using MyStyleApp.Services.Backend;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinFormsAutofacMvvmStarterKit;

namespace MyStyleApp.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private const string STRING_ERROR = "error";
        private const string STRING_ERROR_REQUIRED_FIELD = "error_required_field";
        private const string STRING_EMAIL = "email";
        private const string STRING_PASSWORD = "password";
        private const string STRING_EMAIL_NOT_VALID = "email_not_valid";
        private const string STRING_ERROR_INSECURE_CHARS = "error_insecure_chars";
        private const string STRING_LOGIN_ERROR = "login_error";

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

        private string GetValidationError()
        {
            string validationError = null;

            // Email
            if(string.IsNullOrEmpty(this.Email))
            {
                return string.Format(STRING_ERROR_REQUIRED_FIELD, "${" + STRING_EMAIL + "}");
            }
            if (Regex.IsMatch(this.Email, RegexConstants.INSECURE_CHARS))
            {
                return string.Format(STRING_ERROR_INSECURE_CHARS, "${" + STRING_EMAIL + "}");
            }
            if (Regex.IsMatch(this.Email, RegexConstants.EMAIL))
            {
                return STRING_EMAIL_NOT_VALID;
            }

            // Password
            if (string.IsNullOrEmpty(this.Password))
            {
                return string.Format(STRING_ERROR_REQUIRED_FIELD, "${" + STRING_PASSWORD + "}");
            }
            if (Regex.IsMatch(this.Password, RegexConstants.INSECURE_CHARS))
            {
                return string.Format(STRING_ERROR_INSECURE_CHARS, "${" + STRING_PASSWORD + "}");
            }

            return validationError;
        }

        private async Task Login()
        {
            string validationError = this.GetValidationError();
            if(validationError != null)
            {
                ErrorText = this.LocalizedStrings[STRING_ERROR] + ": " + this.LocalizedStrings[validationError];
                return;
            }

            ErrorText = "";
            this.IsBusy = true;

            try
            {
                await this._loginService.Login(this.Email, this.Password, this.RememberMe);
                await this.Navigator.PushAsync<MainViewModel>();
                await this.Navigator.RemovePage<LoginViewModel>();
            }
            catch (Exception ex)
            {
                ErrorText = this.LocalizedStrings[STRING_LOGIN_ERROR];
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        private async Task NewAccount()
        {
            //await this.Navigator.PushAsync<MainViewModel>();
        }
    }
}
