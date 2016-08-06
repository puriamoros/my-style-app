using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCore;
using MyStyleApp.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using MyStyleApp.Models;
using Xamarin.Forms;
using MyStyleApp.Services.Backend;
using System.Collections.Specialized;

namespace MyStyleApp.ViewModels
{
    class EstablishmentSearchResultsViewModel : NavigableViewModelBase
    {
        private ObservableCollection<Establishment> _establishmentList;
        private Service _selectedService;
        private IFavouritesService _favouritesService;
        private IEstablishmentsService _establishmentsService;

        public ICommand ViewDetailsCommand { get; private set; }
        public ICommand BookCommand { get; private set; }
        public ICommand AddToFavouritesCommand { get; private set; }
        public ICommand DeleteFavouriteCommand { get; private set; }
        public ICommand ShowMapCommand { get; private set; }

        public EstablishmentSearchResultsViewModel(
            INavigator navigator, 
            IUserNotificator userNotificator, 
            LocalizedStringsService localizedStringsService,
            IFavouritesService favouritesService,
            IEstablishmentsService establishmentsService) : 
            base(navigator, userNotificator, localizedStringsService)
        {
            this.ViewDetailsCommand = new Command<Establishment>(this.ViewDetailsAsync);
            this.BookCommand = new Command<Establishment>(this.BookAsync);
            this.AddToFavouritesCommand = new Command<Establishment>(this.AddToFavouritesAsync);
            this.DeleteFavouriteCommand = new Command<Establishment>(this.DeleteFavouriteAsync);
            this.ShowMapCommand = new Command<Establishment>(this.ShowMapAsync);

            this._favouritesService = favouritesService;
            this._establishmentsService = establishmentsService;
        }

        public override void OnPushed()
        {
            base.OnPushed();

            MessagingCenter.Subscribe<Establishment>(this, "favouriteAdded", this.OnFavouriteAdded);
            MessagingCenter.Subscribe<Establishment>(this, "favouriteDeleted", this.OnFavouriteDeleted);
        }

        public override void OnPopped()
        {
            base.OnPopped();

            MessagingCenter.Unsubscribe<Establishment>(this, "favouriteAdded");
            MessagingCenter.Unsubscribe<Establishment>(this, "favouriteDeleted");
        }

        public ObservableCollection<Establishment> EstablishmentsList
        {
            get { return _establishmentList; }
            set { SetProperty(ref _establishmentList, value); }
        }

        public Service SelectedService
        {
            get { return _selectedService; }
            set { SetProperty(ref _selectedService, value); }
        }

        private async void ViewDetailsAsync(Establishment establishment)
        {
            await this.ExecuteBlockingUIAsync(
(Func<Task>)(async () =>
                {
                    await this.PushNavPageAsync<EstablishmentDetailsViewModel>(async (establishmentDetailsVM) =>
                    {
                        // Get establishment details from BE
                        var establishmentDetails = await this._establishmentsService.GetEstablishmentAsync(establishment.Id);
                        await establishmentDetailsVM.InitilizeAsync(establishmentDetails, this.SelectedService.IdServiceCategory, this.SelectedService.Id);
                    }
                    );
                }));
        }

        private async void BookAsync(Establishment establishment)
        {
            await this.ExecuteBlockingUIAsync(
                async () =>
                {
                    await this.PushNavPageAsync<BookViewModel>((bookVM) =>
                    {
                        bookVM.Initialize(establishment, this.SelectedService);
                    });
                });
        }

        private async void AddToFavouritesAsync(Establishment establishment)
        {
            await this.ExecuteBlockingUIAsync(
                async () =>
                {
                    await this._favouritesService.AddFavouriteAsync(establishment);
                    MessagingCenter.Send<Establishment>(establishment, "favouriteAdded");
                });
        }

        private async void DeleteFavouriteAsync(Establishment establishment)
        {
            await this.ExecuteBlockingUIAsync(
                async () =>
                {
                    await this._favouritesService.DeleteFavouriteAsync(establishment);
                    MessagingCenter.Send<Establishment>(establishment, "favouriteDeleted");
                });
        }

        private void RefreshEstablishmentList()
        {
            var list = new List<Establishment>(this.EstablishmentsList);
            this.EstablishmentsList = new ObservableCollection<Establishment>(list);
        }

        private void OnFavouriteAdded(Establishment establishment)
        {
            var establishments = from item in this.EstablishmentsList
                                 where item.Id == establishment.Id
                                 select item;

            if (establishments.Count() > 0)
            {
                establishments.ElementAt(0).IdFavourite = establishment.IdFavourite;
                this.RefreshEstablishmentList();
            }
        }

        private void OnFavouriteDeleted(Establishment establishment)
        {
            var establishments = from item in this.EstablishmentsList
                                 where item.Id == establishment.Id
                                 select item;

            if (establishments.Count() > 0)
            {
                establishments.ElementAt(0).IdFavourite = 0;
                this.RefreshEstablishmentList();
            }
        }

        private async void ShowMapAsync(Establishment establishment)
        {
            if (establishment.Latitude != 0 || establishment.Longitude != 0)
            {
                await this.ExecuteBlockingUIAsync(
                async () =>
                {
                    await this.PushNavPageAsync<MapViewModel>((mapVM) =>
                    {
                        mapVM.Initialize(establishment);
                    });
                });
            }
        }
    }
}



