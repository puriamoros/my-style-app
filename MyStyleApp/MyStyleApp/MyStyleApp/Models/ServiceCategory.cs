using MyStyleApp.Enums;
using Newtonsoft.Json;

namespace MyStyleApp.Models
{
    public class ServiceCategory
    {
        [JsonProperty(Required = Required.Always, PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(Required = Required.Always, PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(Required = Required.Always, PropertyName = "idEstablishmentType")]
        public EstablishmentTypeEnum EstablishmentType { get; set; }
    }
}
