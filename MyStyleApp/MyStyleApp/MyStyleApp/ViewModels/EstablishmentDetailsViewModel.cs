using MvvmCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyStyleApp.Models;
using System.Collections.ObjectModel;
using MyStyleApp.Services;
using Xamarin.Forms;
using System.Windows.Input;

namespace MyStyleApp.ViewModels
{
    class EstablishmentDetailsViewModel : NavigableViewModelBase
    {
        private ObservableCollection<Service> _serviceList;
        private Establishment _establishment;

        public ICommand EstablishmentDetailsTappedCommand { get; private set; }
        public ICommand BookCommand { get; private set; }
        public ICommand FavouriteCommand { get; private set; }

        public EstablishmentDetailsViewModel(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService) :
            base(navigator, userNotificator, localizedStringsService)
        {
            this.EstablishmentDetailsTappedCommand = new Command<Service>(this.EstablishmentDetailsTappedAsync);
            this.BookCommand = new Command(this.BookAsync);
            this.FavouriteCommand = new Command(this.FavouriteAsync);

            // REMOVE!!!
            FillWithMockData();
        }

        private void FillWithMockData()
        {
            var list = new ObservableCollection<Service>();
            for (int i = 0; i < 20; i++)
            {
                list.Add(new Service()
                {
                    Id = i,
                    Name = "Name " + i,
                    Price = i
                }
                );
            }
            this.ServicesList = list;
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

        private async void EstablishmentDetailsTappedAsync(Service service)
        {
            await this.UserNotificator.DisplayAlert(
                "Ir a...", "cuerpo", "ok");
        }

        private async void BookAsync()
        {
            //Change the page    
            await this.PushNavPageAsync<SearchViewModel>();
        }

        private async void FavouriteAsync()
        {
            await this.UserNotificator.DisplayAlert(
                "Añadido a favoritos...", "cuerpo", "ok");
        }

    }
}


