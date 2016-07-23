using System;
using Android.App;
using Android.Content;
using Android.Util;
using Android.Gms.Gcm;
using Android.Gms.Gcm.Iid;
using MyStyleApp.Droid.Constants;

namespace MyStyleApp.Droid
{
    [Service(Exported = false)]
    class RegistrationIntentService : IntentService
    {
        private static object locker = new object();

        public RegistrationIntentService() : base("RegistrationIntentService")
        {
        }

        protected override void OnHandleIntent(Intent intent)
        {
            try
            {
                lock (locker)
                {
                    var instanceID = InstanceID.GetInstance(Android.App.Application.Context);

                    var token = instanceID.GetToken(DroidAppConstants.SENDER_ID, GoogleCloudMessaging.InstanceIdScope, null);
                    Log.Info("MyStyleApp", "GCM registration token received");
                    
                    Xamarin.Forms.MessagingCenter.Send<string>(token, "pushNotificationTokenReceived");
                }
            }
            catch (Exception e)
            {
                Log.Debug("MyStyleApp", "Failed to get a registration token: "+e.ToString());
            }
        }
    }
}