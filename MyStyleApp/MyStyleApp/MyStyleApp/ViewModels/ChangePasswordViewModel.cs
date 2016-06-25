using MvvmCore;
using MyStyleApp.Services;
using MyStyleApp.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private ValidationService _validationService;

        public ChangePasswordViewModel(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService,
            ValidationService validationService) :
            base(navigator, userNotificator, localizedStringsService)
        {

        }

        public string OldPassword
        {
            get { return _oldPassword; }
            set { SetProperty(ref _oldPassword, value); }
        }

        public string NewPassword
        {
            get { return _newPassword; }
            set { SetProperty(ref _newPassword, value); }
        }

        public string NewPasswordRepeated
        {
            get { return _newPasswordRepeated; }
            set { SetProperty(ref _newPasswordRepeated, value); }
        }

        private string GetValidationError()
        {
            // Alwais clear validators before adding
            this._validationService.ClearValidators();

            //hay q cambiar el string q pasamos para cada caso???

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

    }
}
