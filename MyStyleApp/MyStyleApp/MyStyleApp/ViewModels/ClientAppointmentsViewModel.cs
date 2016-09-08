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
    public class ClientAppointmentsViewModel : NavigableViewModelBase
    {
        private const string STORED_APPOINTMENTS_FILE_NAME = "appointments_{0}.xml";
        private IUsersService _userService;
        private IServicesService _servicesService;
        private IAppointmentsService _appointmentsService;
        private ObjectStorageService<List<StoredAppointment>> _appointmentsStorageService;
        private ICalendarService _calendarService;

        private ObservableCollection<Appointment> _appointmentList;

        public Command CancelCommand { get; private set; }
        public Command RefreshCommand { get; private set; }

        public ClientAppointmentsViewModel(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService,
            IUsersService userService,
            IServicesService servicesService,
            IAppointmentsService appointmentsService,
            ObjectStorageService<List<StoredAppointment>> appointmentsStorageService,
            ICalendarService calendarService) :
            base(navigator, userNotificator, localizedStringsService)
        {
            this._userService = userService;
            this._servicesService = servicesService;
            this._appointmentsService = appointmentsService;
            this._appointmentsStorageService = appointmentsStorageService;
            this._calendarService = calendarService;

            this.CancelCommand = new Command<Appointment>(this.CancelAsync, this.CanCancel);
            this.RefreshCommand = new Command(this.InitializeAsync);

            this.SubscribeToMessages();
        }

        private void SubscribeToMessages()
        {
            MessagingCenter.Subscribe<string>(this, "userLogin", (userType) =>
            {
                if (userType == UserTypeEnum.Client.ToString())
                {
                    MessagingCenter.Subscribe<Appointment>(this, "appointmentCreated", this.OnAppointmentCreatedAsync);
                    MessagingCenter.Subscribe<string>(this, "pushNotificationReceived", this.OnPushNotificacionReceived);
                }
            });
            MessagingCenter.Subscribe<string>(this, "userLogout", (userType) =>
            {
                if (userType == UserTypeEnum.Client.ToString())
                {
                    MessagingCenter.Unsubscribe<Appointment>(this, "appointmentCreated");
                    MessagingCenter.Unsubscribe<string>(this, "pushNotificationReceived");
                }
            });

            // Need to subscribe on ctor as well since first userLogin message is delivered before this object is created
            MessagingCenter.Subscribe<Appointment>(this, "appointmentCreated", this.OnAppointmentCreatedAsync);
            MessagingCenter.Subscribe<string>(this, "pushNotificationReceived", this.OnPushNotificacionReceived);
        }

        private void OnPushNotificacionReceived(string context)
        {
            if (context == "appointmentConfirmed" || context == "appointmentCancelled")
            {
                this.InitializeAsync();
            }
        }

        public async void InitializeAsync()
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

                    //Sort appointments by date
                    appointments.Sort((appointment1, appointment2) =>
                    {
                        return appointment1.Date.CompareTo(appointment2.Date);
                    });

                    this.AppointmentList = new ObservableCollection<Appointment>(appointments);

                    MessagingCenter.Send<string>("", "clientAppointmentsRefreshed");

                    await this.UpdateStoredAppointments();
                });
            
        }

        private string GetStoredAppointmentsFileName()
        {
            return string.Format(STORED_APPOINTMENTS_FILE_NAME, this._userService.LoggedUser.Id);
        }

        private CalendarAppointment GetCalendarAppoinment(Appointment appointment)
        {
            string title = this.LocalizedStrings.GetString(
                "calendar_appointment_title",
                "${ESTABLISHMENT_NAME}", appointment.EstablishmentName);
            string description = this.LocalizedStrings.GetString(
                "calendar_appointment_body",
                "${SERVICE_NAME}", appointment.ServiceName,
                "${SERVICE_DURATION}", appointment.ServiceDuration.ToString());
            return new CalendarAppointment()
            {
                Title = title,
                Description = description,
                Date = appointment.Date,
                Duration = TimeSpan.FromMinutes(appointment.ServiceDuration)
            };
        }

        private async Task UpdateStoredAppointments()
        {
            // Get stored appointments from file
            string fileName = this.GetStoredAppointmentsFileName();
            List<StoredAppointment> storedAppointments = null;
            try
            {
                storedAppointments = await this._appointmentsStorageService.LoadFromFileAsync(fileName);
            }
            catch(Exception)
            {
                storedAppointments = new List<StoredAppointment>();
            }

            // Construct a dictionary with the stored appointments
            Dictionary<int, StoredAppointment> dicStoredAppointments = new Dictionary<int, StoredAppointment>();
            foreach (var appointment in storedAppointments)
            {
                dicStoredAppointments.Add(appointment.Id, appointment);
            }

            // Select confirmed or cancelled appointements that are new or whose status have changed
            var updatedAppointments =
                from item in this.AppointmentList
                where (item.Status == AppointmentStatusEnum.Confirmed ||
                    item.Status == AppointmentStatusEnum.Cancelled) &&
                    (!dicStoredAppointments.ContainsKey(item.Id) ||
                    item.Status != dicStoredAppointments[item.Id].Status)
                select item;

            if(updatedAppointments.Count() > 0)
            {
                bool updateCalendar = await this.UserNotificator.DisplayAlert(
                    this.LocalizedStrings.GetString("update_calendar_title"),
                    this.LocalizedStrings.GetString("update_calendar_body"),
                    this.LocalizedStrings.GetString("yes"),
                    this.LocalizedStrings.GetString("no"));

                // Update calendar adding or deleting appointments and
                // create a list of stored appointments with the selected appointments
                List<StoredAppointment> updatedStoredAppointments = new List<StoredAppointment>();
                foreach (var appointment in updatedAppointments)
                {
                    StoredAppointment stored = new StoredAppointment(appointment);
                    if(updateCalendar)
                    {
                        if (stored.Status == AppointmentStatusEnum.Confirmed)
                        {
                            // Add appointment to the calendar
                            CalendarAppointment calendarAppointment = this.GetCalendarAppoinment(appointment);
                            stored.PlatformCalendarId =
                                await this._calendarService.AddAppointmentAsync(calendarAppointment);
                        }
                        else if (stored.Status == AppointmentStatusEnum.Cancelled && dicStoredAppointments.ContainsKey(appointment.Id))
                        {
                            // Remove appointment from the calendar
                            string calendarPlatformId = dicStoredAppointments[appointment.Id].PlatformCalendarId;
                            if (calendarPlatformId != null)
                            {
                                await this._calendarService.DeleteAppointmentAsync(calendarPlatformId);
                            }
                        }
                    }
                    updatedStoredAppointments.Add(stored);
                }

                // Update stored appointments dictionary
                foreach (var stored in updatedStoredAppointments)
                {
                    if (dicStoredAppointments.ContainsKey(stored.Id))
                    {
                        dicStoredAppointments[stored.Id] = stored;
                    }
                    else
                    {
                        dicStoredAppointments.Add(stored.Id, stored);
                    }
                }

                // Save updated stored appointments to file
                await this._appointmentsStorageService.SaveToFileAsync(
                    fileName,
                    new List<StoredAppointment>(dicStoredAppointments.Values.ToArray()));
            }
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

        private async void OnAppointmentCreatedAsync(Appointment appointment)
        {
            this.AppointmentList.Add(appointment);
            this.RefreshAppointmentsList();
            await this.UpdateStoredAppointments();
        }
    }
}
