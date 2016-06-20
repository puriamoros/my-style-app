﻿using System;
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
    class EstablishmentsViewModel : NavigableViewModelBase
    {
        private ObservableCollection<Establishment> _establishmentList;
        private Service _selectedService;
        private IFavouritesService _favouritesService;

        public ICommand ViewDetailsCommand { get; private set; }
        public ICommand BookCommand { get; private set; }
        public ICommand AddToFavouritesCommand { get; private set; }
        public ICommand DeleteFavouriteCommand { get; private set; }

        public EstablishmentsViewModel(
            INavigator navigator, 
            IUserNotificator userNotificator, 
            LocalizedStringsService localizedStringsService,
            IFavouritesService favouritesService) : 
            base(navigator, userNotificator, localizedStringsService)
        {
            this.ViewDetailsCommand = new Command<Establishment>(this.ViewDetailsAsync);
            this.BookCommand = new Command<Establishment>(this.BookAsync);
            this.AddToFavouritesCommand = new Command<Establishment>(this.AddToFavouritesAsync);
            this.DeleteFavouriteCommand = new Command<Establishment>(this.DeleteFavouriteAsync);

            this._favouritesService = favouritesService;

            MessagingCenter.Subscribe<Establishment>(this, "favouriteAdded", this.OnFavouriteAdded);
            MessagingCenter.Subscribe<Establishment>(this, "favouriteDeleted", this.OnFavouriteDeleted);
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
                    //establishmentDetailsVM.SetData(establishment.Id, this.SelectedService.IdServiceCategory, this.SelectedService.Id);
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

        private async void AddToFavouritesAsync(Establishment establishment)
        {
            this.IsBusy = true;
            try
            {
                await this._favouritesService.AddFavouriteAsync(establishment);
                MessagingCenter.Send<Establishment>(establishment, "favouriteAdded");
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
                await this._favouritesService.DeleteFavouriteAsync(establishment);
                MessagingCenter.Send<Establishment>(establishment, "favouriteDeleted");
            }
            catch (Exception)
            {
            }
            finally
            {
                this.IsBusy = false;
            }
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
    }
}



