using Newtonsoft.Json;

namespace MyStyleApp.Models
{
    public class BackendError
    {
        [JsonProperty(Required = Required.Always, PropertyName = "state")]
        public int State { get; set; }

        [JsonProperty(Required = Required.Always, PropertyName = "message")]
        public string Message { get; set; }
    }
}
