using Newtonsoft.Json;
using System.Collections.Generic;

namespace MyStyleApp.Models
{
    public class Establishment
    {
        [JsonProperty(Required = Required.Always, PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(Required = Required.Always, PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(Required = Required.Always, PropertyName = "address")]
        public string Address { get; set; }

        [JsonProperty(Required = Required.Always, PropertyName = "phone")]
        public string Phone { get; set; }

        [JsonProperty(Required = Required.Always, PropertyName = "idEstablishmentType")]
        public int IdEstablishmentType { get; set; }

        [JsonProperty(Required = Required.Default, PropertyName = "idFavourite")]
        public int IdFavourite { get; set; }

        [JsonProperty(Required = Required.Default, PropertyName = "concurrence")]
        public int Concurrence { get; set; }

        [JsonProperty(Required = Required.Always, PropertyName = "hours1")]
        public string Hours1 { get; set; }

        [JsonProperty(Required = Required.Always, PropertyName = "hours2")]
        public string Hours2 { get; set; }

        [JsonProperty(Required = Required.Default, PropertyName = "services")]
        public IList<ShortenService> ShortenServices { get; set; }
    }
}