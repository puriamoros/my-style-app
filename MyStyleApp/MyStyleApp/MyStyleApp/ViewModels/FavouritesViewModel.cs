using System;
using System.Threading.Tasks;
using MyStyleApp.Services;
using MvvmCore;
using System.Collections.ObjectModel;
using MyStyleApp.Models;
using System.Windows.Input;
using Xamarin.Forms;

namespace MyStyleApp.ViewModels
{
    public class FavouritesViewModel : NavigableViewModelBase
    {
        private ObservableCollection<Favourite> _favouritesList;

        public ICommand FavouriteTappedCommand { get; private set; }

        public FavouritesViewModel(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService) :
            base(navigator, userNotificator, localizedStringsService)
        {
            this.FavouriteTappedCommand = new Command<Favourite>(this.FavouriteTappedAsync);

            // REMOVE!!!
            FillWithMockData();
        }

        private void FillWithMockData()
        {
            var list = new ObservableCollection<Favourite>();
            for (int i = 0; i < 20; i++)
            {
                list.Add(new Favourite()
                    {
                        Name = "Name " + i,
                        Description = "Description " + i
                    }
                );
            }
            this.FavouritesList = list;
        }

        public ObservableCollection<Favourite> FavouritesList
        {
            get { return _favouritesList; }
            set { SetProperty(ref _favouritesList, value); }
        }

        private async void FavouriteTappedAsync(Favourite favourite)
        {
            await this.UserNotificator.DisplayAlert(
                "Favourite tapped!", "Favourite name: " + favourite.Name , "Ok");
        }
    }
}
