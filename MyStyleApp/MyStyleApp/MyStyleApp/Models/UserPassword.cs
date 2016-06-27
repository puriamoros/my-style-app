using Newtonsoft.Json;

namespace MyStyleApp.Models
{
    public class UserPassword
    {
        [JsonProperty(Required = Required.Always, PropertyName = "password")]
        public string Value { get; set; }
    }
}
