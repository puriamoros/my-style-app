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

namespace MyStyleApp.ViewModels
{
    class EstablishmentsViewModel : NavigableViewModelBase
    {
        private ObservableCollection<Establishment> _establishmentList;
        private Service _selectedService;
        private IFavouritesService _favouritesService;
        private IUsersService _usersService;

        public ICommand ViewDetailsCommand { get; private set; }
        public ICommand BookCommand { get; private set; }
        public ICommand AddToFavouritesCommand { get; private set; }

        public EstablishmentsViewModel(
            INavigator navigator, 
            IUserNotificator userNotificator, 
            LocalizedStringsService localizedStringsService,
            IFavouritesService favouritesService,
            IUsersService usersService) : 
            base(navigator, userNotificator, localizedStringsService)
        {
            this.ViewDetailsCommand = new Command<Establishment>(this.ViewDetailsAsync);
            this.BookCommand = new Command<Establishment>(this.BookAsync);
            this.AddToFavouritesCommand = new Command<Establishment>(this.AddToFavouritesAsync);

            this._favouritesService = favouritesService;
            this._usersService = usersService;
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

        private async void BookAsync(Establishment establishment)
        {
            await this.UserNotificator.DisplayAlert("book", "", "ok");
            
            //this.IsBusy = true;
            //try
            //{
            //    await this.PushAsync<BookViewModel>((bookVM) =>
            //    {
            //        bookVM.Establishment = establishment;
            //    }
            //    );
            //}
            //catch (Exception)
            //{
            //}
            //finally
            //{
            //    this.IsBusy = false;
            //}
        }

        private async void AddToFavouritesAsync(Establishment establishment)
        {
            this.IsBusy = true;
            try
            {
                Favourite fav = new Favourite()
                {
                    Id = establishment.IdFavourite,
                    IdClient = this._usersService.LoggedUser.Id,
                    IdEstablishment = establishment.Id
                };
                await this._favouritesService.AddFavouriteAsync(fav);

                MessagingCenter.Send<EstablishmentsViewModel>(this, "refresh");
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



