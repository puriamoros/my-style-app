using MyStyleApp.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MyStyleApp.Models
{
    public class ShortenServiceListContainer
    {
        [JsonProperty(Required = Required.Default, PropertyName = "services")]
        public IList<ShortenService> ShortenServices { get; set; }
    }
}
