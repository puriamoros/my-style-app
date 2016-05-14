using Newtonsoft.Json;

namespace MyStyleApp.Models
{
    public class ApiKey
    {
        [JsonProperty(Required = Required.Always, PropertyName = "apiKey")]
        public string Value { get; set; }
    }
}
