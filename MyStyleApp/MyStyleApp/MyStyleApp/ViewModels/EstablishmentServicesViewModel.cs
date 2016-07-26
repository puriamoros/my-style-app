﻿using System;
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
        private IEstablishmentsService _establishmentsService;

        private ObservableCollection<Grouping<string, SelectedService>> _groupedServiceList;

        public EstablishmentServicesViewModel(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService,
            IEstablishmentsService establishmentsService) :
            base(navigator, userNotificator, localizedStringsService)
        {
            this._establishmentsService = establishmentsService;
        }

        public void Initialize(Establishment establishment, IList<ServiceCategory> servicesCategories, IList<Service> services)
        {
            this.Establishment = establishment;

            Dictionary<int, float> establismentShortenServices = new Dictionary<int, float>();
            foreach(var shortenService in this.Establishment.ShortenServices)
            {
                establismentShortenServices.Add(shortenService.Id, shortenService.Price);
            }

            var list = new List<Grouping<string, SelectedService>>();
            foreach (var serviceCategory in servicesCategories)
            {
                var categoryServices = 
                    from service in services
                    where service.IdServiceCategory == serviceCategory.Id
                    orderby service.Name
                    select new SelectedService()
                    {
                        Id = service.Id,
                        Name = service.Name,
                        IdServiceCategory = service.IdServiceCategory,
                        Duration = service.Duration,
                        Price = (establismentShortenServices.ContainsKey(service.Id)) ? establismentShortenServices[service.Id] : 0.0f,
                        Selected = establismentShortenServices.ContainsKey(service.Id)
                    };

                list.Add(new Grouping<string, SelectedService>(serviceCategory.Name, categoryServices));
            }

            list.Sort((one, other) =>
            {
                return one.Key.CompareTo(other.Key);
            });

            this.GroupedServiceList = new ObservableCollection<Grouping<string, SelectedService>>(list);
        }

        public Establishment Establishment
        {
            get { return _establishment; }
            set { SetProperty(ref _establishment, value); }
        }

        public ObservableCollection<Grouping<string, SelectedService>> GroupedServiceList
        {
            get { return _groupedServiceList; }
            set { SetProperty(ref _groupedServiceList, value); }
        }
    }
}