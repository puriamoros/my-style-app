using MvvmCore;
using MyStyleApp.Models;
using MyStyleApp.Services;
using MyStyleApp.Services.Backend;
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
        private const string HOURS_ERROR = "Backend data for establishment's hours are erroneous";
        private DateTime _date;
        private DateTime _minimumDate;
        private DateTime _maximumDate;

        private IAppointmentsService _appointmentsService;
        private Establishment _establishment;

        public ICommand BookCommand { get; private set; }
        
        public BookViewModel(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService,
            IAppointmentsService appointmentsService) :
            base(navigator, userNotificator, localizedStringsService)
        {
            this._appointmentsService = appointmentsService;

            this.BookCommand = new Command(this.BookAsync);
        }

        public void Initialize(Establishment establishment)
        {
            this.Date = DateTime.Today;
            this.MinimumDate = DateTime.Today;
            this.MaximumDate = DateTime.Today.AddMonths(3);

            this._establishment = establishment;
        }

        public DateTime Date
        {
            get { return _date; }
            set
            {
                SetProperty(ref _date, value);
                this.OnDateChanged(value);
            }
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

        private async void OnDateChanged(DateTime dateTime)
        {
            var from = dateTime;
            var to = from.AddDays(1).AddMilliseconds(-1);
            var list = await this._appointmentsService.GetEstablishmentAppointmentsAsync(this._establishment, from, to);

            var intervals = new List<string>();
            if(!string.IsNullOrEmpty(this._establishment.Hours1))
            {
                intervals.Add(this._establishment.Hours1);
            }
            if (!string.IsNullOrEmpty(this._establishment.Hours2))
            {
                intervals.Add(this._establishment.Hours2);
            }

            var slots = new List<Slot>();
            foreach(string interval in intervals)
            {
                // Split "10:00-14:00" into "10:00" and "14:00"
                var splitInterval = interval.Split(new char[] { '-' });
                if (splitInterval.Length != 2)
                {
                    throw new Exception(HOURS_ERROR);
                }
                foreach(string hourStr in splitInterval)
                {
                    // Split "10:00" in "10" and "00"
                    var splitHour = hourStr.Split(new char[] { ':' });
                    if (splitHour.Length != 2)
                    {
                        throw new Exception(HOURS_ERROR);
                    }

                    int hour = 0;
                    int minute = 0;
                    try
                    {
                        hour = int.Parse(splitHour[0]);
                        minute = int.Parse(splitHour[1]);
                    }
                    catch (Exception)
                    {
                        throw new Exception(HOURS_ERROR);
                    }
                    if(hour < 0 || hour > 23 || (minute != 0 && minute != 30))
                    {
                        throw new Exception(HOURS_ERROR);
                    }
                }
            }
        }

        private async void BookAsync()
        {
            //TODO: enviar email al propietario 
            await this.UserNotificator.DisplayAlert(
               this.LocalizedStrings.GetString("booking_requested"),
               this.LocalizedStrings.GetString("email_confirmation"), 
               this.LocalizedStrings.GetString("ok"));
        }
    }
}
