using MyStyleApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStyleApp.Services.Backend
{
    public interface IAppointmentsService
    {
        Task<List<Appointment>> GetAppointmentsAsync(DateTime from, DateTime to);
        Task<List<Appointment>> GetAppointmentsAsync(Establishment establishment, DateTime from, DateTime to);
        Task<Appointment> CreateAppointmentAsync(Appointment appointment);
        Task ConfirmAppointmentAsync(Appointment appointment);
        Task DeleteAppointmentAsync(Appointment appointment);
    }
}
