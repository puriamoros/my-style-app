using MvvmCore;
using MyStyleApp.Enums;
using MyStyleApp.Exceptions;
using MyStyleApp.Models;
using MyStyleApp.Services;
using MyStyleApp.Services.Backend;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MyStyleApp.ViewModels
{
    class OpeningHour
    {
        public int Hour { get; set; }
        public int Minute { get; set; }
    }

    public class BookViewModel : NavigableViewModelBase
    {
        private const string HOURS_ERROR = "Backend data for establishment's hours are erroneous";

        private DateTime _date;
        private DateTime _minimumDate;
        private DateTime _maximumDate;
        private ObservableCollection<Slot> _slotList;

        private IUsersService _usersService;
        private IAppointmentsService _appointmentsService;
        private Establishment _establishment;
        private Service _service;
        private List<OpeningHour> _openingHours;

        public ICommand BookCommand { get; private set; }
        
        public BookViewModel(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService,
            IUsersService usersService,
            IAppointmentsService appointmentsService) :
            base(navigator, userNotificator, localizedStringsService)
        {
            this._usersService = usersService;
            this._appointmentsService = appointmentsService;

            this.BookCommand = new Command<Slot>(this.BookAsync);
        }

        public override void OnPushed()
        {
            base.OnPushed();

            MessagingCenter.Subscribe<Appointment>(this, "appointmentCancelled", this.OnAppointmentCancelled);
        }

        public override void OnPopped()
        {
            base.OnPopped();

            MessagingCenter.Unsubscribe<Appointment>(this, "appointmentCancelled");
        }

        public void Initialize(Establishment establishment, Service service)
        {
            this._establishment = establishment;
            this._service = service;
            this.ProcessEstablishmentOpeningHours();

            this.MinimumDate = DateTime.Today;
            this.MaximumDate = DateTime.Today.AddMonths(3);

            // Do it at the end of the method, since it launches OnDateChanged
            this.Date = DateTime.Today;
        }

        private void ProcessEstablishmentOpeningHours()
        {
            this._openingHours = new List<OpeningHour>();

            var intervals = new List<string>();
            if (!string.IsNullOrEmpty(this._establishment.Hours1))
            {
                intervals.Add(this._establishment.Hours1);
            }
            if (!string.IsNullOrEmpty(this._establishment.Hours2))
            {
                intervals.Add(this._establishment.Hours2);
            }

            foreach (string interval in intervals)
            {
                // Split "10:00-14:00" into "10:00" and "14:00"
                var splitInterval = interval.Split(new char[] { '-' });
                if (splitInterval.Length != 2)
                {
                    throw new Exception(HOURS_ERROR);
                }
                foreach (string hourStr in splitInterval)
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
                    if (hour < 0 || hour > 23 || (minute != 0 && minute != 30))
                    {
                        throw new Exception(HOURS_ERROR);
                    }

                    OpeningHour openingHour = new OpeningHour();
                    openingHour.Hour = hour;
                    openingHour.Minute = minute;
                    this._openingHours.Add(openingHour);
                }
            }
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

        public ObservableCollection<Slot> SlotList
        {
            get { return _slotList; }
            set { SetProperty(ref _slotList, value); }
        }

        private async void OnDateChanged(DateTime dateTime)
        {
            await RefreshAppointmentsAsync(dateTime);
        }

        private async Task RefreshAppointmentsAsync(DateTime dateTime)
        {
            var from = dateTime;
            var to = from.AddDays(1).AddMilliseconds(-1);

            List<Appointment> appointmentList = null;
            await this.ExecuteBlockingUIAsync(
                async () =>
                {
                    appointmentList = await this._appointmentsService.GetEstablishmentAppointmentsAsync(this._establishment, from, to);
                });

            if(appointmentList == null)
            {
                this.SlotList.Clear();
                return;
            }

            Dictionary<DateTime, Slot> slotsDictionary = new Dictionary<DateTime, Slot>();
            for(int i=0; i<this._openingHours.Count-1; i+=2)
            {
                DateTime start = new DateTime(
                    dateTime.Year, dateTime.Month, dateTime.Day,
                    this._openingHours[i].Hour, this._openingHours[i].Minute, 0);
                DateTime end = new DateTime(
                    dateTime.Year, dateTime.Month, dateTime.Day,
                    this._openingHours[i+1].Hour, this._openingHours[i+1].Minute, 0);

                // 00:00 in an end hour means the start of the next day
                // Ie: from 20:00 to 00:00 (00:00 is the start of the next day)
                if (end.Hour == 0 && end.Minute == 0)
                {
                    end = end.AddDays(1);
                }

                while (start < end)
                {
                    if(!slotsDictionary.ContainsKey(start))
                    {
                        // Get appointments affecting this slot and update slot.Count
                        var result = from item in appointmentList
                                     where item.Status != AppointmentStatusEnum.Cancelled &&
                                        (start >= item.Date && start < item.Date + TimeSpan.FromMinutes(item.ServiceDuration))
                                     select item;

                        Slot slot = new Slot();
                        slot.Date = start;
                        slot.Count = result.Count();
                        slot.CanBook = false;
                        slotsDictionary.Add(start, slot);
                    }

                    start = start.AddMinutes(30);
                }
            }

            // Now that we have all slots, we can update each slot.CanBook
            List<Slot> slots = new List<Slot>(slotsDictionary.Values);
            int slotsNeeded = this._service.Duration / 30;
            for (int i = 0; i < slots.Count; i++)
            {
                bool enoughTime = (i+slotsNeeded <= slots.Count);
                for (int j = 0; i + j < slots.Count && j < slotsNeeded && enoughTime; j++)
                {
                    bool enoughConcurrence = (slots[i + j].Count < this._establishment.Concurrence);
                    bool prevSlotTogether = (j == 0 || (slots[i + j].Date - slots[i + j - 1].Date) == TimeSpan.FromMinutes(30));

                    if (!enoughConcurrence || !prevSlotTogether)
                    {
                        enoughTime = false;
                    }
                }

                slots[i].CanBook = enoughTime;
            }

            slots.Sort((one, other) => { return one.Date.CompareTo(other.Date); });

            this.SlotList = new ObservableCollection<Slot>(slots);
        }

        private async void BookAsync(Slot slot)
        {
            bool doRequest = false;
            if (slot.CanBook)
            {
                doRequest = await this.UserNotificator.DisplayAlert(
                   this.LocalizedStrings.GetString("booking_data_title"),
                   this.LocalizedStrings.GetString("booking_data_body",
                    "${ESTABLISMENT_NAME}", this._establishment.Name,
                    "${SERVICE_NAME}", this._service.Name,
                    "${DATE}", slot.Date.ToString("f")),
                   this.LocalizedStrings.GetString("ok"),
                   this.LocalizedStrings.GetString("cancel"));
            }
            else
            {
                await this.UserNotificator.DisplayAlert(
                   this.LocalizedStrings.GetString("booking_error_title"),
                   this.LocalizedStrings.GetString("booking_error_body"),
                   this.LocalizedStrings.GetString("ok"));
            }

            if (doRequest)
            {
                // Appointment to create
                Appointment appointment = new Appointment()
                {
                    IdClient = this._usersService.LoggedUser.Id,
                    IdEstablishment = this._establishment.Id,
                    IdService = this._service.Id,
                    Date = slot.Date,
                    Notes = ""
                };

                bool success = false;
                await this.ExecuteBlockingUIAsync(
                    async () =>
                    {
                        try
                        {
                            // Create appointment
                            Appointment createdAppointment = await this._appointmentsService.CreateAppointmentAsync(appointment);

                            // Appointment created sucessfuly
                            success = true;

                            // Complete created appointment data
                            createdAppointment.EstablishmentName = _establishment.Name;
                            createdAppointment.ServiceName = _service.Name;
                            createdAppointment.ServicePrice = _service.Price;
                            createdAppointment.ServiceDuration = _service.Duration;

                            // Send an appointmentCreated message
                            MessagingCenter.Send<Appointment>(createdAppointment, "appointmentCreated");
                        }
                        catch (BackendException ex)
                        {
                            if(ex.BackendError.State == (int) BackendStatusCodeEnum.StateEstablishmentClosed ||
                                ex.BackendError.State == (int)BackendStatusCodeEnum.StateEstablishmentFull)
                            {
                                await this.UserNotificator.DisplayAlert(
                                   this.LocalizedStrings.GetString("booking_error_title"),
                                   this.LocalizedStrings.GetString("booking_error_body"),
                                   this.LocalizedStrings.GetString("ok"));
                            }
                            else
                            {
                                throw;
                            }
                        }
                        finally
                        {
                            // Always refresh establishment appointments on error or success
                            await this.RefreshAppointmentsAsync(this.Date);
                        }
                    });

                if(success)
                {
                    // Show result ok to user
                    await this.UserNotificator.DisplayAlert(
                       this.LocalizedStrings.GetString("booking_requested_title"),
                       this.LocalizedStrings.GetString("booking_requested_body"),
                       this.LocalizedStrings.GetString("ok"));
                }
            }
        }

        private async void OnAppointmentCancelled(Appointment appointment)
        {
            if(appointment.IdEstablishment == this._establishment.Id &&
                appointment.Date.Year == this.Date.Year &&
                appointment.Date.Month == this.Date.Month &&
                appointment.Date.Day == this.Date.Day)
            {
                await this.RefreshAppointmentsAsync(this.Date);
            }
        }
    }
}
