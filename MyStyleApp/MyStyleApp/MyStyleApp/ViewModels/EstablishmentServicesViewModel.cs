using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCore;
using MyStyleApp.Services;
using MyStyleApp.Models;
using MyStyleApp.Utils;
using MyStyleApp.Services.Backend;
using System.Collections.ObjectModel;

namespace MyStyleApp.ViewModels
{
    public class EstablishmentServicesViewModel : NavigableViewModelBase
    {
        private Establishment _establishment;
        private ServicesService _servicesService;
        private ServiceCategoriesService _serviceCategoriesService;

        private ObservableCollection<Grouping<string, Service>> _groupedServiceList;

        public EstablishmentServicesViewModel(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService,
            ServicesService servicesService,
            ServiceCategoriesService serviceCategoriesService) :
            base(navigator, userNotificator, localizedStringsService)
        {
            this._servicesService = servicesService;
            this._serviceCategoriesService = serviceCategoriesService;

            this.InitializeAsync(null);
        }

        public async void InitializeAsync(Establishment establishment)
        {
            this.Establishment = establishment;

            var servicesCategories = await this._serviceCategoriesService.GetServiceCategoriesAsync();
            var services = await this._servicesService.GetServicesAsync();

            var list = new List<Grouping<string, Service>>();
            foreach(var serviceCategory in servicesCategories)
            {
                var categoryServices = from service in services
                                       where service.IdServiceCategory == serviceCategory.Id
                                       orderby service.Name
                                       select service;

                list.Add(new Grouping<string, Service>(serviceCategory.Name, categoryServices));
            }

            list.Sort((one, other) =>
            {
                return one.Key.CompareTo(other.Key);
            });

            this.GroupedServiceList = new ObservableCollection<Grouping<string, Service>>(list);
        }

        public Establishment Establishment
        {
            get { return _establishment; }
            set { SetProperty(ref _establishment, value); }
        }

        public ObservableCollection<Grouping<string, Service>> GroupedServiceList
        {
            get { return _groupedServiceList; }
            set { SetProperty(ref _groupedServiceList, value); }
        }
    }
}
