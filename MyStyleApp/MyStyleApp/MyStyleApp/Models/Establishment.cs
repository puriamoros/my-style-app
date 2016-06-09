﻿using Newtonsoft.Json;

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

        [JsonProperty(Required = Required.Always, PropertyName = "type")]
        public EstablishmentType Type { get; set; }
    }
}