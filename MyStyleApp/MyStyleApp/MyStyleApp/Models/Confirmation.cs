using Newtonsoft.Json;
using System;

namespace MyStyleApp.Models
{
    public class Confirmation
    {
        [JsonProperty(Required = Required.Always, PropertyName = "confirmed")]
        public bool Confirmed { get; set; }
    }
}

