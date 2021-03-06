﻿using MyStyleApp.Enums;
using Newtonsoft.Json;

namespace MyStyleApp.Models
{
    public class Service
    {
        [JsonProperty(Required = Required.Always, PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(Required = Required.Always, PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(Required = Required.Always, PropertyName = "idServiceCategory")]
        public int IdServiceCategory { get; set; }

        [JsonProperty(Required = Required.Always, PropertyName = "duration")]
        public int Duration { get; set; }

        [JsonProperty(Required = Required.Default, PropertyName = "price")]
        public float Price { get; set; }
    }
}
