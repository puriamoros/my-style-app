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
using MyStyleApp.Exceptions;

namespace MyStyleApp.ViewModels
{
    public class AccountDetailsViewModel : AccountViewModelBase
    {
        private const string STRING_NAME = "name";
        private const string STRING_SURNAME = "surname";
        private const string STRING_PHONE = "phone";
        private const string STRING_EMAIL = "email";
        private const string STRING_REPEATED_EMAIL = "repeat_email";
        private const string STRING_PASSWORD = "password";

        private ValidationService _validationService;

        private IUsersService _usersService;


        public AccountDetailsViewModel(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService,
            ValidationService validationService,
            IUsersService usersService) :
            base(navigator, userNotificator, localizedStringsService)
        {
            this._validationService = validationService;
            this._usersService = usersService;
            this.Title = this.LocalizedStrings.GetString("my_account");

            MessagingCenter.Subscribe<string>(this, "changeAccountMode", this.OnAccountModeChanged);
        }

        public void Initialize(User user)
        {
            base.Initialize(user, AccountModeEnum.View);
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
 
            return this._validationService.GetValidationError();
        }

        protected override void EditAccount()
        {
            this.Mode = AccountModeEnum.Edit;
        }

        protected override async void SaveAccountAsync()
        {
            string validationError = this.GetValidationError();

            if (validationError == null)
            {
                this.ErrorText = "";
                await this.ExecuteBlockingUIAsync(
                    async () =>
                    {
                        User updateUser = new User();
                        updateUser.Name = this.Name;
                        updateUser.Surname = this.Surname;
                        updateUser.Email = this.Email;
                        updateUser.Phone = this.Phone;
                        updateUser.UserType = this._usersService.LoggedUser.UserType;
                        
                        try
                        {
                            await this._usersService.UpdateUserAsync(this._usersService.LoggedUser.Id, updateUser);
                        }
                        catch (BackendException ex)
                        {
                            if (ex.BackendError.State == (int)BackendStatusCodeEnum.StateDuplicatedKeyError)
                            {
                                this.ErrorText = this.LocalizedStrings.GetString("error_duplicated_email");
                                return;
                            }
                            else
                            {
                                throw;
                            }
                        }


                        this.Mode = AccountModeEnum.View;

                        try
                        {
                            await this._usersService.MeAsync();
                            this.Initialize(this._usersService.LoggedUser);
                        }
                        catch
                        {
                            await this._usersService.LogoutAsync();
                            await this.SetMainPageNavPageAsync<LoginViewModel>();
                        }
                    });
            }
            else
            {
                this.ErrorText = validationError;
            } 
        }

        private void OnAccountModeChanged(string accountModeStr)
        {
            AccountModeEnum accountMode;
            if (Enum.TryParse(accountModeStr, out accountMode))
            {
                if (accountMode == AccountModeEnum.Edit || accountMode == AccountModeEnum.View)
                {
                    this.Initialize(this._usersService.LoggedUser);
                    this.Mode = accountMode;
                }
            }
        }

        protected override void Cancel()
        {
            this.Initialize(this._usersService.LoggedUser);
            this.Mode = AccountModeEnum.View;
        }

        protected override async void ChangePasswordAccountAsync()
        {
            await this.ExecuteBlockingUIAsync(
                async () =>
                {
                    await this.PushNavPageAsync<ChangePasswordViewModel>((changePVM) =>
                    {
                        changePVM.Initialize();
                    });
                });
        }

        protected override async void LogOutAsync()
        {
            await this.ExecuteBlockingUIAsync(
                async () =>
                {
                    await this._usersService.LogoutAsync();
                    await this.SetMainPageNavPageAsync<LoginViewModel>();
                });
        }
    }
}
