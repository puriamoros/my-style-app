using MyStyleApp.Services;
using MvvmCore;
using System.Collections.ObjectModel;
using MyStyleApp.Models;
using System.Windows.Input;
using Xamarin.Forms;
using System;
using MyStyleApp.Services.Backend;

namespace MyStyleApp.ViewModels
{
    public class SearchViewModel : NavigableViewModelBase
    {
        private ProvincesService _provincesService;
        private EstablishmentTypesService _establishmentTypesService;
        private IServiceCategoriesService _serviceCategoriesService;
        private IServicesService _servicesService;

        private ObservableCollection<Province> _provinceList;
        private Province _selectedProvince;
        private ObservableCollection<EstablishmentType> _establishmentTypeList;
        private EstablishmentType _selectedEstablishmentType;
        private ObservableCollection<ServiceCategory> _serviceCategoryList;
        private string _selectedServiceCategory;
        private ObservableCollection<Service> _serviceList;
        private Service _selectedService;

        public ICommand SearchCommand { get; private set; }

        public SearchViewModel(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService,
            ProvincesService provincesService,
            EstablishmentTypesService establishmentTypesService,
            IServiceCategoriesService serviceCategoriesService,
            IServicesService servicesService) :
            base(navigator, userNotificator, localizedStringsService)
        {
            this._provincesService = provincesService;
            this._establishmentTypesService = establishmentTypesService;
            this._serviceCategoriesService = serviceCategoriesService;
            this._servicesService = servicesService;

            this.SearchCommand = new Command(this.SearchAsync);

            this.ProvinceList = new ObservableCollection<Province>(this._provincesService.GetProvinces());
            this.EstablishmentTypeList = new ObservableCollection<EstablishmentType>(
                this._establishmentTypesService.GetEstablishmentTypes());

            this.InitializeAsync();
        }

        private async void InitializeAsync()
        {
            this.IsBusy = true;
            try
            {
                this.ServiceCategoryList = new ObservableCollection<ServiceCategory>(
                    await this._serviceCategoriesService.GetServiceCategories());
            }
            catch (Exception)
            {
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        public ObservableCollection<Province> ProvinceList
        {
            get { return _provinceList; }
            set { SetProperty(ref _provinceList, value); }
        }

        public Province SelectedProvince
        {
            get { return _selectedProvince; }
            set { SetProperty(ref _selectedProvince, value); }
        }

        public ObservableCollection<EstablishmentType> EstablishmentTypeList
        {
            get { return _establishmentTypeList; }
            set { SetProperty(ref _establishmentTypeList, value); }
        }

        public EstablishmentType SelectedEstablishmentType
        {
            get { return _selectedEstablishmentType; }
            set { SetProperty(ref _selectedEstablishmentType, value); }
        }

        public ObservableCollection<ServiceCategory> ServiceCategoryList
        {
            get { return _serviceCategoryList; }
            set { SetProperty(ref _serviceCategoryList, value); }
        }

        public string SelectedServiceCategory
        {
            get { return _selectedServiceCategory; }
            set { SetProperty(ref _selectedServiceCategory, value); }
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

        public string ChooseItemPlaceholder
        {
            get
            {
                return (Device.OS == TargetPlatform.Android) ?
                    this.LocalizedStrings.GetString("choose_an_item") : "";
            }
        }

        private async void SearchAsync()
        {
            await this.PushNavPageAsync<EstablishmentsViewModel>();
        }
    }
}
