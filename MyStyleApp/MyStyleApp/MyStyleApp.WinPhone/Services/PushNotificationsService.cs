using System;
using Windows.Networking.PushNotifications;
using Xamarin.Forms;

[assembly: Dependency(typeof(MyStyleApp.WinPhone.Services.PushNotificationsService))]
namespace MyStyleApp.WinPhone.Services
{
    public class PushNotificationsService : MyStyleApp.Services.IPushNotificationsService
    {

        public void RegisterDevice()
        {
            string uri = "";
            try
            {
                var channelTask = PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync().AsTask();
                if(channelTask.Wait(10000))
                {
                    var channel = channelTask.Result;
                    uri = channel.Uri;
                    channel.PushNotificationReceived += Channel_PushNotificationReceived;
                    System.Diagnostics.Debug.WriteLine("Channel uri received");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Error receiving channel uri: timeout");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error receiving channel uri: "+ ex.ToString());
            }

            Xamarin.Forms.MessagingCenter.Send<string>(uri, "pushNotificationTokenReceived");
        }

        private void Channel_PushNotificationReceived(PushNotificationChannel sender, PushNotificationReceivedEventArgs args)
        {
            switch (args.NotificationType)
            {
                case PushNotificationType.Toast:
                    string context = "";
                    var nodeList = args.ToastNotification.Content.GetElementsByTagName("context");
                    if (nodeList.Count > 0)
                    {
                        context = nodeList.Item(0).InnerText;
                    }
                    Xamarin.Forms.MessagingCenter.Send<string>(context, "pushNotificationReceived");
                    break;
                case PushNotificationType.Tile:
                    break;
                case PushNotificationType.TileFlyout:
                    break;
                case PushNotificationType.Badge:
                    break;
                case PushNotificationType.Raw:
                    break;
            }

            
        }
    }
}
