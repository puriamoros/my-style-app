﻿using System;
using System.Linq;
using MyStyleApp.Services;
using MvvmCore;
using System.Collections.ObjectModel;
using MyStyleApp.Models;
using System.Windows.Input;
using Xamarin.Forms;
using MyStyleApp.Services.Backend;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyStyleApp.ViewModels
{
    public class FavouritesViewModel : NavigableViewModelBase
    {
        private ObservableCollection<Establishment> _favouritesList;
        private IFavouritesService _favouritesService;
        private IEstablishmentsService _establishmentsService;

        public ICommand ViewDetailsCommand { get; private set; }
        public ICommand DeleteFavouriteCommand { get; private set; }

        public FavouritesViewModel(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService,
            IFavouritesService favouritesService,
            IEstablishmentsService establishmentsService) :
            base(navigator, userNotificator, localizedStringsService)
        {
            this.ViewDetailsCommand = new Command<Establishment>(this.ViewDetailsAsync);
            this.DeleteFavouriteCommand = new Command<Establishment>(this.DeleteFavouriteAsync);
            this._favouritesService = favouritesService;
            this._establishmentsService = establishmentsService;

            MessagingCenter.Subscribe<Establishment>(this, "favouriteAdded", this.OnFavouriteAdded);
            MessagingCenter.Subscribe<Establishment>(this, "favouriteDeleted", this.OnFavouriteDeleted);
        }

        public async void InitializeAsync()
        {
            await this.ExecuteBlockingUIAsync(
                async () =>
                {
                    var list = await this._favouritesService.GetFavouritesAsync();
                    this.FavouritesList = new ObservableCollection<Establishment>(list);
                });
        }

        public ObservableCollection<Establishment> FavouritesList
        {
            get { return _favouritesList; }
            set { SetProperty(ref _favouritesList, value); }
        }

        private async void ViewDetailsAsync(Establishment establishment)
        {
            await this.ExecuteBlockingUIAsync(
                async () =>
                {
                    await this.SetMainPageTabAsync<SearchViewModel>(async (searchVM) =>
                    {
                        await searchVM.ExecuteBlockingUIAsync(
                            async () =>
                            {
                                await searchVM.PopNavPageToRootAsync();

                                // Hack to workaround a bug with Android when popping and then inmediately pushing
                                if (Device.OS == TargetPlatform.Android)
                                {
                                    await Task.Delay(10);
                                }
                                // Hack to workaround a bug with iOS when popping and then inmediately pushing
                                if (Device.OS == TargetPlatform.iOS)
                                {
                                    await Task.Delay(350);
                                }

                                await searchVM.PushNavPageAsync<EstablishmentDetailsViewModel>(async (establishmentDetailsVM) =>
                                {
                                    // Get establishment details from BE
                                    var establishmentDetails = await this._establishmentsService.GetEstablishmentAsync(establishment.Id);
                                    await establishmentDetailsVM.InitilizeAsync(establishmentDetails, 0, 0);
                                });
                            });
                    });
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

        private void RefreshFavouritesList()
        {
            var list = new List<Establishment>(this.FavouritesList);
            this.FavouritesList = new ObservableCollection<Establishment>(list);
        }

        private void OnFavouriteAdded(Establishment establishment)
        {
            var establishments = from item in this.FavouritesList
                                where item.Id == establishment.Id
                                select item;

            if(establishments.Count() <= 0)
            {
                this.FavouritesList.Add(establishment);
            }
        }

        private void OnFavouriteDeleted(Establishment establishment)
        {
            var establishments = from item in this.FavouritesList
                                 where item.Id == establishment.Id
                                 select item;

            if (establishments.Count() > 0)
            {
                this.FavouritesList.Remove(establishments.ElementAt(0));
            }
        }
    }
}
