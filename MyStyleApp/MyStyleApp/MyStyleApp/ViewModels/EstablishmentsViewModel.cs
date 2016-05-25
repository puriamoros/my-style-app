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


namespace MyStyleApp.ViewModels
{
    class EstablishmentsViewModel : NavigableViewModelBase
    {
        private ObservableCollection<Establishment> _establishmentList;

        public ICommand EstablishmentTappedCommand { get; private set; }

        public EstablishmentsViewModel(
            INavigator navigator, 
            IUserNotificator userNotificator, 
            LocalizedStringsService localizedStringsService) : 
            base(navigator, userNotificator, localizedStringsService)
        {
            this.EstablishmentTappedCommand = new Command<Establishment>(
                async (establishment) => await EstablishmentTapped(establishment));

            // REMOVE!!!
            FillWithMockData();
        }
        private void FillWithMockData()
        {
            var list = new ObservableCollection<Establishment>();
            for (int i = 0; i < 20; i++)
            {
                list.Add(new Establishment()
                {
                    Id = i,
                    Name = "Name " + i,
                    Address = "Address " + i
                }
                );
            }
            this.EstablishmentsList = list;
        }
        public ObservableCollection<Establishment> EstablishmentsList
        {
            get { return _establishmentList; }
            set { SetProperty(ref _establishmentList, value); }
        }

        private async Task EstablishmentTapped(Establishment establishment)
        {
            await this.UserNotificator.DisplayAlert(
                "Ver q se hace...", "cuerpo", "ok");
        }
    }
}


