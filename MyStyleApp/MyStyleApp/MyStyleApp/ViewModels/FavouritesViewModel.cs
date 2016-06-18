using System;
using System.Threading.Tasks;
using MyStyleApp.Services;
using MvvmCore;
using System.Collections.ObjectModel;
using MyStyleApp.Models;
using System.Windows.Input;
using Xamarin.Forms;
using MyStyleApp.Services.Backend;

namespace MyStyleApp.ViewModels
{
    public class FavouritesViewModel : NavigableViewModelBase
    {
        private ObservableCollection<Establishment> _favouritesList;
        private IFavouritesService _favouritesService;
        private IUsersService _usersService;

        public ICommand ViewDetailsCommand { get; private set; }
        public ICommand DeleteFavouriteCommand { get; private set; }

        public FavouritesViewModel(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService,
            IFavouritesService favouritesService,
            IUsersService usersService) :
            base(navigator, userNotificator, localizedStringsService)
        {
            this.ViewDetailsCommand = new Command<Establishment>(this.ViewDetailsAsync);
            this.DeleteFavouriteCommand = new Command<Establishment>(this.DeleteFavouriteAsync);
            this._favouritesService = favouritesService;
            this._usersService = usersService;

            MessagingCenter.Subscribe<EstablishmentsViewModel>(this, "refresh", (establishmentsVM) => InitializeAsync());

            this.InitializeAsync();
        }

        private async void InitializeAsync()
        {
            this.IsBusy = true;
            try
            {
                this.FavouritesList = new ObservableCollection<Establishment>(
                    await this._favouritesService.GetFavouritesAsync(this._usersService.LoggedUser.Id));
            }
            catch (Exception ex)
            {
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        public ObservableCollection<Establishment> FavouritesList
        {
            get { return _favouritesList; }
            set { SetProperty(ref _favouritesList, value); }
        }

        private async void ViewDetailsAsync(Establishment establishment)
        {
            this.IsBusy = true;
            try
            {
                await this.PushNavPageAsync<EstablishmentDetailsViewModel>((establishmentDetailsVM) =>
                {
                    establishmentDetailsVM.Establishment = establishment;
                }
                );
            }
            catch (Exception)
            {
            }
            finally
            {
                this.IsBusy = false;
            }   
        }

        private async void DeleteFavouriteAsync(Establishment establishment)
        {
            this.IsBusy = true;
            try
            {
                Favourite fav = new Favourite()
                {
                    Id = establishment.IdFavourite,
                    IdClient = this._usersService.LoggedUser.Id,
                    IdEstablishment =  establishment.Id
                };
                await this._favouritesService.DeleteFavouriteAsync(fav);

                this.FavouritesList = new ObservableCollection<Establishment>(
                    await this._favouritesService.GetFavouritesAsync(this._usersService.LoggedUser.Id));
            }
            catch (Exception)
            {
            }
            finally
            {
                this.IsBusy = false;
            }
        }
    }
}
