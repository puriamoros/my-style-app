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
        protected const string STRING_PASSWORD = "password";
        protected const string STRING_REPEATED_PASSWORD = "repeat_password";

        public CreateAccountViewModel(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService,
            ValidationService validationService,
            IUsersService usersService) :
            base(navigator, userNotificator, localizedStringsService, validationService, usersService)
        {
            this.Title = this.LocalizedStrings.GetString("create_account");
        }

        public void Initialize()
        {
            base.Initialize(null, BaseModeEnum.Create);
        }

        protected override void ConfigureValidationService()
        {
            base.ConfigureValidationService();

            // Password
            this._validationService.AddValidator(
                new RequiredValidator(this.Password, STRING_PASSWORD));

            // RepeatPassword
            this._validationService.AddValidator(
                new RequiredValidator(this.RepeatPassword, STRING_REPEATED_PASSWORD));

            this._validationService.AddValidator(
                new EqualValidator(this.Password, this.RepeatPassword, STRING_PASSWORD, STRING_REPEATED_PASSWORD));
        }

        private string GetValidationError()
        {
            this.ConfigureValidationService();

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
                        User user = new User()
                        {
                            Name = this.Name,
                            Surname = this.Surname,
                            Phone = this.Phone,
                            Email = this.Email,
                            Password = this.Password,
                            UserType = UserTypeEnum.Client
                        };
                        await this._usersService.RegisterUserAsync(user);
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
