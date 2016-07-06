using MyStyleApp.Enums;
using Newtonsoft.Json;
using System;

namespace MyStyleApp.Models
{
    public class Appointment
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

        [JsonProperty(Required = Required.Always, PropertyName = "status")]
        public AppointmentStatusEnum Status { get; set; }

        [JsonProperty(Required = Required.Default, PropertyName = "establishmentName")]
        public string EstablishmentName { get; set; }

        [JsonProperty(Required = Required.Default, PropertyName = "servicePrice")]
        public float ServicePrice { get; set; }

        [JsonIgnore]
        public string ServiceName { get; set; }

        //[JsonIgnore]
        //public string ServiceNameAndPrice
        //{
        //    get { return ServiceName + " - " + ServicePrice.ToString("0.00") + "€"; }
        //}
    }
}
