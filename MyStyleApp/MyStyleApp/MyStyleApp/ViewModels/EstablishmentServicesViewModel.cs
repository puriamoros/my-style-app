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
using MyStyleApp.Enums;
using System.Globalization;
using System.Text.RegularExpressions;
using MyStyleApp.Constants;

namespace MyStyleApp.ViewModels
{
    public class EstablishmentServicesViewModel : NavigableViewModelBase
    {
        private IEstablishmentsService _establishmentsService;

        private string _searchText;
        private string _errorText;
        private bool _isClearSearchVisible;
        private Dictionary<int, EstablishmentTypeEnum> _establishmentTypeByServiceCategory;
        private List<Grouping<string, SelectedService>> _servicesList;
        private Action<EstablishmentTypeEnum, List<ShortenService>> _resultsAction;

        public ICommand ClearSearchCommand { get; private set; }
        public ICommand SelectionDoneCommand { get; private set; }

        private ObservableCollection<Grouping<string, SelectedService>> _groupedServiceList;

        public EstablishmentServicesViewModel(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService,
            IEstablishmentsService establishmentsService) :
            base(navigator, userNotificator, localizedStringsService)
        {
            this._establishmentsService = establishmentsService;
            this.ClearSearchCommand = new Command(this.ClearSearch);
            this.SelectionDoneCommand = new Command(this.SelectionDone);
        }

        public void Initialize(
            IList<ShortenService> shortenServices,
            IList<ServiceCategory> serviceCategories,
            IList<Service> services,
            Action<EstablishmentTypeEnum, List<ShortenService>> resultsAction)
        {
            this._resultsAction = resultsAction;
            this.SearchText = null;
            this.IsClearSearchVisible = false;

            // Services of each service category: IdServiceCategory -> service list
            Dictionary<int, List<Service>> establismentServices = new Dictionary<int, List<Service>>();
            foreach (var service in services)
            {
                if (!establismentServices.ContainsKey(service.IdServiceCategory))
                {
                    establismentServices.Add(service.IdServiceCategory, new List<Service>());
                }
                establismentServices[service.IdServiceCategory].Add(service);
            }

            // Price of each offered service: IdService -> price
            Dictionary<int, float> establismentOfferedServices = new Dictionary<int, float>();
            if (shortenServices != null)
            {
                foreach (var shortenService in shortenServices)
                {
                    establismentOfferedServices.Add(shortenService.Id, shortenService.Price);
                }
            }

            this._servicesList = new List<Grouping<string, SelectedService>>();
            this._establishmentTypeByServiceCategory = new Dictionary<int, EstablishmentTypeEnum>();
            foreach (var serviceCategory in serviceCategories)
            {
                this._establishmentTypeByServiceCategory.Add(serviceCategory.Id, serviceCategory.EstablishmentType);
                var selectedServiceList = new List<SelectedService>();
                foreach (var service in establismentServices[serviceCategory.Id])
                {
                    var servicePrice = (establismentOfferedServices.ContainsKey(service.Id)) ? establismentOfferedServices[service.Id] : 0;
                    var selectedService = new SelectedService()
                    {
                        Id = service.Id,
                        Name = service.Name,
                        IdServiceCategory = service.IdServiceCategory,
                        EstablismentType = serviceCategory.EstablishmentType,
                        Duration = service.Duration,
                        Price = servicePrice,
                        PriceStr = (servicePrice != 0) ? servicePrice.ToString("0.00").Replace(',','.') : "",
                        Selected = (servicePrice != 0),
                        IsVisible = true,
                        HeightRequest = -1,
                        IsEnabled = true
                    };
                    selectedServiceList.Add(selectedService);
                }
                selectedServiceList.Sort((one, other) =>
                {
                    return one.Name.CompareTo(other.Name);
                });
                this._servicesList.Add(new Grouping<string, SelectedService>(serviceCategory.Name, selectedServiceList));
            }

            this._servicesList.Sort((one, other) =>
            {
                return one.Key.CompareTo(other.Key);
            });

            this.GroupedServiceList = new ObservableCollection<Grouping<string, SelectedService>>(this._servicesList);
        }

        public ObservableCollection<Grouping<string, SelectedService>> GroupedServiceList
        {
            get { return _groupedServiceList; }
            set { SetProperty(ref _groupedServiceList, value); }
        }

        public string ErrorText
        {
            get { return _errorText; }
            set { SetProperty(ref _errorText, value); }
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

        public bool IsClearSearchVisible
        {
            get { return _isClearSearchVisible; }
            set { SetProperty(ref _isClearSearchVisible, value); }
        }

        private void OnSearchTextChanged()
        {
            if (this.GroupedServiceList != null && this.SearchText != null)
            {
                var text = this.SearchText.ToLower();
                this.IsClearSearchVisible = text.Length > 0;
                foreach(var grouping in this.GroupedServiceList)
                {
                    foreach (var service in grouping)
                    {
                        bool hasText = service.Name.ToLower().Contains(text);
                        if(Device.OS == TargetPlatform.iOS)
                        {
                            // In iOS it is not possible to hide a list row, so we disable it
                            service.IsEnabled = hasText;
                        }
                        else
                        {
                            // In the other platforms we can hide the list row
                            service.IsVisible = hasText;
                            service.HeightRequest = (hasText) ? -1 : 0;
                        }
                    }
                }
            }
        }

        private void ClearSearch()
        {
            if(this.SearchText != null)
            {
                this.SearchText = "";
            }
            this.IsClearSearchVisible = false;
        }

        private async void SelectionDone()
        {
            this.ErrorText = "";

            if(this._resultsAction != null)
            {
                try
                {
                    EstablishmentTypeEnum establishmentType = EstablishmentTypeEnum.Unknown;
                    List<ShortenService> shortenServices = new List<ShortenService>();
                    foreach (var grouping in this._servicesList)
                    {
                        bool hasServiceSelected = false;
                        foreach (var service in grouping)
                        {
                            if (service.Selected)
                            {
                                service.PriceStr = service.PriceStr.Replace(',', '.');

                                float price;
                                if (!float.TryParse(service.PriceStr, out price) || price <= 0)
                                {
                                    throw new FormatException(this.LocalizedStrings.GetString("error_service_price", "${SERVICE_NAME}", service.Name));
                                }

                                hasServiceSelected = true;
                                var shortenService = new ShortenService()
                                {
                                    Id = service.Id,
                                    Price = float.Parse(service.PriceStr, CultureInfo.InvariantCulture)
                                };
                                shortenServices.Add(shortenService);
                            }
                        }

                        if (hasServiceSelected)
                        {
                            var groupEstablishmentType = this._establishmentTypeByServiceCategory[grouping[0].IdServiceCategory];
                            if (establishmentType == EstablishmentTypeEnum.Unknown)
                            {
                                establishmentType = groupEstablishmentType;
                            }
                            else if (establishmentType != groupEstablishmentType)
                            {
                                establishmentType = EstablishmentTypeEnum.HairdresserAndAesthetics;
                            }
                        }
                    }

                    this._resultsAction(establishmentType, shortenServices);

                    if (Device.OS == TargetPlatform.Windows || Device.OS == TargetPlatform.WinPhone)
                    {
                        await this.PopNavPageAsync();
                    }
                    else
                    {
                        await this.PopNavPageModalAsync();
                    }
                }
                catch (FormatException ex)
                {
                    this.ErrorText = ex.Message;
                }
            }
        }
    }
}
