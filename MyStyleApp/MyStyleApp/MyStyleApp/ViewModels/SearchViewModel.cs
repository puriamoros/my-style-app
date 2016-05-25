using System.Threading.Tasks;
using MyStyleApp.Services;
using MvvmCore;
using System.Collections.ObjectModel;
using MyStyleApp.Models;
using System.Windows.Input;
using Xamarin.Forms;
using System;

namespace MyStyleApp.ViewModels
{
    public class SearchViewModel : NavigableViewModelBase
    {
        private ObservableCollection<City> _cityList;
        private City _selectedCity;
        private ObservableCollection<EstablishmentType> _establishmenttypeList;
        private EstablishmentType _selectedEstablishmentType;
        private ObservableCollection<Service> _serviceList;
        private Service _selectedService;

        public ICommand SearchCommand { get; private set; }

        public SearchViewModel(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService) :
            base(navigator, userNotificator, localizedStringsService)
        {
            this.SearchCommand = new Command(this.Search);

            // REMOVE!!!
            FillWithMockData();
        }

        private void FillWithMockData()
        {
            var listCity = new ObservableCollection<City>();
            var listEsblishmentType = new ObservableCollection<EstablishmentType>();
            var listService = new ObservableCollection<Service>();
            for (int i = 0; i < 3; i++)
            {
                listCity.Add(new City()
                    {
                        Id = i,
                        Name = "Ciudad " + i
                    }
                );
            }
            this.CityList = listCity;
            for (int i = 0; i < 3; i++)
            {
                listEsblishmentType.Add(new EstablishmentType()
                {
                    Id = i,
                    Name = "Tipo " + i
                }
                );
            }
            this.EstablishmentTypeList = listEsblishmentType;
            for (int i = 0; i < 3; i++)
            {
                listService.Add(new Service()
                {
                    Id = i,
                    Name = "Servicio " + i
                }
                );
            }
            this.ServiceList = listService;
        }

        public ObservableCollection<City> CityList
        {
            get { return _cityList; }
            set { SetProperty(ref _cityList, value); }
        }

        public City SelectedCity
        {
            get { return _selectedCity; }
            set { SetProperty(ref _selectedCity, value); }
        }

        public ObservableCollection<EstablishmentType> EstablishmentTypeList
        {
            get { return _establishmenttypeList; }
            set { SetProperty(ref _establishmenttypeList, value); }
        }

        public EstablishmentType SelectedEstablishmentType
        {
            get { return _selectedEstablishmentType; }
            set { SetProperty(ref _selectedEstablishmentType, value); }
        }

        public ObservableCollection<Service> ServiceList
        {
            get { return _serviceList; }
            set { SetProperty(ref _serviceList, value); }
        }

        public Service SelectedService
        {
            get { return _selectedService; }
            set { SetProperty(ref _selectedService, value); }
        }

        private async void Search()
        {
            await this.PushAsync<EstablishmentsViewModel>();
        }
    }
}
