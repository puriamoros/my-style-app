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
    public class EstablishmentSearchViewModel : NavigableViewModelBase
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

        public EstablishmentSearchViewModel(
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

            this.SubscribeToMessages();
        }

        private void SubscribeToMessages()
        {
            MessagingCenter.Subscribe<string>(this, "userLogin", (userType) =>
            {
                if (userType == UserTypeEnum.Client.ToString())
                {
                }
            });
            MessagingCenter.Subscribe<string>(this, "userLogout", async (userType) =>
            {
                if (userType == UserTypeEnum.Client.ToString())
                {
                    await this.PopNavPageToRootAsync();
                }
            });
        }

        public async void InitializeAsync()
        {
            this.SelectedProvince = null;
            this.SelectedEstablishmentType = null;
            this.SelectedServiceCategory = null;
            this.SelectedService = null;
            await this.ExecuteBlockingUIAsync(
                async () =>
                {
                    await this._serviceCategoriesService.GetServiceCategoriesAsync();
                    await this._servicesService.GetServicesAsync();
                });
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
            await this.ExecuteBlockingUIAsync(
                async () =>
                {
                    if(selectedEstablishmentType != null)
                    {
                        var all = await this._serviceCategoriesService.GetServiceCategoriesAsync();
                        if (selectedEstablishmentType.Id != (int)EstablishmentTypeEnum.HairdresserAndAesthetics)
                        {
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
                    else if(this.ServiceCategoryList != null)
                    {
                        this.ServiceCategoryList.Clear();
                    }
                });
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
            await this.ExecuteBlockingUIAsync(
                async () =>
                {
                    if(selectedServiceCategory != null)
                    {
                        var all = await this._servicesService.GetServicesAsync();

                        var selected = from item in all
                                       where item.IdServiceCategory == selectedServiceCategory.Id
                                       select item;

                        this.ServiceList = new ObservableCollection<Service>(selected);
                    }
                    else if(this.ServiceList != null)
                    {
                        this.ServiceList.Clear();
                    }
                    
                });
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
            await this.ExecuteBlockingUIAsync(
                async () =>
                {
                    var list = await this._establishmentsService.SearchEstablishmentsAsync(this.SelectedProvince, this.SelectedService);
                    //var list = await this._establishmentsService.GetEstablishmentsAsync(new Province() { Id = 1 }, new Service() { Id = 1 });
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
                        await this.PushNavPageAsync<EstablishmentSearchResultsViewModel>((establishmentsVM) =>
                        {
                            establishmentsVM.EstablishmentsList = new ObservableCollection<Establishment>(list);
                            establishmentsVM.SelectedService = this.SelectedService;
                        });
                    }
                }); 
        }

        private bool CanSearch()
        {
            //return true;
            return this.SelectedProvince != null && this.SelectedService != null;
        }
    }
}
