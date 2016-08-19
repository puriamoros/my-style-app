using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStyleApp.Models
{
    public class StoredAppointment : Appointment
    {
        public StoredAppointment()
        {
        }

        public StoredAppointment(Appointment appointment)
        {
            this.Id = appointment.Id;
            this.IdClient = appointment.IdClient;
            this.IdEstablishment = appointment.IdEstablishment;
            this.IdService = appointment.IdService;
            this.Date = appointment.Date;
            this.Notes = appointment.Notes;
            this.Status = appointment.Status;
            this.EstablishmentName = appointment.EstablishmentName;
            this.ServicePrice = appointment.ServicePrice;
            this.ServiceDuration = appointment.ServiceDuration;
            this.ServiceName = appointment.ServiceName;
            this.ClientName = appointment.ClientName;
        }

        public string PlatformCalendarId { get; set; }
    }
}
