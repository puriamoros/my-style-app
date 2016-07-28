using System.Threading.Tasks;
using MyStyleApp.Services;
using MvvmCore;
using System;
using System.Linq;
using MyStyleApp.Enums;
using MyStyleApp.Models;
using MyStyleApp.Services.Backend;
using System.Collections.ObjectModel;
using MyStyleApp.Exceptions;
using Xamarin.Forms;

namespace MyStyleApp.ViewModels
{
    public class AppointmentDetailsViewModel : NavigableViewModelBase
    {
        private IAppointmentsService _appointmentsService;
        private Appointment _appointment;
        private string _notes;

        private Command SaveNotesCommand;

        public AppointmentDetailsViewModel(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService,
            IUsersService userService,
            IServicesService servicesService,
            IAppointmentsService appointmentsService,
            IEstablishmentsService establishmentsService) :
            base(navigator, userNotificator, localizedStringsService)
        {
            this._appointmentsService = appointmentsService;

            this.SaveNotesCommand = new Command<Appointment>(this.SaveNotesAsync);
        }

        public void Initialize(Appointment appointment)
        {
            this.Appointment = new Appointment();

            this.Appointment.Date = appointment.Date;
            this.Appointment.ClientName = appointment.ClientName;
            this.Appointment.EstablishmentName = appointment.EstablishmentName;
            this.Appointment.ServiceName = appointment.ServiceName;
            this.Appointment.ServicePrice = appointment.ServicePrice;
            this.Appointment.Notes = appointment.Notes;
        }

        public Appointment Appointment
        {
            get { return _appointment; }
            set { SetProperty(ref _appointment, value); }
        }

        public async void SaveNotesAsync(Appointment appointment)
        {
            if(this.Appointment.Notes == null)
            {
                this.Appointment.Notes = "";
            }
            await this.ExecuteBlockingUIAsync(
                    async () =>
                    {
                        await this._appointmentsService.UpdateAppointmentNotesAsync(this.Appointment);
                        
                    });
        }

    }
}
