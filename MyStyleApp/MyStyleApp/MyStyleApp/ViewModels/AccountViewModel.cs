using System.Threading.Tasks;
using MyStyleApp.Services;
using MvvmCore;
using MyStyleApp.Validators;
using MyStyleApp.Constants;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace MyStyleApp.ViewModels
{
    public class AccountViewModel : NavigableViewModelBase
    {
        private const string STRING_NAME = "name";
        private const string STRING_SURNAME = "surname";
        private const string STRING_PHONE = "phone";
        private const string STRING_EMAIL = "email";
        private const string STRING_PASSWORD = "password";

        private ValidationService _validationService;

        public ICommand CreateAccountCommand { get; private set; }

        private string _name;
        private string _surname;
        private string _phone;
        private string _email;
        private string _password;
        private string _errorText;

        public AccountViewModel(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService,
            ValidationService validationService) :
            base(navigator, userNotificator, localizedStringsService)
        {
            this.CreateAccountCommand = new Command(this.CreateAccountAsync);

            this._validationService = validationService;
        }

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public string Surname
        {
            get { return _surname; }
            set { SetProperty(ref _surname, value); }
        }

        public string Phone
        {
            get { return _phone; }
            set { SetProperty(ref _phone, value); }
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

        public string ErrorText
        {
            get { return _errorText; }
            set { SetProperty(ref _errorText, value); }
        }

        private string GetValidationError()
        {
            // Alwais clear validators before adding
            this._validationService.ClearValidators();

            // Name
            this._validationService.AddValidator(
                new RequiredValidator(this.Name, STRING_NAME));
            this._validationService.AddValidator(
                new RegexValidator(
                    this.Name, RegexConstants.NOT_INSECURE_CHARS,
                    "error_insecure_chars", STRING_NAME));
            this._validationService.AddValidator(
                new LengthValidator(this.Name, STRING_NAME, 2, 100));

            // Surname
            this._validationService.AddValidator(
                new RequiredValidator(this.Surname, STRING_SURNAME));
            this._validationService.AddValidator(
                new RegexValidator(
                    this.Surname, RegexConstants.NOT_INSECURE_CHARS,
                    "error_insecure_chars", STRING_SURNAME));
            this._validationService.AddValidator(
                new LengthValidator(this.Surname, STRING_SURNAME, 2, 100));

            // Phone
            this._validationService.AddValidator(
                new RequiredValidator(this.Phone, STRING_PHONE));
            this._validationService.AddValidator(
                new RegexValidator(
                    this.Phone, RegexConstants.NOT_INSECURE_CHARS,
                    "error_insecure_chars", STRING_PHONE));
            this._validationService.AddValidator(
                new LengthValidator(this.Phone, STRING_PHONE, 9, 9));

            // Email
            this._validationService.AddValidator(
                new RequiredValidator(this.Email, STRING_EMAIL));
            this._validationService.AddValidator(
                new RegexValidator(
                    this.Email, RegexConstants.NOT_INSECURE_CHARS,
                    "error_insecure_chars", STRING_EMAIL));
            this._validationService.AddValidator(
                new RegexValidator(
                    this.Email, RegexConstants.EMAIL,
                    "error_invalid_field", STRING_EMAIL));

            // Password
            this._validationService.AddValidator(
                new RequiredValidator(this.Password, STRING_PASSWORD));

            return this._validationService.GetValidationError();
        }

        private async void CreateAccountAsync()
        {
            string validationError = this.GetValidationError();
            if (validationError != null)
            {
                this.ErrorText = this.LocalizedStrings.GetString("error") + ": " + validationError;
                return;
            }

            this.ErrorText = "";
            this.IsBusy = true;

            try
            {
                //await this._usersService.LoginAsync(this.Email, this.Password, this.RememberMe);
                //await this.SetMainPageAsync<MainViewModel>();
            }
            catch (Exception)
            {
                //ErrorText = this.LocalizedStrings[STRING_LOGIN_ERROR];
            }
            finally
            {
                this.IsBusy = false;
            }
        }
    }
}
