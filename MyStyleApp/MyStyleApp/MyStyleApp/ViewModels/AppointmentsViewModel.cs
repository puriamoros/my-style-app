using System.Threading.Tasks;
using MyStyleApp.Services;
using MvvmCore;
using System.Linq;
using MyStyleApp.Services.Backend;
using MyStyleApp.Models;
using System.Collections.ObjectModel;
using System;

namespace MyStyleApp.ViewModels
{
    public class AppointmentsViewModel : NavigableViewModelBase
    {
        //public ICommand NewAccountCommand { get; private set; }

        private IUsersService _userService;
        private IServicesService _servicesService;
        private IAppointmentsService _appointmentsService;

        private ObservableCollection<Appointment> _appointmentList;

        public AppointmentsViewModel(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService,
            IUsersService userService,
            IServicesService servicesService,
            IAppointmentsService appointmentsService) :
            base(navigator, userNotificator, localizedStringsService)
        {
            this._userService = userService;
            this._servicesService = servicesService;
            this._appointmentsService = appointmentsService;

            //this.NewAccountCommand = new Command(this.NewAccountAsync);

            this.InitializeAsync();
        }

        private async void InitializeAsync()
        {
            await this.ExecuteBlockingUIAsync(
                async () =>
                {
                    var appointments = await this._appointmentsService.GetClientAppointmentsAsync(DateTime.Today);
                    var services = await this._servicesService.GetServicesAsync();

                    foreach (Appointment appointment in appointments)
                    {
                        var result = from service in services
                                     where service.Id == appointment.IdService
                                     select service;

                        if (result.Count() > 0)
                        {
                            appointment.ServiceName = result.ElementAt(0).Name;
                        }
                    }

                    this.AppointmentList = new ObservableCollection<Appointment>(appointments);
                });
            
        }

        public ObservableCollection<Appointment> AppointmentList
        {
            get { return _appointmentList; }
            set { SetProperty(ref _appointmentList, value); }
        }
    }
}
