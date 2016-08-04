using System.Threading.Tasks;
using MyStyleApp.Services;
using MvvmCore;
using MyStyleApp.Validators;
using MyStyleApp.Constants;
using System;
using System.Linq;
using Xamarin.Forms;
using MyStyleApp.Enums;
using MyStyleApp.Models;
using MyStyleApp.Services.Backend;
using MyStyleApp.Exceptions;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace MyStyleApp.ViewModels
{
    public class CreateEstablishmentViewModel : EstablishmentViewModelBase
    {
        IUsersService _usersService;
        private IEstablishmentsService _establishmentsService;
        private IServicesService _servicesService;
        private IServiceCategoriesService _serviceCategoriesService;

        public CreateEstablishmentViewModel(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService,
            ProvincesService provincesService,
            ValidationService validationService,
            IUsersService usersService,
            IEstablishmentsService establishmentsService,
            IServicesService servicesService,
            IServiceCategoriesService serviceCategoriesService) :
            base(navigator, userNotificator, localizedStringsService, provincesService, validationService, usersService)
        {
            this._usersService = usersService;
            this._establishmentsService = establishmentsService;
            this._servicesService = servicesService;
            this._serviceCategoriesService = serviceCategoriesService;

            this.Title = this.LocalizedStrings.GetString("create_establishment");
        }

        public void Initialize()
        {
            base.Initialize(null, BaseModeEnum.Create);
            this.SelectedProvince = null;
        }

        protected override async void OfferedServices()
        {
            await this.ExecuteBlockingUIAsync(
                async () =>
                {
                    Establishment establishment = null;
                    var servicesCategories = await this._serviceCategoriesService.GetServiceCategoriesAsync();
                    var services = await this._servicesService.GetServicesAsync();

                    await this.PushNavPageModalAsync<EstablishmentServicesViewModel>((establishmentServicesVM) =>
                    {
                        establishmentServicesVM.Initialize(establishment, servicesCategories, services, this.SetShortenServicesList);
                    });
                });
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
                    Establishment establishment = new Establishment()
                    {
                        IdOwner = this._usersService.LoggedUser.Id,
                        Name = this.Name,
                        IdProvince = this.SelectedProvince.Id,
                        Address = this.Address,
                        Phone = this.Phone,
                        Hours1 = "",
                        Hours2 = "",
                        ConfirmType = (this.AutoConfirm) ? ConfirmTypeEnum.Automatic : ConfirmTypeEnum.Manual,
                        Concurrence = int.Parse(this.Concurrence),
                        Latitude = double.Parse(this.Latitude),
                        Longitude = double.Parse(this.Longitude),
                        IdEstablishmentType = (int) this._establishmentType,
                        ShortenServices = this._shortenServices
                    };

                    var newEstablishment = await this._establishmentsService.CreateEstablishmentAsync(establishment);

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
