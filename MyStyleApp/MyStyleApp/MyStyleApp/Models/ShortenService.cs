using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStyleApp.Models
{
    public class ShortenService
    {
        [JsonProperty(Required = Required.Always, PropertyName = "idService")]
        public int Id { get; set; }

        [JsonProperty(Required = Required.Always, PropertyName = "price")]
        public float Price { get; set; }
    }
}
