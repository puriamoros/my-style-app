using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStyleApp.Models
{
    public class Favourite
    {
        [JsonProperty(Required = Required.Always, PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(Required = Required.Always, PropertyName = "idClient")]
        public int IdClient { get; set; }

        [JsonProperty(Required = Required.Always, PropertyName = "idEstablishment")]
        public int IdEstablishment { get; set; }   
    }
}
