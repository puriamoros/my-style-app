using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStyleApp.Models
{
    public class Staff : User
    {
        [JsonProperty(Required = Required.Always, PropertyName = "idEstablishment")]
        public int IdEstablishment { get; set; }

        [JsonProperty(Required = Required.Default, PropertyName = "staffName")]
        public string StaffName { get; set; }
    }
}
