using MyStyleApp.Services;
using MvvmCore;
using System.Collections.ObjectModel;
using MyStyleApp.Models;
using System.Windows.Input;
using Xamarin.Forms;
using System;
using MyStyleApp.Services.Backend;
using System.Collections.Generic;
using System.Linq;
using MyStyleApp.Enums;

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
        private ServiceCategory _selectedServiceCategory;
        private ObservableCollection<Service> _serviceList;
        private Service _selectedService;

        private IEstablishmentsService _establishmentsService;

        public Command SearchCommand { get; private set; }

        public SearchViewModel(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService,
            ProvincesService provincesService,
            EstablishmentTypesService establishmentTypesService,
            IServiceCategoriesService serviceCategoriesService,
            IServicesService servicesService,
            IEstablishmentsService establishmentsService) :
            base(navigator, userNotificator, localizedStringsService)
        {
            this._provincesService = provincesService;
            this._establishmentTypesService = establishmentTypesService;
            this._serviceCategoriesService = serviceCategoriesService;
            this._servicesService = servicesService;

            this._establishmentsService = establishmentsService;

            this.SearchCommand = new Command(this.SearchAsync, this.CanSearch);

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
                await this._serviceCategoriesService.GetServiceCategoriesAsync();
                await this._servicesService.GetServicesAsync();
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
            set
            {
                SetProperty(ref _selectedProvince, value);
                this.SearchCommand.ChangeCanExecute();
            }
        }

        public ObservableCollection<EstablishmentType> EstablishmentTypeList
        {
            get { return _establishmentTypeList; }
            set { SetProperty(ref _establishmentTypeList, value); }
        }

        public EstablishmentType SelectedEstablishmentType
        {
            get { return _selectedEstablishmentType; }
            set
            {
                SetProperty(ref _selectedEstablishmentType, value);
                this.OnSelectedEstablishmentTypeChanged(value);
            }
        }

        private async void OnSelectedEstablishmentTypeChanged(EstablishmentType selectedEstablishmentType)
        {
            this.IsBusy = true;
            try
            {
                var all = await this._serviceCategoriesService.GetServiceCategoriesAsync();
                if (selectedEstablishmentType.Id != (int)EstablishmentTypeEnum.HairdresserAndAesthetics)
                {
                    /*List<ServiceCategory> selected = new List<ServiceCategory>();
                    foreach (ServiceCategory item in all)
                    {
                        if (item.IdEstablishmentType == selectedEstablishmentType.Id)
                            selected.Add(item);
                    }*/
                    var selected = from item in all
                                   where item.IdEstablishmentType == selectedEstablishmentType.Id
                                   select item;
                    
                    this.ServiceCategoryList = new ObservableCollection<ServiceCategory>(selected);
                }  
                else
                {
                    this.ServiceCategoryList = new ObservableCollection<ServiceCategory>(all);
                }
            }
            catch(Exception ex)
            {
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        public ObservableCollection<ServiceCategory> ServiceCategoryList
        {
            get { return _serviceCategoryList; }
            set { SetProperty(ref _serviceCategoryList, value); }
        }

        public ServiceCategory SelectedServiceCategory
        {
            get { return _selectedServiceCategory; }
            set
            {
                SetProperty(ref _selectedServiceCategory, value);
                this.OnSelectedServiceCategoryChanged(value);
            }
        }

        private async void OnSelectedServiceCategoryChanged(ServiceCategory selectedServiceCategory)
        {
            this.IsBusy = true;
            try
            {
                var all = await this._servicesService.GetServicesAsync();
                List<Service> selected = new List<Service>();
                foreach (Service item in all)
                {
                    if (item.IdServiceCategory == selectedServiceCategory.Id)
                        selected.Add(item);
                }
                this.ServiceList = new ObservableCollection<Service>(selected);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        public ObservableCollection<Service> ServiceList
        {
            get { return _serviceList; }
            set { SetProperty(ref _serviceList, value); }
        }

        public Service SelectedService
        {
            get { return _selectedService; }
            set
            {
                SetProperty(ref _selectedService, value);
                this.SearchCommand.ChangeCanExecute();
            }
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
            this.IsBusy = true;
            try
            {
                //var list = await this._establishmentsService.GetEstablishmentsAsync(this.SelectedProvince, this.SelectedService);
                var list = await this._establishmentsService.GetEstablishmentsAsync(new Province(){ Id = 1 }, new Service() { Id = 1 });
                if (list.Count <= 0)
                {
                    this.IsBusy = false;
                    await this.UserNotificator.DisplayAlert(
                        this.LocalizedStrings.GetString("warning"),
                        this.LocalizedStrings.GetString("no_search_results"), 
                        this.LocalizedStrings.GetString("ok"));
                }
                else
                {
                    await this.PushNavPageAsync<EstablishmentsViewModel>((establishmentsVM) =>
                    {
                        establishmentsVM.EstablishmentsList = new ObservableCollection<Establishment>(list);
                        establishmentsVM.SelectedService = this.SelectedService;
                    });
                }
            }
            catch (Exception)
            {
                //TODO: Handle exception
            }    
            finally
            {
                this.IsBusy = false;
            }  
        }

        private bool CanSearch()
        {
            return true;//this.SelectedProvince != null && this.SelectedService != null;
        }
    }
}
