using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Appointments;
using Windows.Foundation;
using Xamarin.Forms;

[assembly: Dependency(typeof(MyStyleApp.WinPhone.Services.CalendarService))]
namespace MyStyleApp.WinPhone.Services
{
    class CalendarService : MyStyleApp.Services.ICalendarService
    {
        public async Task<string> AddAppointmentAsync(Models.CalendarAppointment calendarAppointment)
        {
            var appointmentRcd = new Appointment();

            // StartTime
            var date = calendarAppointment.Date;
            var timeZoneOffset = TimeZoneInfo.Local.GetUtcOffset(date);
            date.Add(timeZoneOffset);
            var startTime = new DateTimeOffset(date);
            appointmentRcd.StartTime = startTime;

            // Subject
            appointmentRcd.Subject =  calendarAppointment.Title;
            // Location
            appointmentRcd.Location = "";
            // Details
            appointmentRcd.Details = calendarAppointment.Description;
            // Duration          
            appointmentRcd.Duration = calendarAppointment.Duration;
            // All Day
            appointmentRcd.AllDay = false;
            //Busy Status
            appointmentRcd.BusyStatus = AppointmentBusyStatus.Busy;
            // Sensitivity
            appointmentRcd.Sensitivity = AppointmentSensitivity.Public;

            var tcs = new TaskCompletionSource<string>();
            Device.BeginInvokeOnMainThread(async () =>
            {
                Rect rect = new Rect(new Windows.Foundation.Point(10, 10), new Windows.Foundation.Size(100, 200));
                string result = await AppointmentManager.ShowAddAppointmentAsync(
                    appointmentRcd, rect, Windows.UI.Popups.Placement.Default);

                tcs.SetResult(result);
            });

            string appointmentId = await tcs.Task;
            return string.IsNullOrEmpty(appointmentId) ? null : appointmentId;
        }

        public async Task<bool> DeleteAppointmentAsync(string calendarAppointmentId)
        {
            var tcs = new TaskCompletionSource<bool>();
            Device.BeginInvokeOnMainThread(async () =>
            {
                Rect rect = new Rect(new Windows.Foundation.Point(10, 10), new Windows.Foundation.Size(100, 200));
                bool result = await AppointmentManager.ShowRemoveAppointmentAsync(calendarAppointmentId, rect);

                tcs.SetResult(result);
            });

            return await tcs.Task;
        }
    }
}
