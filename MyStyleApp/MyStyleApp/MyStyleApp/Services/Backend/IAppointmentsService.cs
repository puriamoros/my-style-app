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
        Task<List<Appointment>> GetClientAppointmentsAsync(DateTime from);
        Task<List<Appointment>> GetEstablishmentAppointmentsAsync(Establishment establishment, DateTime from, DateTime to);
        Task<Appointment> CreateAppointmentAsync(Appointment appointment);
        Task UpdateAppointmentStatusAsync(Appointment appointment);
        Task DeleteAppointmentAsync(Appointment appointment);
    }
}
