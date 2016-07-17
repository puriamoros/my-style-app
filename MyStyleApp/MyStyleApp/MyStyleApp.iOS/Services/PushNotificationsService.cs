using Xamarin.Forms;
using System.Threading.Tasks;

[assembly: Dependency(typeof(MyStyleApp.iOS.Services.PushNotificationsService))]
namespace MyStyleApp.iOS.Services
{
    public class PushNotificationsService : MyStyleApp.Services.IPushNotificationsService
    {
        public void RegisterDevice()
        {
            // Since it is necessary to have an apple development account in order to use push
            // notifications, we won't implement them on iOS
            string token = "";
            Xamarin.Forms.MessagingCenter.Send<string>(token, "pushNotificationTokenReceived");
        }
    }
}
