using MyStyleApp.Services;
using MvvmCore;
using MyStyleApp.Validators;
using MyStyleApp.Constants;
using System.Windows.Input;
using Xamarin.Forms;
using MyStyleApp.Enums;
using MyStyleApp.Models;
using MyStyleApp.Services.Backend;
using System.Collections.ObjectModel;
using System;

namespace MyStyleApp.ViewModels
{
    public abstract class AccountViewModelBase : NavigableViewModelBase
    {
        protected const string STRING_NAME = "name";
        protected const string STRING_SURNAME = "surname";
        protected const string STRING_PHONE = "phone";
        protected const string STRING_EMAIL = "email";
        protected const string STRING_REPEATED_EMAIL = "repeat_email";

        protected IUsersService _usersService;

        public Command CreateAccountCommand { get; private set; }
        public Command EditAccountCommand { get; private set; }
        public Command SaveAccountCommand { get; private set; }
        public Command CancelCommand { get; private set; }
        public Command ChangePasswordAccountCommand { get; private set; }
        public Command LogOutCommand { get; private set; }

        private string _name;
        private string _surname;
        private string _phone;
        private string _email;
        private string _repeatEmail;
        private string _password;
        private string _repeatPassword;
        private string _errorText;

        private ObservableCollection<UserType> _userTypeList;
        private UserType _selectedUserType;

        private ObservableCollection<Establishment> _establishmentList;
        private Establishment _selectedEstablishment;

        private BaseModeEnum _mode;

        protected ValidationService _validationService;

        public AccountViewModelBase(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService,
            ValidationService validationService,
            IUsersService usersService) :
            base(navigator, userNotificator, localizedStringsService)
        {
            this._usersService = usersService;
            this._validationService = validationService;

            this.CreateAccountCommand = new Command(this.CreateAccountAsync, this.CanCreateAccount);
            this.EditAccountCommand = new Command(this.EditAccount);
            this.SaveAccountCommand = new Command(this.SaveAccountAsync, this.CanSaveAccount);
            this.CancelCommand = new Command(this.Cancel);
            this.ChangePasswordAccountCommand = new Command(this.ChangePasswordAccountAsync);
            this.LogOutCommand = new Command(this.LogOutAsync);
            
            this.Mode = BaseModeEnum.View;
            
        }

        protected void Initialize(User user, BaseModeEnum mode)
        {
            if (user != null)
            {
                this.Name = user.Name;
                this.Surname = user.Surname;
                this.Phone = user.Phone;
                this.Email = user.Email;
                this.RepeatEmail = user.Email;
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
            this.RepeatPassword = "";
            this.ErrorText = "";

            this.Mode = mode;
        }

        public BaseModeEnum Mode
        {
            get { return _mode; }
            set { SetProperty(ref _mode, value); }
        }

        public ObservableCollection<UserType> UserTypeList
        {
            get { return _userTypeList; }
            set { SetProperty(ref _userTypeList, value); }
        }

        public UserType SelectedUserType
        {
            get { return _selectedUserType; }
            set
            {
                SetProperty(ref _selectedUserType, value);
                this.SaveAccountCommand.ChangeCanExecute();
            }
        }

        public ObservableCollection<Establishment> EstablishmentList
        {
            get { return _establishmentList; }
            set { SetProperty(ref _establishmentList, value); }
        }

        public Establishment SelectedEstablishment
        {
            get { return _selectedEstablishment; }
            set
            {
                SetProperty(ref _selectedEstablishment, value);
                this.SaveAccountCommand.ChangeCanExecute();
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                SetProperty(ref _name, value);
                this.CreateAccountCommand.ChangeCanExecute();
                this.SaveAccountCommand.ChangeCanExecute();
            }
        }

        public string Surname
        {
            get { return _surname; }
            set
            {
                SetProperty(ref _surname, value);
                this.CreateAccountCommand.ChangeCanExecute();
                this.SaveAccountCommand.ChangeCanExecute();
            }
        }

        public string Phone
        {
            get { return _phone; }
            set
            {
                SetProperty(ref _phone, value);
                this.CreateAccountCommand.ChangeCanExecute();
                this.SaveAccountCommand.ChangeCanExecute();
            }
        }

        public string Email
        {
            get { return _email; }
            set
            {
                SetProperty(ref _email, value);
                this.CreateAccountCommand.ChangeCanExecute();
                this.SaveAccountCommand.ChangeCanExecute();
            }
        }

        public string RepeatEmail
        {
            get { return _repeatEmail; }
            set
            {
                SetProperty(ref _repeatEmail, value);
                this.CreateAccountCommand.ChangeCanExecute();
                this.SaveAccountCommand.ChangeCanExecute();
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                SetProperty(ref _password, value);
                this.CreateAccountCommand.ChangeCanExecute();
            }
        }

        public string RepeatPassword
        {
            get { return _repeatPassword; }
            set
            {
                SetProperty(ref _repeatPassword, value);
                this.CreateAccountCommand.ChangeCanExecute();
            }
        }

        public string ErrorText
        {
            get { return _errorText; }
            set { SetProperty(ref _errorText, value); }
        }

        protected virtual void ConfigureValidationService()
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
            this._validationService.AddValidator(
                new RegexValidator(
                    this.Phone, RegexConstants.PHONE,
                    "error_invalid_field", STRING_PHONE));

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

            this._validationService.AddValidator(
                new EqualValidator(this.Email, this.RepeatEmail, STRING_EMAIL, STRING_REPEATED_EMAIL));

        }

        protected virtual async void CreateAccountAsync()
        { 
        }

        protected virtual void EditAccount()
        {
        }

        protected virtual async void SaveAccountAsync()
        {
        }

        protected virtual void Cancel()
        {
        }

        protected virtual async void ChangePasswordAccountAsync()
        {
        }

        protected virtual async void LogOutAsync()
        {
        }

        protected virtual bool CanSaveAccount()
        {
            return !string.IsNullOrEmpty(this.Name) && !string.IsNullOrEmpty(this.Surname) && !string.IsNullOrEmpty(this.Phone)
                && !string.IsNullOrEmpty(this.Email) && !string.IsNullOrEmpty(this.RepeatEmail);
        }

        protected virtual bool CanCreateAccount()
        {
            return !string.IsNullOrEmpty(this.Name) && !string.IsNullOrEmpty(this.Surname) && !string.IsNullOrEmpty(this.Phone) 
                && !string.IsNullOrEmpty(this.Email) && !string.IsNullOrEmpty(this.RepeatEmail) 
                && !string.IsNullOrEmpty(this.Password) && !string.IsNullOrEmpty(this.RepeatPassword);
        }

    }
}

