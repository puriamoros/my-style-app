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
    public class CreateAccountViewModel : AccountViewModelBase
    {
        public CreateAccountViewModel(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService,
            ValidationService validationService,
            IUsersService usersService) :
            base(navigator, userNotificator, localizedStringsService)
        {

        }

        public void Initialize(User user)
        {
            base.Initialize(user, AccountModeEnum.Create);
        }

        //private string GetValidationError()
        //{
        //    // Alwais clear validators before adding
        //    this._validationService.ClearValidators();

        //    // Name
        //    this._validationService.AddValidator(
        //        new RequiredValidator(this.Name, STRING_NAME));
        //    this._validationService.AddValidator(
        //        new RegexValidator(
        //            this.Name, RegexConstants.NOT_INSECURE_CHARS,
        //            "error_insecure_chars", STRING_NAME));
        //    this._validationService.AddValidator(
        //        new LengthValidator(this.Name, STRING_NAME, 2, 100));

        //    // Surname
        //    this._validationService.AddValidator(
        //        new RequiredValidator(this.Surname, STRING_SURNAME));
        //    this._validationService.AddValidator(
        //        new RegexValidator(
        //            this.Surname, RegexConstants.NOT_INSECURE_CHARS,
        //            "error_insecure_chars", STRING_SURNAME));
        //    this._validationService.AddValidator(
        //        new LengthValidator(this.Surname, STRING_SURNAME, 2, 100));

        //    // Phone
        //    this._validationService.AddValidator(
        //        new RequiredValidator(this.Phone, STRING_PHONE));
        //    this._validationService.AddValidator(
        //        new RegexValidator(
        //            this.Phone, RegexConstants.NOT_INSECURE_CHARS,
        //            "error_insecure_chars", STRING_PHONE));
        //    this._validationService.AddValidator(
        //        new LengthValidator(this.Phone, STRING_PHONE, 9, 9));

        //    // Email
        //    this._validationService.AddValidator(
        //        new RequiredValidator(this.Email, STRING_EMAIL));
        //    this._validationService.AddValidator(
        //        new RegexValidator(
        //            this.Email, RegexConstants.NOT_INSECURE_CHARS,
        //            "error_insecure_chars", STRING_EMAIL));
        //    this._validationService.AddValidator(
        //        new RegexValidator(
        //            this.Email, RegexConstants.EMAIL,
        //            "error_invalid_field", STRING_EMAIL));

        //    // RepeatEmail
        //    this._validationService.AddValidator(
        //        new RequiredValidator(this.RepeatEmail, STRING_EMAIL));
        //    this._validationService.AddValidator(
        //        new RegexValidator(
        //            this.RepeatEmail, RegexConstants.NOT_INSECURE_CHARS,
        //            "error_insecure_chars", STRING_EMAIL));
        //    this._validationService.AddValidator(
        //        new RegexValidator(
        //            this.RepeatEmail, RegexConstants.EMAIL,
        //            "error_invalid_field", STRING_EMAIL));

        //    //TODO: comprobar q email y repeat email son iguales. Idem con password y repeatpassword

        //    // Password
        //    this._validationService.AddValidator(
        //        new RequiredValidator(this.Password, STRING_PASSWORD));

        //    // RepeatPassword
        //    this._validationService.AddValidator(
        //        new RequiredValidator(this.RepeatPassword, STRING_PASSWORD));

        //    return this._validationService.GetValidationError();
        //}

        //private async void CreateAccountAsync()
        //{
        //    string validationError = this.GetValidationError();
        //    if (validationError != null)
        //    {
        //        this.ErrorText = this.LocalizedStrings.GetString("error") + ": " + validationError;
        //        return;
        //    }

        //    this.ErrorText = "";

        //    await this.ExecuteBlockingUIAsync(
        //        async () =>
        //        {
        //            //await this._usersService.LoginAsync(this.Email, this.Password, this.RememberMe);
        //            //await this.SetMainPageAsync<MainViewModel>();
        //        });

        //    //this.IsBusy = true;
        //    //try
        //    //{
        //    //    //await this._usersService.LoginAsync(this.Email, this.Password, this.RememberMe);
        //    //    //await this.SetMainPageAsync<MainViewModel>();
        //    //}
        //    //catch (Exception)
        //    //{
        //    //    //ErrorText = this.LocalizedStrings[STRING_LOGIN_ERROR];
        //    //}
        //    //finally
        //    //{
        //    //    this.IsBusy = false;
        //    //}
        //}

    }
}
