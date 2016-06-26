using MyStyleApp.Services;
using MvvmCore;
using MyStyleApp.Validators;
using MyStyleApp.Constants;
using System.Windows.Input;
using Xamarin.Forms;
using MyStyleApp.Enums;
using MyStyleApp.Models;
using MyStyleApp.Services.Backend;

namespace MyStyleApp.ViewModels
{
    public abstract class AccountViewModelBase : NavigableViewModelBase
    {
        public ICommand CreateAccountCommand { get; private set; }
        public ICommand EditAccountCommand { get; private set; }
        public ICommand SaveAccountCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
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
        
        public AccountViewModelBase(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService) :
            base(navigator, userNotificator, localizedStringsService)
        {
            this.CreateAccountCommand = new Command(this.CreateAccountAsync);
            this.EditAccountCommand = new Command(this.EditAccount);
            this.SaveAccountCommand = new Command(this.SaveAccountAsync);
            this.CancelCommand = new Command(this.Cancel);
            this.ChangePasswordAccountCommand = new Command(this.ChangePasswordAccountAsync);
            this.LogOutCommand = new Command(this.LogOutAsync);
            
            this.Mode = AccountModeEnum.View;
        }

        protected void Initialize(User user, AccountModeEnum mode)
        {
            if (user != null)
            {
                this.Name = user.Name;
                this.Surname = user.Surname;
                this.Phone = user.Phone;
                this.Email = user.Email;
                this.RepeatEmail = this.Email;
                this.Password = "";
                this.RepeatPassword = "";
            }
            else
            {
                this.Name = "";
                this.Surname = "";
                this.Phone = "";
                this.Email = "";
                this.RepeatEmail = "";
                this.Password = "";
                this.RepeatPassword = "";
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

    }
}

