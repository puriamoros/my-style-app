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
        private DateTime _date;
        private DateTime _minimumDate;
        private DateTime _maximumDate;

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

        public void Initialize()
        {
            this.Date = DateTime.Today;
            this.MinimumDate = DateTime.Today;
            this.MaximumDate = DateTime.Today.AddMonths(3);
        }

        public DateTime Date
        {
            get { return _date; }
            set { SetProperty(ref _date, value); }
        }

        public DateTime MinimumDate
        {
            get { return _minimumDate; }
            set { SetProperty(ref _minimumDate, value); }
        }

        public DateTime MaximumDate
        {
            get { return _maximumDate; }
            set { SetProperty(ref _maximumDate, value); }
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
