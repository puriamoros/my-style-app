using MvvmCore;
using System;
using MyStyleApp.Models;
using System.Collections.ObjectModel;
using MyStyleApp.Services;
using Xamarin.Forms;
using System.Windows.Input;
using MyStyleApp.Services.Backend;

namespace MyStyleApp.ViewModels
{
    class EstablishmentDetailsViewModel : NavigableViewModelBase
    {
        private ObservableCollection<Service> _serviceList;
        private Establishment _establishment;
        private IFavouritesService _favouritesService;
        
        public ICommand BookCommand { get; private set; }
        public ICommand AddToFavouritesCommand { get; private set; }
        public ICommand DeleteFavouriteCommand { get; private set; }

        public EstablishmentDetailsViewModel(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService,
            IFavouritesService favouritesService) :
            base(navigator, userNotificator, localizedStringsService)
        {
            this.BookCommand = new Command(this.BookAsync);
            this.AddToFavouritesCommand = new Command(this.AddToFavouritesAsync);
            this.DeleteFavouriteCommand = new Command(this.DeleteFavouriteAsync);

            this._favouritesService = favouritesService;

            MessagingCenter.Subscribe<Establishment>(this, "favouriteAdded", this.OnFavouriteAdded);
            MessagingCenter.Subscribe<Establishment>(this, "favouriteDeleted", this.OnFavouriteDeleted);
        }

        public Establishment Establishment
        {
            get { return _establishment; }
            set { SetProperty(ref _establishment, value); }
        }

        public ObservableCollection<Service> ServicesList
        {
            get { return _serviceList; }
            set { SetProperty(ref _serviceList, value); }
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


