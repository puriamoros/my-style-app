using MvvmCore;
using MyStyleApp.Models;
using MyStyleApp.Services;
using MyStyleApp.Services.Backend;
using MyStyleApp.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MyStyleApp.ViewModels
{
    public class ChangePasswordViewModel : NavigableViewModelBase
    {
        private const string STRING_OLDPASSWORD = "old_password";
        private const string STRING_NEWPASSWORD = "new_password";
        private const string STRING_NEWPASSWORDREPEATED = "repeated_new_password";

        private string _oldPassword;
        private string _newPassword;
        private string _newPasswordRepeated;
        private string _errorText;

        private ValidationService _validationService;

        private IUsersService _usersService;

        public Command SavePasswordCommand { get; private set; }

        public ChangePasswordViewModel(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService,
            ValidationService validationService,
            IUsersService usersService) :
            base(navigator, userNotificator, localizedStringsService)
        {
            this.SavePasswordCommand = new Command(this.SavePasswordAsync, this.CanSavePassword);
            this._validationService = validationService;
            this._usersService = usersService;
        }

        public void Initialize()
        {
            this.OldPassword = "";
            this.NewPassword = "";
            this.NewPasswordRepeated = "";
        }

        public string OldPassword
        {
            get { return _oldPassword; }
            set
            {
                SetProperty(ref _oldPassword, value);
                this.SavePasswordCommand.ChangeCanExecute();
            }
        }

        public string NewPassword
        {
            get { return _newPassword; }
            set
            {
                SetProperty(ref _newPassword, value);
                this.SavePasswordCommand.ChangeCanExecute();
            }
        }

        public string NewPasswordRepeated
        {
            get { return _newPasswordRepeated; }
            set
            {
                SetProperty(ref _newPasswordRepeated, value);
                this.SavePasswordCommand.ChangeCanExecute();
            }
        }

        public string ErrorText
        {
            get { return _errorText; }
            set { SetProperty(ref _errorText, value); }
        }

        private string GetValidationError()
        {
            this.NewPassword = this.NewPassword.Trim();
            this.NewPasswordRepeated = this.NewPasswordRepeated.Trim();
            this.OldPassword = this.OldPassword.Trim();

            // Alwais clear validators before adding
            this._validationService.ClearValidators();

            // OldPassword
            this._validationService.AddValidator(
                new RequiredValidator(this.OldPassword, STRING_OLDPASSWORD));

            // NewPassword
            this._validationService.AddValidator(
                new RequiredValidator(this.NewPassword, STRING_NEWPASSWORD));

            // NewPasswordRepeated
            this._validationService.AddValidator(
                new RequiredValidator(this.NewPasswordRepeated, STRING_NEWPASSWORDREPEATED));
            
            // OldPassword-NewPassword
            this._validationService.AddValidator(
                new EqualValidator(this.OldPassword, this.NewPassword, STRING_OLDPASSWORD, STRING_NEWPASSWORD, true));

            // NewPassword-NewPasswordRepeated
            this._validationService.AddValidator(
                new EqualValidator(this.NewPassword, this.NewPasswordRepeated, STRING_NEWPASSWORD, STRING_NEWPASSWORDREPEATED));

            return this._validationService.GetValidationError();
        }

        public async void SavePasswordAsync()
        {
            string validationError = this.GetValidationError();

            if (validationError == null)
            {
                this.ErrorText = "";
                await this.ExecuteBlockingUIAsync(
                    async () =>
                    {
                        await this._usersService.UpdatePasswordAsync(this._usersService.LoggedUser.Id, this.OldPassword, this.NewPassword);
                        await this.PopNavPageAsync();
                    });
            }
            else
            {
                this.ErrorText = validationError;
            }
        }

        private bool CanSavePassword()
        {
            return !string.IsNullOrEmpty(this.OldPassword) && !string.IsNullOrEmpty(this.NewPassword) && !string.IsNullOrEmpty(this.NewPasswordRepeated);
        }

    }
}
