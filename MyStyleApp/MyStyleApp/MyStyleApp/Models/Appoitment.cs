using Newtonsoft.Json;
using System;

namespace MyStyleApp.Models
{
    class Appoitment
    {
        [JsonProperty(Required = Required.Always, PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(Required = Required.Always, PropertyName = "establishment")]
        public Establishment Establishment { get; set; }

        [JsonProperty(Required = Required.Always, PropertyName = "date")]
        public DateTime Date { get; set; }

        [JsonProperty(Required = Required.Always, PropertyName = "service")]
        public TimeSpan Service { get; set; }
    }
}
