using MyStyleApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStyleApp.Services
{
    public interface ICalendarService
    {
        Task<string> AddAppointmentAsync(CalendarAppointment calendarAppointment);
        Task<bool> DeleteAppointmentAsync(string calendarAppointmentId);
    }
}
