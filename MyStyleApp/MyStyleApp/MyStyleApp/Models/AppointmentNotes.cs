using Newtonsoft.Json;

namespace MyStyleApp.Models
{
    public class AppointmentNotes
    {
        [JsonProperty(Required = Required.Always, PropertyName = "notes")]
        public string Notes { get; set; }
    }
}
