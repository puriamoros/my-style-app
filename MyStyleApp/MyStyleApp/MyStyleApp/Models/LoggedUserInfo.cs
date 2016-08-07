using Newtonsoft.Json;

namespace MyStyleApp.Models
{
    public class LoggedUserInfo : User
    {
        [JsonProperty(Required = Required.Default, PropertyName = "staffInfo")]
        public LoggedStaffInfo StaffInfo { get; set; }
    }
}
