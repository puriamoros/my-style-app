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
    public class ClientHistoryViewModel : NavigableViewModelBase
    {
        private IAppointmentsService _appointmentsService;
        private IUsersService _usersService;

        private User _user;
        private Appointment _appointment;
        private ObservableCollection<Appointment> _appointmentList;

        public ClientHistoryViewModel(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService,
            IUsersService usersService,
            IServicesService servicesService,
            IAppointmentsService appointmentsService,
            IEstablishmentsService establishmentsService) :
            base(navigator, userNotificator, localizedStringsService)
        {
            this._usersService = usersService;
            this._appointmentsService = appointmentsService;
            
        }

        public async void Initialize(Appointment appointment)
        {
            this.Appointment = appointment;

            await this.ExecuteBlockingUIAsync(
                async () =>
                {
                    this.User = await this._usersService.GetUserAsync(appointment.IdClient);

                    var list = await this._appointmentsService.GetAllClientAppointmentsAsync(this.User);
                    this.AppointmentList = new ObservableCollection<Appointment>(list); 
                });
        }

        public User User
        {
            get { return _user; }
            set { SetProperty(ref _user, value); }
        }
        
        public Appointment Appointment
        {
            get { return _appointment; }
            set { SetProperty(ref _appointment, value); }
        }

        public ObservableCollection<Appointment> AppointmentList
        {
            get { return _appointmentList; }
            set { SetProperty(ref _appointmentList, value); }
        }
    }
}
