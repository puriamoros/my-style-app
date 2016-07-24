using Newtonsoft.Json;

namespace MyStyleApp.Models
{
    public class UserPlatform
    {
        [JsonProperty(Required = Required.Always, PropertyName = "platform")]
        public string Platform { get; set; }

        [JsonProperty(Required = Required.Always, PropertyName = "pushToken")]
        public string PushToken { get; set; }

        [JsonProperty(Required = Required.Always, PropertyName = "languageCode")]
        public string LanguageCode { get; set; }
    }
}
