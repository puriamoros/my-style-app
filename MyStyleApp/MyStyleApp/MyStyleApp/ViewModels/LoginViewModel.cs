using MyStyleApp.Constants;
using MyStyleApp.Models;
using MyStyleApp.Services;
using MyStyleApp.Services.Backend;
using MyStyleApp.Validators;
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
        private const string STRING_ERROR_INVALID_FIELD = "error_invalid_field";
        private const string STRING_ERROR_INSECURE_CHARS = "error_insecure_chars";
        private const string STRING_ERROR = "error";
        private const string STRING_EMAIL = "email";
        private const string STRING_PASSWORD = "password";
        private const string STRING_LOGIN_ERROR = "login_error";

        public ICommand LoginCommand { get; private set; }
        public ICommand NewAccountCommand { get; private set; }

        private IUsersService _usersService;
        private ValidationService _validationService;

        private string _email;
        private string _password;
        private bool _rememberMe;
        private string _errorText;

        public LoginViewModel(
            INavigator navigator,
            LocalizedStringsService localizedStringsService,
            IUsersService usersService,
            ValidationService validationService) :
            base(navigator, localizedStringsService)
        {
            this._usersService = usersService;
            this._validationService = validationService;
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
            // Alwais clear validators before adding
            this._validationService.ClearValidators();

            // Email
            this._validationService.AddValidator(
                new RequiredValidator(this.Email, STRING_EMAIL));
            this._validationService.AddValidator(
                new RegexValidator(
                    this.Email, RegexConstants.NOT_INSECURE_CHARS,
                    STRING_ERROR_INSECURE_CHARS, STRING_EMAIL));
            this._validationService.AddValidator(
                new RegexValidator(
                    this.Email, RegexConstants.EMAIL,
                    STRING_ERROR_INVALID_FIELD, STRING_EMAIL));

            // Password
            this._validationService.AddValidator(
                new RequiredValidator(this.Password, STRING_PASSWORD));

            return this._validationService.GetValidationError();
        }

        private async Task Login()
        {
            string validationError = this.GetValidationError();
            if(validationError != null)
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

        public override void OnAppearing()
        {
            base.OnAppearing();
        }

        public override void OnPushed()
        {
            base.OnPushed();
        }
    }
}
