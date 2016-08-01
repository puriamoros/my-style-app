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
using System.Windows.Input;

namespace MyStyleApp.ViewModels
{
    public class ClientHistoryViewModel : NavigableViewModelBase
    {
        private IAppointmentsService _appointmentsService;
        private IUsersService _usersService;
        private IServicesService _servicesService;

        private User _user;
        private Appointment _appointment;
        private ObservableCollection<Appointment> _appointmentList;

        public ICommand AppointmentDetailsCommand { get; private set; }

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
            this._servicesService = servicesService;

            this.AppointmentDetailsCommand = new Command<Appointment>(this.AppointmentDetailsAsync);
        }

        public async void Initialize(Appointment appointment)
        {
            this.Appointment = appointment;

            await this.ExecuteBlockingUIAsync(
                async () =>
                {
                    this.User = await this._usersService.GetUserAsync(appointment.IdClient);

                    var list = await this._appointmentsService.GetAllClientAppointmentsAsync(this.User, this.Appointment.IdEstablishment);

                    var services = await this._servicesService.GetServicesAsync();

                    foreach (Appointment item in list)
                    {
                        var result = from service in services
                                     where service.Id == item.IdService
                                     select service;

                        if (result.Count() > 0)
                        {
                            item.ServiceName = result.ElementAt(0).Name;
                        }
                    }

                    list.Sort((appointment1, appointment2) =>
                    {
                        return -appointment1.Date.CompareTo(appointment2.Date);
                    });

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

        private async void AppointmentDetailsAsync(Appointment appointment)
        {
            await this.PushNavPageAsync<AppointmentDetailsViewModel>((appointmentDetailsVM) =>
            {
                appointmentDetailsVM.Initialize(appointment);
            });
        }
    }
}
