using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyStyleApp.Services;
using MvvmCore;
using MyStyleApp.Models;
using Xamarin.Forms;

namespace MyStyleApp.ViewModels
{
    public class MapViewModel : NavigableViewModelBase
    {
        private Establishment _establishment;

        public MapViewModel(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService) :
            base(navigator, userNotificator, localizedStringsService)
        {
        }

        public void Initialize(Establishment establishment)
        {
            this.Establishment = establishment;
            MessagingCenter.Send<Establishment>(this.Establishment, "showEstablishmentOnMap");
        }

        public Establishment Establishment
        {
            get { return _establishment; }
            set { SetProperty(ref _establishment, value); }
        }
    }
}
