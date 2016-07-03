using Newtonsoft.Json;
using System;

namespace MyStyleApp.Models
{
    public class GenericStatus
    {
        [JsonProperty(Required = Required.Always, PropertyName = "status")]
        public int Status { get; set; }
    }
}

