using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStyleApp.Models
{
    public class Service
    {
        [JsonProperty(Required = Required.Always, PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(Required = Required.Always, PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(Required = Required.Always, PropertyName = "price")]
        public float Price { get; set; }

        [JsonIgnore]
        public string NameAndPrice
        {
            get
            {
                return Name + " " + Price + " €";
            }
        }
    }
}
