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
   
    public class EstablishmentAppointmentsViewModel : NavigableViewModelBase
    {
        private DateTime _date;
        private DateTime _minimumDate;
        private DateTime _maximumDate;

        private IUsersService _userService;
        private IServicesService _servicesService;
        private IAppointmentsService _appointmentsService;
        private IEstablishmentsService _establishmentsService;

        private ObservableCollection<Appointment> _appointmentList;
        private ObservableCollection<Establishment> _establishmentList;
        private Establishment _selectedEstablishment;

        public Command CancelCommand { get; private set; }
        public Command ConfirmCommand { get; private set; }

        public EstablishmentAppointmentsViewModel(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService,
            IUsersService userService,
            IServicesService servicesService,
            IAppointmentsService appointmentsService,
            IEstablishmentsService establishmentsService) :
            base(navigator, userNotificator, localizedStringsService)
        {
            this._userService = userService;
            this._servicesService = servicesService;
            this._appointmentsService = appointmentsService;
            this._establishmentsService = establishmentsService;

            this.CancelCommand = new Command<Appointment>(this.CancelAsync, this.CanCancel);
            this.ConfirmCommand = new Command<Appointment>(this.ConfirmAsync, this.CanConfirm);

            this.InitializeAsync();

            this.SubscribeToMessages();
        }

        private void SubscribeToMessages()
        {
            MessagingCenter.Subscribe<string>(this, "userLogin", (userType) =>
            {
                if (userType != UserTypeEnum.Client.ToString())
                {
                    MessagingCenter.Subscribe<string>(this, "pushNotificationReceived", this.OnPushNotificacionReceived);
                }
            });
            MessagingCenter.Subscribe<string>(this, "userLogout", (userType) =>
            {
                if (userType != UserTypeEnum.Client.ToString())
                {
                    MessagingCenter.Unsubscribe<string>(this, "pushNotificationReceived");
                }
            });

            // Need to subscribe on ctor as well since first userLogin message is delivered before this object is created
            MessagingCenter.Subscribe<string>(this, "pushNotificationReceived", this.OnPushNotificacionReceived);
        }

        private void OnPushNotificacionReceived(string context)
        {
            if (this.SelectedEstablishment != null &&
                (context.StartsWith("appointmentCancelled") || context.StartsWith("appointmentCreated")))
            {
                string[] split = context.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                if(split.Length == 3)
                {
                    try
                    {
                        int idEstablishment = int.Parse(split[1]);
                        DateTime date = DateTime.Parse(split[2]);

                        if(idEstablishment == this.SelectedEstablishment.Id &&
                            date.Year == this.Date.Year &&
                            date.Month == this.Date.Month &&
                            date.Day == this.Date.Day)
                        {
                            this.OnDataChanged();
                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }
        }

        public async void InitializeAsync()
        {
            this.MinimumDate = DateTime.Today;
            this.MaximumDate = DateTime.Today.AddMonths(3);
            this.Date = DateTime.Today;

            await this.ExecuteBlockingUIAsync(
                async () =>
                {
                    var establishments = await this._establishmentsService.GetMyEstablishmentsAsync();

                    this.EstablishmentList = new ObservableCollection<Establishment>(establishments);
                });
        }

        public DateTime Date
        {
            get { return _date; }
            set
            {
                SetProperty(ref _date, value);
                this.OnDataChanged();
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

        public ObservableCollection<Appointment> AppointmentList
        {
            get { return _appointmentList; }
            set { SetProperty(ref _appointmentList, value); }
        }

        public ObservableCollection<Establishment> EstablishmentList
        {
            get { return _establishmentList; }
            set { SetProperty(ref _establishmentList, value); }
        }

        public Establishment SelectedEstablishment
        {
            get { return _selectedEstablishment; }
            set
            {
                SetProperty(ref _selectedEstablishment, value);
                this.OnDataChanged();
            }
        }

        private async void OnDataChanged()
        {
            await this.ExecuteBlockingUIAsync(
                async () =>
                {
                    if (this.SelectedEstablishment != null)
                    {
                        var appointments = await this._appointmentsService.GetEstablishmentAppointmentsAsync(this.SelectedEstablishment, this.Date, this.Date.AddDays(1).AddMilliseconds(-1));
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
                    }
                    else
                    {
                        if(this.AppointmentList != null)
                        {
                            this.AppointmentList.Clear();
                        }                      
                    }
                });
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
                        this.CancelCommand.ChangeCanExecute();
                        this.ConfirmCommand.ChangeCanExecute();
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

        private async void ConfirmAsync(Appointment appointment)
        {
            bool doConfirm = await this.UserNotificator.DisplayAlert(
                this.LocalizedStrings.GetString("appointment_confirm_title"),
                this.LocalizedStrings.GetString("appointment_confirm_body"),
                this.LocalizedStrings.GetString("yes"),
                this.LocalizedStrings.GetString("no"));

            if (doConfirm)
            {
                await this.ExecuteBlockingUIAsync(
                async () =>
                {
                    try
                    {
                        await this._appointmentsService.UpdateAppointmentStatusAsync(appointment, AppointmentStatusEnum.Confirmed);
                        appointment.Status = AppointmentStatusEnum.Confirmed;
                        this.RefreshAppointmentsList();
                        this.CancelCommand.ChangeCanExecute();
                        this.ConfirmCommand.ChangeCanExecute();
                    }
                    catch (BackendException ex)
                    {
                        if (ex.BackendError.State == (int)BackendStatusCodeEnum.StateAppointmentConfirmationError)
                        {
                            await this.UserNotificator.DisplayAlert(
                                this.LocalizedStrings.GetString("error"),
                                this.LocalizedStrings.GetString("appointment_confirmation_error"),
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

        private bool CanConfirm(Appointment appointment)
        {
            if (appointment != null)
            {
                return appointment.Status == AppointmentStatusEnum.Pending;
            }
            return false;
        }

        private void RefreshAppointmentsList()
        {
            var list = new List<Appointment>(this.AppointmentList);
            list.Sort((one, other) => { return one.Date.CompareTo(other.Date); });
            this.AppointmentList = new ObservableCollection<Appointment>(list);
        }
    }
}
