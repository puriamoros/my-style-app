using Android.App;
using Android.Content;
using Android.Gms.Gcm.Iid;
using Android.Util;

namespace MyStyleApp.Droid
{
    [Service(Exported = false), IntentFilter(new[] { "com.google.android.gms.iid.InstanceID" })]
    class MyInstanceIDListenerService : InstanceIDListenerService
    {
        public override void OnTokenRefresh()
        {
            Log.Info("MyStyleApp", "OnTokenRefresh: calling RegistrationIntentService");
            var intent = new Intent(this, typeof(RegistrationIntentService));
            StartService(intent);
        }
    }
}