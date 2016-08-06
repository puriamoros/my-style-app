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
    public class CreateEstablishmentViewModel : EstablishmentViewModelBase
    {
        public CreateEstablishmentViewModel(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService,
            ProvincesService provincesService,
            ValidationService validationService,
            IUsersService usersService,
            IServicesService servicesService,
            IServiceCategoriesService serviceCategoriesService,
            IEstablishmentsService establishmentsService) :
            base(navigator, userNotificator, localizedStringsService, provincesService, 
                validationService, usersService, servicesService, serviceCategoriesService, establishmentsService)
        {
            this.Title = this.LocalizedStrings.GetString("create_establishment");
        }

        public void Initialize()
        {
            base.InitializeAsync(null, BaseModeEnum.Create);
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
                    var establishment = this.CreateEstablishmentFromData();

                    var newEstablishment = await this._establishmentsService.CreateEstablishmentAsync(establishment);

                    newEstablishment.ProvinceName = SelectedProvince.Name;
                    MessagingCenter.Send<Establishment>(newEstablishment, "establishmentModified");

                    await this.PopNavPageAsync();
                });
        }

    }
}
