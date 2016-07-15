using Android.App;
using Android.Content;
using Android.OS;
using Android.Gms.Gcm;
using Android.Util;

namespace MyStyleApp.Droid
{
    [Service(Exported = false), IntentFilter(new[] { "com.google.android.c2dm.intent.RECEIVE" })]
    public class MyGcmListenerService : GcmListenerService
    {
        public override void OnMessageReceived(string from, Bundle data)
        {
            var title = data.GetString("title");
            var body = data.GetString("body");
            Log.Debug("MyStyleApp", "From:    " + from);
            Log.Debug("MyStyleApp", "Title: " + title);
            Log.Debug("MyStyleApp", "Body: " + body);
            SendNotification(title, body);
        }

        // Converts the remote notification into a local notification and lauches it
        void SendNotification(string title, string body)
        {
            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);

            var notificationBuilder = new Notification.Builder(this)
                .SetSmallIcon(Resource.Drawable.icon)
                .SetDefaults(NotificationDefaults.All)
                .SetContentTitle(title)
                .SetContentText(body)
                .SetAutoCancel(true)
                .SetContentIntent(pendingIntent);

            var notification = notificationBuilder.Build();

            // Default sound and default vibration
            //notification.Defaults |= NotificationDefaults.Sound | NotificationDefaults.Vibrate;
            Log.Debug("MyStyleApp", "NotificationDefaults: " + notification.Defaults);

            var notificationManager = (NotificationManager)GetSystemService(Context.NotificationService);
            notificationManager.Notify(0, notification);

            Xamarin.Forms.MessagingCenter.Send<string>(title, "pushNotificationReceived");
        }
    }
}