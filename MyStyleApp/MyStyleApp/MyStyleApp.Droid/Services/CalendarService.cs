using System;
using Android.App;
using Android.Content;
using Android.Provider;
using System.Threading.Tasks;
using Xamarin.Forms;
using MyStyleApp.Models;

[assembly: Dependency(typeof(MyStyleApp.Droid.Services.CalendarService))]
namespace MyStyleApp.Droid.Services
{
    public class CalendarService : MyStyleApp.Services.ICalendarService
    {
        private const string UTC_TIME_ZONE = "UTC";
        private const int CALENDAR_ID = 1;
        private DateTime _epoch;
        private ContentResolver _resolver;

        public CalendarService()
        {
            this._epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            this._resolver = ((Activity)Forms.Context).ContentResolver;
        }

        public async Task<string> AddAppointmentAsync(CalendarAppointment calendarAppointment)
        {
            ContentValues contentValues = new ContentValues();

            contentValues.Put(CalendarContract.Events.InterfaceConsts.CalendarId, CALENDAR_ID); // Default calendar
            contentValues.Put(CalendarContract.Events.InterfaceConsts.Title, calendarAppointment.Title);
            contentValues.Put(CalendarContract.Events.InterfaceConsts.Description, calendarAppointment.Description);

            contentValues.Put(CalendarContract.Events.InterfaceConsts.Dtstart, GetDateTimeMS(calendarAppointment.Date));
            contentValues.Put(CalendarContract.Events.InterfaceConsts.Dtend, GetDateTimeMS(calendarAppointment.Date.Add(calendarAppointment.Duration)));

            contentValues.Put(CalendarContract.Events.InterfaceConsts.EventTimezone, UTC_TIME_ZONE);
            contentValues.Put(CalendarContract.Events.InterfaceConsts.EventEndTimezone, UTC_TIME_ZONE);

            var eventUri = this._resolver.Insert(CalendarContract.Events.ContentUri, contentValues);
            return eventUri.LastPathSegment;
        }

        private long GetDateTimeMS(DateTime dt)
        {
            return (long) dt.ToUniversalTime().Subtract(this._epoch).TotalMilliseconds;
        }

        public async Task<bool> DeleteAppointmentAsync(string calendarAppointmentId)
        {
            long eventId = long.Parse(calendarAppointmentId);
            this._resolver.Delete(ContentUris.WithAppendedId(CalendarContract.Events.ContentUri, eventId), null, null);
            return true;
        }
    }
}