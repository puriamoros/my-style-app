using MyStyleApp.Models;
using MyStyleApp.Services;
using System;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinFormsAutofacMvvmStarterKit;

namespace MyStyleApp.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private string _message;
        private readonly INavigator _navigator;
        private ICalendarService _calendarService;

        public ICommand NextViewCommand { get; private set; }
        public ICommand AddAppointmentCommand { get; private set; }

        public LoginViewModel(INavigator navigator, ICalendarService calendarService)
        {
            this._navigator = navigator;
            this._calendarService = calendarService;
            this.Message = "First Page";
            this.NextViewCommand = new Command(async () => await this._navigator.PushAsync<RegisteredStoresViewModel>());

            Appointment appointment = new Appointment()
            {
                Title = "Título del nuevo evento",
                Description = "Descripción del nuevo evento",
                Date = DateTime.UtcNow.Add(TimeSpan.FromHours(1)),
                Duration = TimeSpan.FromHours(2)
            };
            this.AddAppointmentCommand = new Command(async () => await this._calendarService.AddAppointment(appointment));
        }

        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }
    }
}
