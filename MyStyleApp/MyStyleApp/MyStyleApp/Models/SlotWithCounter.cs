using Newtonsoft.Json;
using System;

namespace MyStyleApp.Models
{
    public class SlotWithCounter
    {
        [JsonProperty(Required = Required.Always, PropertyName = "date")]
        public DateTime Date { get; set; }

        [JsonProperty(Required = Required.Always, PropertyName = "count")]
        public int Count { get; set; }
    }
}
