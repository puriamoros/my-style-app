using MyStyleApp.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MyStyleApp.Models
{
    public class ShortenServiceListContainer
    {
        [JsonProperty(Required = Required.Default, PropertyName = "services")]
        public List<ShortenService> ShortenServices { get; set; }
    }
}
