using MvvmCore;
using System;
using MyStyleApp.Models;
using System.Collections.ObjectModel;
using MyStyleApp.Services;
using Xamarin.Forms;
using System.Windows.Input;
using MyStyleApp.Services.Backend;
using System.Collections.Generic;

namespace MyStyleApp.ViewModels
{
    class EstablishmentDetailsViewModel : NavigableViewModelBase
    {
        private IFavouritesService _favouritesService;
        private IServiceCategoriesService _serviceCategoriesService;
        private IServicesService _servicesService;

        private Establishment _establishment;

        private ObservableCollection<Service> _serviceList;
        private Service _selectedService;
        
        private ObservableCollection<ServiceCategory> _serviceCategoryList;
        private ServiceCategory _selectedServiceCategory;

        private int _idEstablishment;
        private int _idServiceCategory;
        private int _idService;
        
        public ICommand BookCommand { get; private set; }
        public ICommand AddToFavouritesCommand { get; private set; }
        public ICommand DeleteFavouriteCommand { get; private set; }

        public EstablishmentDetailsViewModel(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService,
            IFavouritesService favouritesService,
            IServiceCategoriesService serviceCategoriesService,
            IServicesService servicesService) :
            base(navigator, userNotificator, localizedStringsService)
        {
            this.BookCommand = new Command(this.BookAsync);
            this.AddToFavouritesCommand = new Command(this.AddToFavouritesAsync);
            this.DeleteFavouriteCommand = new Command(this.DeleteFavouriteAsync);

            this._favouritesService = favouritesService;
            this._serviceCategoriesService = serviceCategoriesService;
            this._servicesService = servicesService;

            MessagingCenter.Subscribe<Establishment>(this, "favouriteAdded", this.OnFavouriteAdded);
            MessagingCenter.Subscribe<Establishment>(this, "favouriteDeleted", this.OnFavouriteDeleted);
        }

        public void SetData(int idEstablishment, int idServiceCategory, int idService)
        {
            this._idEstablishment = idEstablishment;
            this._idServiceCategory = idServiceCategory;
            this._idService = idService;

            //TODO:
            // - llamar a backend establishments/{idEstablishment}
            // - asignar el resultado a selectedEstablishment
            // - guardar la lista de servicios
            // - asignar selectedCategory a idServiceCategory
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

        //public Service SelectedService
        //{
        //    get { return _selectedService; }
        //    set
        //    {
        //        SetProperty(ref _selectedService, value);
        //        this.SearchCommand.ChangeCanExecute();
        //    }
        //}

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
            this.IsBusy = true;
            try
            {
                await this.PushNavPageAsync<BookViewModel>();
            }
            catch (Exception)
            {
            }
            finally
            {
                this.IsBusy = false;
            }             
        }
        
        private async void AddToFavouritesAsync()
        {
            this.IsBusy = true;
            try
            {
                await this._favouritesService.AddFavouriteAsync(this.Establishment);
                MessagingCenter.Send<Establishment>(this.Establishment, "favouriteAdded");
            }
            catch (Exception)
            {
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        private async void DeleteFavouriteAsync()
        {
            this.IsBusy = true;
            try
            {
                await this._favouritesService.DeleteFavouriteAsync(this.Establishment);
                MessagingCenter.Send<Establishment>(this.Establishment, "favouriteDeleted");
            }
            catch (Exception)
            {
            }
            finally
            {
                this.IsBusy = false;
            }
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


