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
    public class CreateAccountViewModel : AccountViewModelBase
    {
        private const string STRING_NAME = "name";
        private const string STRING_SURNAME = "surname";
        private const string STRING_PHONE = "phone";
        private const string STRING_EMAIL = "email";
        private const string STRING_REPEATED_EMAIL = "repeat_email";
        private const string STRING_PASSWORD = "password";
        private const string STRING_REPEATED_PASSWORD = "repeat_password";

        private ValidationService _validationService;

        private IUsersService _usersService;

        public CreateAccountViewModel(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService,
            ValidationService validationService,
            IUsersService usersService) :
            base(navigator, userNotificator, localizedStringsService, usersService)
        {
            this._validationService = validationService;
            this._usersService = usersService;
            this.Title = this.LocalizedStrings.GetString("create_account");
        }

        public void Initialize(User user)
        {
            base.Initialize(user, AccountModeEnum.Create);
        }

        private string GetValidationError()
        {
            // Alwais clear validators before adding
            this._validationService.ClearValidators();

            // Name
            this._validationService.AddValidator(
                new RequiredValidator(this.User.Name, STRING_NAME));
            this._validationService.AddValidator(
                new RegexValidator(
                    this.User.Name, RegexConstants.NOT_INSECURE_CHARS,
                    "error_insecure_chars", STRING_NAME));
            this._validationService.AddValidator(
                new LengthValidator(this.User.Name, STRING_NAME, 2, 100));

            // Surname
            this._validationService.AddValidator(
                new RequiredValidator(this.User.Surname, STRING_SURNAME));
            this._validationService.AddValidator(
                new RegexValidator(
                    this.User.Surname, RegexConstants.NOT_INSECURE_CHARS,
                    "error_insecure_chars", STRING_SURNAME));
            this._validationService.AddValidator(
                new LengthValidator(this.User.Surname, STRING_SURNAME, 2, 100));

            // Phone
            this._validationService.AddValidator(
                new RequiredValidator(this.User.Phone, STRING_PHONE));
            this._validationService.AddValidator(
                new RegexValidator(
                    this.User.Phone, RegexConstants.NOT_INSECURE_CHARS,
                    "error_insecure_chars", STRING_PHONE));
            this._validationService.AddValidator(
                new LengthValidator(this.User.Phone, STRING_PHONE, 9, 9));
            this._validationService.AddValidator(
                new RegexValidator(
                    this.User.Phone, RegexConstants.PHONE,
                    "error_invalid_field", STRING_PHONE));

            // Email
            this._validationService.AddValidator(
                new RequiredValidator(this.User.Email, STRING_EMAIL));
            this._validationService.AddValidator(
                new RegexValidator(
                    this.User.Email, RegexConstants.NOT_INSECURE_CHARS,
                    "error_insecure_chars", STRING_EMAIL));
            this._validationService.AddValidator(
                new RegexValidator(
                    this.User.Email, RegexConstants.EMAIL,
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
                new EqualValidator(this.User.Email, this.RepeatEmail, STRING_EMAIL, STRING_REPEATED_EMAIL));

            // Password
            this._validationService.AddValidator(
                new RequiredValidator(this.User.Password, STRING_PASSWORD));

            // RepeatPassword
            this._validationService.AddValidator(
                new RequiredValidator(this.RepeatPassword, STRING_REPEATED_PASSWORD));

            this._validationService.AddValidator(
                new EqualValidator(this.User.Password, this.RepeatPassword, STRING_PASSWORD, STRING_REPEATED_PASSWORD));

            return this._validationService.GetValidationError();
        }

        protected override async void CreateAccountAsync()
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
                    try
                    {
                        await this._usersService.RegisterUserAsync(this.User);
                    }
                    catch (BackendException ex)
                    {
                        if (ex.BackendError.State == (int) BackendStatusCodeEnum.StateDuplicatedKeyError)
                        {
                            this.ErrorText = this.LocalizedStrings.GetString("error_duplicated_email");
                            return;
                        }
                        else
                        {
                            throw;
                        }
                    }
                    
                    this.IsBusy = false;

                    await this.UserNotificator.DisplayAlert(
                       this.LocalizedStrings.GetString("created_account"),
                       this.LocalizedStrings.GetString("please_login"),
                       this.LocalizedStrings.GetString("ok"));

                    await this.PopNavPageAsync();
                });

            
        }

    }
}
