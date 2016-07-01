using Newtonsoft.Json;
using System;

namespace MyStyleApp.Models
{
    public class Slot
    {
        [JsonProperty(Required = Required.Always, PropertyName = "date")]
        public DateTime Date { get; set; }

        [JsonProperty(Required = Required.Always, PropertyName = "idAppointment")]
        public int IdAppointment { get; set; }
    }
}
