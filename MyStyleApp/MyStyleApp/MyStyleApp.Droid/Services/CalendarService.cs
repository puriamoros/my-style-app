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
        private DateTime _epoch;

        public CalendarService()
        {
            _epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        }

        public async Task<bool> AddAppointment(Appointment appointment)
        {
            Intent intent = new Intent(Intent.ActionInsert);

            //intent.PutExtra(CalendarContract.Events.InterfaceConsts.CalendarId, _calId);
            intent.PutExtra(CalendarContract.Events.InterfaceConsts.Title, appointment.Title);
            intent.PutExtra(CalendarContract.Events.InterfaceConsts.Description, appointment.Description);

            intent.PutExtra(CalendarContract.Events.InterfaceConsts.Dtstart, GetDateTimeMS(appointment.Date));
            intent.PutExtra(CalendarContract.Events.InterfaceConsts.Dtend, GetDateTimeMS(appointment.Date.Add(appointment.Duration)));
            intent.PutExtra(CalendarContract.ExtraEventBeginTime, GetDateTimeMS(appointment.Date));
            intent.PutExtra(CalendarContract.ExtraEventEndTime, GetDateTimeMS(appointment.Date.Add(appointment.Duration)));

            intent.PutExtra(CalendarContract.Events.InterfaceConsts.EventTimezone, UTC_TIME_ZONE);
            intent.PutExtra(CalendarContract.Events.InterfaceConsts.EventEndTimezone, UTC_TIME_ZONE);
            intent.SetData(CalendarContract.Events.ContentUri);
            ((Activity)Forms.Context).StartActivity(intent);
            return true;
        }

        public long GetDateTimeMS(DateTime dt)
        {
            return (long) dt.ToUniversalTime().Subtract(this._epoch).TotalMilliseconds;
        }
    }
}