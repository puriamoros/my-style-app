using MvvmCore;
using System;
using MyStyleApp.Models;
using System.Collections.ObjectModel;
using MyStyleApp.Services;
using Xamarin.Forms;
using System.Windows.Input;
using MyStyleApp.Services.Backend;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

namespace MyStyleApp.ViewModels
{
    class EstablishmentDetailsViewModel : NavigableViewModelBase
    {
        private IFavouritesService _favouritesService;
        private IServiceCategoriesService _serviceCategoriesService;
        private IServicesService _servicesService;
        private IEstablishmentsService _establishmentsService;

        private Establishment _establishment;
        
        private ObservableCollection<ServiceNameAndPrice> _serviceList;
        private ServiceNameAndPrice _selectedService;
        
        private ObservableCollection<ServiceCategory> _serviceCategoryList;
        private ServiceCategory _selectedServiceCategory;

        private List<ServiceNameAndPrice> _establishmentServicesList;

        private bool _initializing;

        private object lockObj;
        
        public ICommand BookCommand { get; private set; }
        public ICommand AddToFavouritesCommand { get; private set; }
        public ICommand DeleteFavouriteCommand { get; private set; }

        public EstablishmentDetailsViewModel(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService,
            IFavouritesService favouritesService,
            IServiceCategoriesService serviceCategoriesService,
            IServicesService servicesService,
            IEstablishmentsService establishmentsService) :
            base(navigator, userNotificator, localizedStringsService)
        {
            this.lockObj = new object();

            this.BookCommand = new Command(this.BookAsync);
            this.AddToFavouritesCommand = new Command(this.AddToFavouritesAsync);
            this.DeleteFavouriteCommand = new Command(this.DeleteFavouriteAsync);

            this._favouritesService = favouritesService;
            this._serviceCategoriesService = serviceCategoriesService;
            this._servicesService = servicesService;
            this._establishmentsService = establishmentsService;

            MessagingCenter.Subscribe<Establishment>(this, "favouriteAdded", this.OnFavouriteAdded);
            MessagingCenter.Subscribe<Establishment>(this, "favouriteDeleted", this.OnFavouriteDeleted);
        }

        public async Task InitilizeAsync(int idEstablishment, int idServiceCategory, int idService)
        {
            lock(lockObj)
            {
                this._initializing = true;
            }

            this.Establishment = null;
            if(this.ServiceCategoryList != null)
            {
                this.ServiceCategoryList.Clear();
            }
            this.SelectedServiceCategory = null;
            if (this.ServiceList != null)
            {
                this.ServiceList.Clear();
            }
            this.SelectedService = null;

            await this.ExecuteBlockingUIAsync(
                async () =>
                {
                    // Get selected establishment from BE
                    this.Establishment = await this._establishmentsService.GetEstablishmentAsync(idEstablishment);

                    // Get all the services and services categories from BE
                    var allServicesList = await this._servicesService.GetServicesAsync();
                    var allServiceCategoriesList = await this._serviceCategoriesService.GetServiceCategoriesAsync();

                    // Get a list with all the services offered by the establishment
                    this._establishmentServicesList = new List<ServiceNameAndPrice>();
                    foreach (ShortenService item in this.Establishment.ShortenServices)
                    {
                        var result = from service in allServicesList
                                     where service.Id == item.Id
                                     select service;

                        if(result.Count() > 0)
                        {
                            ServiceNameAndPrice snp = new ServiceNameAndPrice();
                            snp.Id = result.ElementAt(0).Id;
                            snp.Name = result.ElementAt(0).Name;
                            snp.IdServiceCategory = result.ElementAt(0).IdServiceCategory;
                            snp.Price = item.Price;
                             
                            this._establishmentServicesList.Add(snp);
                        }
                    }

                    // Get a list with all the service categories of the services offered by the establishment
                    List<ServiceCategory> establishmentServiceCategoriesList = new List<ServiceCategory>();
                    foreach (ServiceNameAndPrice item in this._establishmentServicesList)
                    {
                        var result = from serviceCategory in allServiceCategoriesList
                                     where serviceCategory.Id == item.IdServiceCategory
                                     select serviceCategory;

                        if (result.Count() > 0)
                        {
                            var sc = establishmentServiceCategoriesList.Find((serviceCategory) =>
                            {
                                return serviceCategory.Id == result.ElementAt(0).Id;
                            });

                            if (sc == null)
                            {
                                establishmentServiceCategoriesList.Add(result.ElementAt(0));
                            }
                        }
                    }

                    this.ServiceCategoryList = new ObservableCollection<ServiceCategory>(establishmentServiceCategoriesList);

                    var result2 = from item in this.ServiceCategoryList
                                 where item.Id == idServiceCategory
                                 select item;

                    if (result2.Count() > 0)
                    {
                        this.SelectedServiceCategory = result2.ElementAt(0);
                        this.OnSelectedServiceCategoryChanged(this.SelectedServiceCategory);

                        var result3 = from item in this.ServiceList
                                      where item.Id == idService
                                      select item;

                        if (result3.Count() > 0)
                        {
                            this.SelectedService = result3.ElementAt(0);
                        }
                    }
                });

            lock (lockObj)
            {
                this._initializing = false;
            }
        }

        

        public Establishment Establishment
        {
            get { return _establishment; }
            set { SetProperty(ref _establishment, value); }
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

                lock (lockObj)
                {
                    if (!this._initializing)
                    {
                        this.OnSelectedServiceCategoryChanged(value);
                    }
                }
            }
        }

        private void OnSelectedServiceCategoryChanged(ServiceCategory selectedServiceCategory)
        {
            if(selectedServiceCategory != null)
            {
                var all = this._establishmentServicesList;
                var selected = from item in all
                               where item.IdServiceCategory == selectedServiceCategory.Id
                               select item;

                this.ServiceList = new ObservableCollection<ServiceNameAndPrice>(selected);
            }
            else
            {
                this.ServiceList.Clear();
            }
        }

        public ObservableCollection<ServiceNameAndPrice> ServiceList
        {
            get { return _serviceList; }
            set { SetProperty(ref _serviceList, value); }
        }

        public ServiceNameAndPrice SelectedService
        {
            get { return _selectedService; }
            set
            {
                SetProperty(ref _selectedService, value);
                //this.SearchCommand.ChangeCanExecute();
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

        private async void BookAsync()
        {
            await this.ExecuteBlockingUIAsync(
                async () =>
                {
                    await this.PushNavPageAsync<BookViewModel>((bookVM) =>
                    {
                        bookVM.Initialize();
                    });
                });     
        }
        
        private async void AddToFavouritesAsync()
        {
            await this.ExecuteBlockingUIAsync(
                async () =>
                {
                    await this._favouritesService.AddFavouriteAsync(this.Establishment);
                    MessagingCenter.Send<Establishment>(this.Establishment, "favouriteAdded");
                });
        }

        private async void DeleteFavouriteAsync()
        {
            await this.ExecuteBlockingUIAsync(
                async () =>
                {
                    await this._favouritesService.DeleteFavouriteAsync(this.Establishment);
                    MessagingCenter.Send<Establishment>(this.Establishment, "favouriteDeleted");
                });
        }

        private void RefreshEstablishment()
        {
            this.OnPropertyChanged(nameof(this.Establishment));
        }

        private void OnFavouriteAdded(Establishment favourite)
        {
            if (this.Establishment.Id == favourite.Id)
            {
                this.Establishment.IdFavourite = favourite.IdFavourite;
                this.RefreshEstablishment();
            }
        }

        private void OnFavouriteDeleted(Establishment favourite)
        {
            if (this.Establishment.Id == favourite.Id)
            {
                this.Establishment.IdFavourite = 0;
                this.RefreshEstablishment();
            }
        }
    }
}


