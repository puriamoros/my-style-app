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
        private const string STRING_ERROR_INVALID_FIELD = "error_invalid_field";
        private const string STRING_ERROR_INSECURE_CHARS = "error_insecure_chars";
        private const string STRING_LOGIN_ERROR = "login_error";
        private const string TOKEN_FIELD = "${FIELD_NAME}";

        public ICommand LoginCommand { get; private set; }
        public ICommand NewAccountCommand { get; private set; }

        private IUsersService _usersService;
        private ObjectStorageService<string> _localStorageService;

        private string _email;
        private string _password;
        private bool _rememberMe;
        private string _errorText;

        public LoginViewModel(
            INavigator navigator,
            LocalizedStringsService localizedStringsService,
            IUsersService usersService,
            ObjectStorageService<string> localStorageService) :
            base(navigator, localizedStringsService)
        {
            this._usersService = usersService;
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
            string validationError = "";

            // Email
            string fieldEmail = this.LocalizedStrings.GetString(STRING_EMAIL);
            if (string.IsNullOrEmpty(this.Email))
            {
                return this.LocalizedStrings.GetString(
                    STRING_ERROR_REQUIRED_FIELD, TOKEN_FIELD, fieldEmail);
            }
            if (Regex.IsMatch(this.Email, RegexConstants.INSECURE_CHARS))
            {
                return this.LocalizedStrings.GetString(
                    STRING_ERROR_INSECURE_CHARS, TOKEN_FIELD, fieldEmail);
            }
            if (!Regex.IsMatch(this.Email, RegexConstants.EMAIL))
            {
                return this.LocalizedStrings.GetString(
                    STRING_ERROR_INVALID_FIELD, TOKEN_FIELD, fieldEmail);
            }

            // Password
            string fieldPassword = this.LocalizedStrings.GetString(STRING_PASSWORD);
            if (string.IsNullOrEmpty(this.Password))
            {
                return this.LocalizedStrings.GetString(
                    STRING_ERROR_REQUIRED_FIELD, TOKEN_FIELD, fieldPassword);
            }

            return validationError;
        }

        private async Task Login()
        {
            string validationError = this.GetValidationError();
            if(!string.IsNullOrEmpty(validationError))
            {
                this.ErrorText = this.LocalizedStrings.GetString(STRING_ERROR) +": " + validationError;
                return;
            }

            this.ErrorText = "";
            this.IsBusy = true;

            try
            {
                await this._usersService.Login(this.Email, this.Password, this.RememberMe);
                await this.Navigator.SetMainPage<MainViewModel>();
            }
            catch (Exception)
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
