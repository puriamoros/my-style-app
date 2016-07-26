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
        IUsersService _usersService;

        public Command CreateAccountCommand { get; private set; }
        public Command EditAccountCommand { get; private set; }
        public Command SaveAccountCommand { get; private set; }
        public Command CancelCommand { get; private set; }
        public Command ChangePasswordAccountCommand { get; private set; }
        public Command LogOutCommand { get; private set; }

        private User _user;
        private string _repeatEmail;
        private string _repeatPassword;
        private string _errorText;

        private ObservableCollection<UserType> _userTypeList;
        private UserType _selectedUserType;

        private ObservableCollection<Establishment> _establishmentList;
        private Establishment _selectedEstablishment;

        private AccountModeEnum _mode;
        
        public AccountViewModelBase(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService,
            IUsersService usersService) :
            base(navigator, userNotificator, localizedStringsService)
        {
            this._usersService = usersService;

            this.CreateAccountCommand = new Command(this.CreateAccountAsync, this.CanCreateAccount);
            this.EditAccountCommand = new Command(this.EditAccount);
            this.SaveAccountCommand = new Command(this.SaveAccountAsync, this.CanSaveAccount);
            this.CancelCommand = new Command(this.Cancel);
            this.ChangePasswordAccountCommand = new Command(this.ChangePasswordAccountAsync);
            this.LogOutCommand = new Command(this.LogOutAsync);
            
            this.Mode = AccountModeEnum.View;
            
        }

        protected void Initialize(User user, AccountModeEnum mode)
        {
            if (user != null)
            {
                this.User = new User()
                {
                    Id = user.Id,
                    Name = user.Name,
                    Surname = user.Surname,
                    Phone = user.Phone,
                    Email = user.Email,
                    UserType = user.UserType
                };
                this.RepeatEmail = this.User.Email;
             }
            else
            {
                this.User = new User()
                {
                    Name = "",
                    Surname = "",
                    Phone = "",
                    Email = "",
                    UserType = UserTypeEnum.Client
                };
                this.RepeatEmail = "";
            }
            this.User.Password = "";
            this.RepeatPassword = "";
            this.ErrorText = "";

            this.Mode = mode;
        }

        public User User
        {
            get { return _user; }
            set { SetProperty(ref _user, value); }
        }

        public AccountModeEnum Mode
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
            return !string.IsNullOrEmpty(this.User.Name) && !string.IsNullOrEmpty(this.User.Surname) && !string.IsNullOrEmpty(this.User.Phone)
                && !string.IsNullOrEmpty(this.User.Email) && !string.IsNullOrEmpty(this.RepeatEmail);
        }

        protected virtual bool CanCreateAccount()
        {
            return !string.IsNullOrEmpty(this.User.Name) && !string.IsNullOrEmpty(this.User.Surname) && !string.IsNullOrEmpty(this.User.Phone) 
                && !string.IsNullOrEmpty(this.User.Email) && !string.IsNullOrEmpty(this.RepeatEmail) 
                && !string.IsNullOrEmpty(this.User.Password) && !string.IsNullOrEmpty(this.RepeatPassword);
        }

    }
}

