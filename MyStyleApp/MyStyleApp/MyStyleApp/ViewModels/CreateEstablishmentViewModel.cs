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
    public class CreateEstablishmentViewModel : EstablishmentViewModelBase
    {
        private const string STRING_NAME = "name";
        private const string STRING_ADDRESS = "addres";
        private const string STRING_PHONE = "phone";
        private const string STRING_LATITUDE = "latitude";
        private const string STRING_LONGITUDE = "longitude";

        protected ValidationService _validationService;

        private IEstablishmentsService _establishmentsService;

        public CreateEstablishmentViewModel(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService,
            ValidationService validationService,
            IUsersService usersService,
            IEstablishmentsService establishmentsService) :
            base(navigator, userNotificator, localizedStringsService, usersService)
        {
            this._validationService = validationService;
            this._establishmentsService = establishmentsService;
            this.Title = this.LocalizedStrings.GetString("create_establishment");
        }

        public void Initialize()
        {
            base.Initialize(null, AccountModeEnum.Create);
        }

        protected virtual void ConfigureValidationService()
        {
            // Alwais clear validators before adding
            this._validationService.ClearValidators();

            // Name
            this._validationService.AddValidator(
                new RequiredValidator(this.Establishment.Name, STRING_NAME));
            this._validationService.AddValidator(
                new RegexValidator(
                    this.Establishment.Name, RegexConstants.NOT_INSECURE_CHARS,
                    "error_insecure_chars", STRING_NAME));
            this._validationService.AddValidator(
                new LengthValidator(this.Establishment.Name, STRING_NAME, 2, 100));

            // Address
            this._validationService.AddValidator(
                new RequiredValidator(this.Establishment.Address, STRING_ADDRESS));
            this._validationService.AddValidator(
                new RegexValidator(
                    this.Establishment.Address, RegexConstants.NOT_INSECURE_CHARS,
                    "error_insecure_chars", STRING_ADDRESS));
            this._validationService.AddValidator(
                new LengthValidator(this.Establishment.Address, STRING_ADDRESS, 2, 100));

            // Phone
            this._validationService.AddValidator(
                new RequiredValidator(this.Establishment.Phone, STRING_PHONE));
            this._validationService.AddValidator(
                new RegexValidator(
                    this.Establishment.Phone, RegexConstants.NOT_INSECURE_CHARS,
                    "error_insecure_chars", STRING_PHONE));
            this._validationService.AddValidator(
                new LengthValidator(this.Establishment.Phone, STRING_PHONE, 9, 9));
            this._validationService.AddValidator(
                new RegexValidator(
                    this.Establishment.Phone, RegexConstants.PHONE,
                    "error_invalid_field", STRING_PHONE));           
        }

        private string GetValidationError()
        {
            this.ConfigureValidationService();

            return this._validationService.GetValidationError();
        }

        protected override async void CreateEstablishmentAsync()
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
                        await this._establishmentsService.CreateEstablishmentAsync(this.Establishment);
                    }
                    catch (BackendException ex)
                    {
                        //if (ex.BackendError.State == (int)BackendStatusCodeEnum.StateDuplicatedKeyError)
                        //{
                        //    this.ErrorText = this.LocalizedStrings.GetString("error_duplicated_email");
                        //    return;
                        //}
                        //else
                        //{
                        //    throw;
                        //}
                    }

                    this.IsBusy = false;

                    await this.UserNotificator.DisplayAlert(
                       this.LocalizedStrings.GetString("created_establishment"),
                       this.LocalizedStrings.GetString("please_enter_services"),
                       this.LocalizedStrings.GetString("ok"));

                    await this.PopNavPageAsync();
                });
        }

    }
}
