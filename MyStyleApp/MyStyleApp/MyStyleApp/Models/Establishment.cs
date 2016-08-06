using MyStyleApp.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MyStyleApp.Models
{
    public class Establishment
    {
        [JsonProperty(Required = Required.Always, PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(Required = Required.Always, PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(Required = Required.Always, PropertyName = "address")]
        public string Address { get; set; }

        [JsonProperty(Required = Required.Always, PropertyName = "phone")]
        public string Phone { get; set; }

        [JsonProperty(Required = Required.Always, PropertyName = "idEstablishmentType")]
        public int IdEstablishmentType { get; set; }

        [JsonProperty(Required = Required.Default, PropertyName = "idFavourite")]
        public int IdFavourite { get; set; }

        [JsonProperty(Required = Required.Always, PropertyName = "idOwner")]
        public int IdOwner { get; set; }

        [JsonProperty(Required = Required.Always, PropertyName = "idProvince")]
        public int IdProvince { get; set; }

        [JsonProperty(Required = Required.Default, PropertyName = "concurrence")]
        public int Concurrence { get; set; }

        [JsonProperty(Required = Required.Always, PropertyName = "hours1")]
        public string Hours1 { get; set; }

        [JsonProperty(Required = Required.Always, PropertyName = "hours2")]
        public string Hours2 { get; set; }

        [JsonProperty(Required = Required.Always, PropertyName = "confirmType")]
        public ConfirmTypeEnum ConfirmType { get; set; }

        [JsonProperty(Required = Required.Always, PropertyName = "latitude")]
        public double Latitude { get; set; }

        [JsonProperty(Required = Required.Always, PropertyName = "longitude")]
        public double Longitude { get; set; }

        [JsonProperty(Required = Required.Default, PropertyName = "services")]
        public List<ShortenService> ShortenServices { get; set; }

        [JsonIgnore]
        public string ProvinceName { get; set; }

        // Need this to solve an extrange behaviour in BindablePicker
        public override bool Equals(object obj)
        {
            if(obj is Establishment)
            {
                return ((Establishment)obj).Id == this.Id;
            }
            return false;
        }
    }
}