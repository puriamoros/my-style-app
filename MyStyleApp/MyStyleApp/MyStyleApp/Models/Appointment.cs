using Newtonsoft.Json;
using System;

namespace MyStyleApp.Models
{
    class Appointment
    {
        [JsonProperty(Required = Required.Always, PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(Required = Required.Always, PropertyName = "idClient")]
        public int IdClient { get; set; }

        [JsonProperty(Required = Required.Always, PropertyName = "idEstablishment")]
        public int IdEstablishment { get; set; }

        [JsonProperty(Required = Required.Always, PropertyName = "idService")]
        public int IdService { get; set; }

        [JsonProperty(Required = Required.Always, PropertyName = "date")]
        public DateTime Date { get; set; }

        [JsonProperty(Required = Required.Always, PropertyName = "notes")]
        public string Notes { get; set; }
    }
}
