using Newtonsoft.Json;

namespace MyStyleApp.Models
{
    public class User
    {
        [JsonProperty(Required = Required.Always, PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(Required = Required.Always, PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(Required = Required.Always, PropertyName = "surname")]
        public string Surname { get; set; }

        [JsonProperty(Required = Required.Always, PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(Required = Required.Default, PropertyName = "password")]
        public string Password { get; set; }

        [JsonProperty(Required = Required.Always, PropertyName = "userType")]
        public int UserType { get; set; }

        [JsonProperty(Required = Required.Always, PropertyName = "phone")]
        public string Phone { get; set; }
    }
}
