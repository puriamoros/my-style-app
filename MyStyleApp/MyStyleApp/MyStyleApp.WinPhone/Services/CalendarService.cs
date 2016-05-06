﻿using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Appointments;
using Windows.Foundation;
using Xamarin.Forms;

[assembly: Dependency(typeof(MyStyleApp.WinPhone.Services.CalendarService))]
namespace MyStyleApp.WinPhone.Services
{
    class CalendarService : MyStyleApp.Services.ICalendarService
    {
        public async Task<bool> AddAppointment(Models.Appointment appointment)
        {
            var appointmentRcd = new Appointment();

            // StartTime
            var date = appointment.Date;
            var time = appointment.Date.TimeOfDay;
            var timeZoneOffset = TimeZoneInfo.Local.GetUtcOffset(DateTime.Now);
            var startTime = new DateTimeOffset(date.Year, date.Month, date.Day, time.Hours, time.Minutes, time.Seconds, timeZoneOffset);
            appointmentRcd.StartTime = startTime;

            // Subject
            appointmentRcd.Subject =  appointment.Title;
            // Location
            appointmentRcd.Location = "";
            // Details
            appointmentRcd.Details = appointment.Description;
            // Duration          
            appointmentRcd.Duration = appointment.Duration;
            // All Day
            appointmentRcd.AllDay = false;
            //Busy Status
            appointmentRcd.BusyStatus = AppointmentBusyStatus.Busy;
            // Sensitivity
            appointmentRcd.Sensitivity = AppointmentSensitivity.Public;

            // var rect = GetElementRect(sender as FrameworkElement);
            Rect rect = new Rect(new Windows.Foundation.Point(10, 10), new Windows.Foundation.Size(100, 200));

            string retVal = await AppointmentManager.ShowAddAppointmentAsync(
                appointmentRcd, rect, Windows.UI.Popups.Placement.Default);

            return !string.IsNullOrEmpty(retVal);
        }
    }
}