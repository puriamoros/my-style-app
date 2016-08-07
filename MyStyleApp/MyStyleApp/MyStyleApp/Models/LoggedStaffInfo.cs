using Newtonsoft.Json;

namespace MyStyleApp.Models
{
    public class LoggedStaffInfo
    {
        [JsonProperty(Required = Required.Always, PropertyName = "idEstablishment")]
        public int IdEstablishment { get; set; }
    }
}
