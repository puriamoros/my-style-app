using EventKit;
using Foundation;
using MyStyleApp.Models;
using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using UIKit;

[assembly: Dependency(typeof(MyStyleApp.iOS.Services.CalendarService))]
namespace MyStyleApp.iOS.Services
{
    public class CalendarService : MyStyleApp.Services.ICalendarService
    {
        private EKEventStore _eventStore;

        public CalendarService()
        {
            this._eventStore = new EKEventStore();
        }

        public async Task<string> AddAppointmentAsync(CalendarAppointment calendarAppointment)
        {
            var granted = await _eventStore.RequestAccessAsync(EKEntityType.Event);//, (bool granted, NSError e) =>

            if (granted.Item1)
            {
                EKEvent newEvent = EKEvent.FromStore(_eventStore);
                // set the alarm for 10 minutes from now
                //newEvent.AddAlarm(EKAlarm.FromDate((NSDate)appointment.));
                // make the event start 20 minutes from now and last 30 minutes
                newEvent.StartDate = DateTimeToNSDate(calendarAppointment.Date);
                newEvent.EndDate = DateTimeToNSDate(calendarAppointment.Date.Add(calendarAppointment.Duration));
                newEvent.Title = calendarAppointment.Title;
                newEvent.Notes = calendarAppointment.Description;
                newEvent.Calendar = _eventStore.DefaultCalendarForNewEvents;
                NSError e;
                this._eventStore.SaveEvent(newEvent, EKSpan.ThisEvent, out e);
                return newEvent.EventIdentifier;
            }
            else
            {
                //new UIAlertView("Access Denied", "User Denied Access to Calendar Data", null, "ok", null).Show();
            }
            // });

            return null;
        }
        private DateTime NSDateToDateTime(NSDate date)
        {
            // NSDate has a wider range than DateTime, so clip
            // the converted date to DateTime.Min|MaxValue.
            double secs = date.SecondsSinceReferenceDate;
            if (secs < -63113904000)
                return DateTime.MinValue;
            if (secs > 252423993599)
                return DateTime.MaxValue;
            return (DateTime)date;
        }

        private NSDate DateTimeToNSDate(DateTime date)
        {
            if (date.Kind == DateTimeKind.Unspecified)
                date = DateTime.SpecifyKind(date, DateTimeKind.Utc);// or DateTimeKind.Local, this depends on each app
            return (NSDate)date;
        }

        public async Task<bool> DeleteAppointmentAsync(string calendarAppointmentId)
        {
            var eventToRemove = this._eventStore.EventFromIdentifier(calendarAppointmentId);
            if(eventToRemove != null)
            {
                NSError e;
                this._eventStore.RemoveEvent(eventToRemove, EKSpan.ThisEvent, true, out e);
            }
            return true;
        }
    }
}
