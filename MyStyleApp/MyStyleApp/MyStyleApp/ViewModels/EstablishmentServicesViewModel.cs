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
using System.Windows.Input;
using Xamarin.Forms;

namespace MyStyleApp.ViewModels
{
    public class EstablishmentServicesViewModel : NavigableViewModelBase
    {
        private Establishment _establishment;
        private IEstablishmentsService _establishmentsService;

        private string _searchText;
        private bool _isCancelSearchVisible;

        public ICommand CancelSearchCommand { get; private set; }

        private ObservableCollection<Grouping<string, SelectedService>> _groupedServiceList;

        public EstablishmentServicesViewModel(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService,
            IEstablishmentsService establishmentsService) :
            base(navigator, userNotificator, localizedStringsService)
        {
            this._establishmentsService = establishmentsService;
            this.CancelSearchCommand = new Command(this.CancelSearch);
        }

        public void Initialize(Establishment establishment, IList<ServiceCategory> serviceCategories, IList<Service> services)
        {
            this.Establishment = establishment;
            this.SearchText = null;
            this.IsCancelSearchVisible = false;

            Dictionary<int, float> establismentShortenServices = new Dictionary<int, float>();
            foreach(var shortenService in this.Establishment.ShortenServices)
            {
                establismentShortenServices.Add(shortenService.Id, shortenService.Price);
            }

            Dictionary<int, List<Service>> establismentServices = new Dictionary<int, List<Service>>();
            foreach (var service in services)
            {
                service.Price = (establismentShortenServices.ContainsKey(service.Id)) ? establismentShortenServices[service.Id] : 0;
                if(!establismentServices.ContainsKey(service.IdServiceCategory))
                {
                    establismentServices.Add(service.IdServiceCategory, new List<Service>());
                }
                establismentServices[service.IdServiceCategory].Add(service);
            }

            var list = new List<Grouping<string, SelectedService>>();
            foreach(var serviceCategory in serviceCategories)
            {
                var selectedServiceList = new List<SelectedService>();
                foreach (var service in establismentServices[serviceCategory.Id])
                {
                    var selectedService = new SelectedService()
                    {
                        Id = service.Id,
                        Name = service.Name,
                        IdServiceCategory = service.IdServiceCategory,
                        Duration = service.Duration,
                        Price = service.Price,
                        PriceStr = (establismentShortenServices.ContainsKey(service.Id)) ? service.Price.ToString("0.00") : "",
                        Selected = establismentShortenServices.ContainsKey(service.Id),
                        IsVisible = true,
                        HeightRequest = -1
                    };
                    selectedServiceList.Add(selectedService);
                }
                selectedServiceList.Sort((one, other) =>
                {
                    return one.Name.CompareTo(other.Name);
                });
                list.Add(new Grouping<string, SelectedService>(serviceCategory.Name, selectedServiceList));
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

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                SetProperty(ref _searchText, value);
                this.OnSearchTextChanged();
            }
        }

        public bool IsCancelSearchVisible
        {
            get { return _isCancelSearchVisible; }
            set { SetProperty(ref _isCancelSearchVisible, value); }
        }

        private void OnSearchTextChanged()
        {
            if (this.GroupedServiceList != null && this.SearchText != null)
            {
                var text = this.SearchText.ToLower();
                this.IsCancelSearchVisible = text.Length > 0;
                foreach(var grouping in this.GroupedServiceList)
                {
                    foreach (var service in grouping)
                    {
                        bool show = service.Name.ToLower().Contains(text);
                        service.IsVisible = show;
                        service.HeightRequest = (show) ? -1 : 0;
                    }
                }
            }
        }

        private void CancelSearch()
        {
            if(this.SearchText != null)
            {
                this.SearchText = "";
            }
            this.IsCancelSearchVisible = false;
        }
    }
}
