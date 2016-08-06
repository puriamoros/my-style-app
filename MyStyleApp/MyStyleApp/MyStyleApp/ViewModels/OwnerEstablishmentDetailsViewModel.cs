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
using System.Globalization;

namespace MyStyleApp.ViewModels
{
    public class OwnerEstablishmentDetailsViewModel : EstablishmentViewModelBase
    {
        IUsersService _usersService;
        private IEstablishmentsService _establishmentsService;
        private IServicesService _servicesService;
        private IServiceCategoriesService _serviceCategoriesService;

        public OwnerEstablishmentDetailsViewModel(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService,
            ProvincesService provincesService,
            ValidationService validationService,
            IUsersService usersService,
            IEstablishmentsService establishmentsService,
            IServicesService servicesService,
            IServiceCategoriesService serviceCategoriesService) :
            base(navigator, userNotificator, localizedStringsService, provincesService, validationService, usersService, servicesService, serviceCategoriesService)
        {
            this._usersService = usersService;
            this._establishmentsService = establishmentsService;
            this._servicesService = servicesService;
            this._serviceCategoriesService = serviceCategoriesService;

            this.Title = this.LocalizedStrings.GetString("create_establishment");
        }

        public void Initialize(Establishment establishment)
        {
            base.Initialize(establishment, BaseModeEnum.View);
        }

        protected override async void SaveEstablishmentAsync()
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
                        Hours1 = string.Format("{0}:{1}-{2}:{3}",Hours1Start.Hours, Hours1Start.Minutes, Hours1End.Hours, Hours1Start.Minutes),
                        Hours2 = string.Format("{0}:{1}-{2}:{3}", Hours2Start.Hours, Hours2Start.Minutes, Hours2End.Hours, Hours2Start.Minutes),
                        ConfirmType = (this.AutoConfirm) ? ConfirmTypeEnum.Automatic : ConfirmTypeEnum.Manual,
                        Concurrence = int.Parse(this.Concurrence),
                        Latitude = double.Parse(this.Latitude, CultureInfo.InvariantCulture),
                        Longitude = double.Parse(this.Longitude, CultureInfo.InvariantCulture),
                        IdEstablishmentType = (int) this._establishmentType,
                        ShortenServices = this._shortenServices
                    };

                    await this._establishmentsService.UpdateEstablishmentAsync(establishment);

                    this.IsBusy = false;

                    // mandar mensaje a lista de establecimientos

                    await this.PopNavPageAsync();
                });
        }

        protected override void EditEstablishment()
        {
            this.Mode = BaseModeEnum.Edit;
        }

        protected override void Cancel()
        {
            //this.Initialize(this.);
            this.Mode = BaseModeEnum.View;
        }

    }
}
