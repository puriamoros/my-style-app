using System.Threading.Tasks;
using MyStyleApp.Services;
using MvvmCore;
using System.Linq;
using MyStyleApp.Services.Backend;
using MyStyleApp.Models;
using System.Collections.ObjectModel;
using System;
using Xamarin.Forms;
using MyStyleApp.Enums;
using MyStyleApp.Exceptions;
using System.Collections.Generic;

namespace MyStyleApp.ViewModels
{
    public class AppointmentsViewModel : NavigableViewModelBase
    {
        
        private IUsersService _userService;
        private IServicesService _servicesService;
        private IAppointmentsService _appointmentsService;

        private ObservableCollection<Appointment> _appointmentList;

        public Command CancelCommand { get; private set; }

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

            this.CancelCommand = new Command<Appointment>(this.CancelAsync, this.CanCancel);

            this.InitializeAsync();
        }

        public override void OnAppearing()
        {
            base.OnAppearing();

            MessagingCenter.Subscribe<Appointment>(this, "appointmentCreated", this.OnAppointmentCreated);
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();

            MessagingCenter.Unsubscribe<Appointment>(this, "appointmentCreated");
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

        private async void CancelAsync(Appointment appointment)
        {
            bool doCancel = await this.UserNotificator.DisplayAlert(
                this.LocalizedStrings.GetString("appointment_cancel_title"),
                this.LocalizedStrings.GetString("appointment_cancel_body"),
                this.LocalizedStrings.GetString("yes"),
                this.LocalizedStrings.GetString("no"));

            if(doCancel)
            {
                await this.ExecuteBlockingUIAsync(
                async () =>
                {
                    try
                    {
                        await this._appointmentsService.UpdateAppointmentStatusAsync(appointment, AppointmentStatusEnum.Cancelled);
                        appointment.Status = AppointmentStatusEnum.Cancelled;
                        this.RefreshAppointmentsList();
                        MessagingCenter.Send<Appointment>(appointment, "appointmentCancelled");
                    }
                    catch (BackendException ex)
                    {
                        if (ex.BackendError.State == (int)BackendStatusCodeEnum.StateAppointmentCancellationError)
                        {
                            await this.UserNotificator.DisplayAlert(
                                this.LocalizedStrings.GetString("error"),
                                this.LocalizedStrings.GetString("appointment_cancellation_error"),
                                this.LocalizedStrings.GetString("ok"));
                            return;
                        }
                        else
                        {
                            throw;
                        }
                    }
                });
            }
        }

        private bool CanCancel(Appointment appointment)
        {
            if(appointment != null)
            {
                return appointment.Status != AppointmentStatusEnum.Cancelled;
            }
            return false;
        }

        private void RefreshAppointmentsList()
        {
            var list = new List<Appointment>(this.AppointmentList);
            list.Sort((one, other) => { return one.Date.CompareTo(other.Date); });
            this.AppointmentList = new ObservableCollection<Appointment>(list);
        }

        private void OnAppointmentCreated(Appointment appointment)
        {
            this.AppointmentList.Add(appointment);
            this.RefreshAppointmentsList();
        }
    }
}
