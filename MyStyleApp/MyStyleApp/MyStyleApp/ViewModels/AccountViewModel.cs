using System.Threading.Tasks;
using MyStyleApp.Services;
using MvvmCore;
using MyStyleApp.Validators;
using MyStyleApp.Constants;
using System;
using System.Windows.Input;
using Xamarin.Forms;
using MyStyleApp.Enums;
using MyStyleApp.Models;
using MyStyleApp.Services.Backend;

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
        public ICommand EditAccountCommand { get; private set; }
        public ICommand SaveAccountCommand { get; private set; }
        public ICommand ChangePasswordAccountCommand { get; private set; }
        public ICommand LogOutCommand { get; private set; }


        private string _name;
        private string _surname;
        private string _phone;
        private string _email;
        private string _repeatEmail;
        private string _password;
        private string _repeatPassword;
        private string _errorText;

        private AccountModeEnum _mode;

        IUsersService _usersService;

        public AccountViewModel(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService,
            ValidationService validationService,
            IUsersService usersService) :
            base(navigator, userNotificator, localizedStringsService)
        {
            this.CreateAccountCommand = new Command(this.CreateAccountAsync);
            this.EditAccountCommand = new Command(this.EditAccount);
            this.SaveAccountCommand = new Command(this.SaveAccountAsync);
            this.ChangePasswordAccountCommand = new Command(this.ChangePasswordAccountAsync);
            this.LogOutCommand = new Command(this.LogOutAsync);

            this._validationService = validationService;

            this._usersService = usersService;

            this.Mode = AccountModeEnum.View;
        }

        public void Initialize(User user, AccountModeEnum mode)
        {
            if(user != null)
            {
                this.Name = user.Name;
                this.Surname = user.Surname;
                this.Phone = user.Phone;
                this.Email = user.Email;
                this.RepeatEmail = this.Email;
            }
            else
            {
                this.Name = "";
                this.Surname = "";
                this.Phone = "";
                this.Email = "";
                this.RepeatEmail = "";
            }
            this.Password = "";
            
            this.Mode = mode;
        }

        public AccountModeEnum Mode
        {
            get { return _mode; }
            set { SetProperty(ref _mode, value); }
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

        public string RepeatEmail
        {
            get { return _repeatEmail; }
            set { SetProperty(ref _repeatEmail, value); }
        }

        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        public string RepeatPassword
        {
            get { return _repeatPassword; }
            set { SetProperty(ref _repeatPassword, value); }
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

            // RepeatEmail
            this._validationService.AddValidator(
                new RequiredValidator(this.RepeatEmail, STRING_EMAIL));
            this._validationService.AddValidator(
                new RegexValidator(
                    this.RepeatEmail, RegexConstants.NOT_INSECURE_CHARS,
                    "error_insecure_chars", STRING_EMAIL));
            this._validationService.AddValidator(
                new RegexValidator(
                    this.RepeatEmail, RegexConstants.EMAIL,
                    "error_invalid_field", STRING_EMAIL));

            //TODO: comprobar q email y repeat email son iguales. Idem con password y repeatpassword

            // Password
            this._validationService.AddValidator(
                new RequiredValidator(this.Password, STRING_PASSWORD));

            // RepeatPassword
            this._validationService.AddValidator(
                new RequiredValidator(this.RepeatPassword, STRING_PASSWORD));

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

            await this.ExecuteBlockingUIAsync(
                async () =>
                {
                    //await this._usersService.LoginAsync(this.Email, this.Password, this.RememberMe);
                    //await this.SetMainPageAsync<MainViewModel>();
                });
            
            //this.IsBusy = true;
            //try
            //{
            //    //await this._usersService.LoginAsync(this.Email, this.Password, this.RememberMe);
            //    //await this.SetMainPageAsync<MainViewModel>();
            //}
            //catch (Exception)
            //{
            //    //ErrorText = this.LocalizedStrings[STRING_LOGIN_ERROR];
            //}
            //finally
            //{
            //    this.IsBusy = false;
            //}
        }

        public void EditAccount()
        {
            this.Mode = AccountModeEnum.Edit;
        }

        public async void SaveAccountAsync()
        {
            await this.ExecuteBlockingUIAsync(
                async () =>
                {
                    //await this._usersService.LoginAsync(this.Email, this.Password, this.RememberMe);
                    //await this.SetMainPageAsync<MainViewModel>();
                    this.Mode = AccountModeEnum.View;
                });
        }

        public async void ChangePasswordAccountAsync()
        {
            await this.ExecuteBlockingUIAsync(
                async () =>
                {
                    //await this.SetMainPageAsync<MainViewModel>();
                });
        }
        public async void LogOutAsync()
        {
            await this.ExecuteBlockingUIAsync(
                async () =>
                {
                    await this._usersService.LogoutAsync();
                    await this.SetMainPageAsync<LoginViewModel>();
                });
        }

    }
}
