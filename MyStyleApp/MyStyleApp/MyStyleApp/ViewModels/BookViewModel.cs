using MvvmCore;
using MyStyleApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MyStyleApp.ViewModels
{
    class BookViewModel : NavigableViewModelBase
    {
        public ICommand BookCommand { get; private set; }
        
        public BookViewModel(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService
            ) :
            base(navigator, userNotificator, localizedStringsService)
        {
            this.BookCommand = new Command(this.BookAsync);            
        }


        private async void BookAsync()
        {
            //await this.UserNotificator.DisplayAlert(
            //   "Reserva solicitada", "Recibirás un correo electrónico de confirmación.", "Aceptar");
            await this.UserNotificator.DisplayAlert(
               this.LocalizedStrings.GetString("booking_requested"),
               this.LocalizedStrings.GetString("email_confirmation"), 
               this.LocalizedStrings.GetString("ok"));
        }
    }
}
