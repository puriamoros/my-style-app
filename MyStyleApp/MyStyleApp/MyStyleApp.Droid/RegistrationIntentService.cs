using System;
using Android.App;
using Android.Content;
using Android.Util;
using Android.Gms.Gcm;
using Android.Gms.Gcm.Iid;

namespace MyStyleApp.Droid
{
    [Service(Exported = false)]
    class RegistrationIntentService : IntentService
    {
        private static object locker = new object();
        private bool tokenSent;

        public RegistrationIntentService() : base("RegistrationIntentService")
        {
            tokenSent = false;
        }

        protected override void OnHandleIntent(Intent intent)
        {
            try
            {
                Log.Info("MyStyleApp", "Calling InstanceID.GetToken");
                lock (locker)
                {
                    var instanceID = InstanceID.GetInstance(this);
#if DEBUG
                    try
                    {
                        Log.Info("MyStyleApp", "Calling: InstanceID.DeleteInstanceID");
                        instanceID.DeleteInstanceID();
                    }
                    catch (Exception e)
                    {
                        Log.Info("MyStyleApp", "Exception on: InstanceID.DeleteInstanceID");
                    }
#endif
                    var token = instanceID.GetToken(
                        "1021355216197", GoogleCloudMessaging.InstanceIdScope, null);

                    System.Diagnostics.Debug.WriteLine("GCM Registration Token: " + token);
                    Log.Info("MyStyleApp", "GCM Registration Token: " + token);
                    SendRegistrationToAppServer(token);
                    //Subscribe(token);
                }
            }
            catch (Exception e)
            {
                Log.Debug("MyStyleApp", "Failed to get a registration token: "+e.ToString());
                return;
            }
        }

        void SendRegistrationToAppServer(string token)
        {
            if (!tokenSent)
            {
                // TODO:
                // Add custom implementation here as needed to send the token to your BE.

                tokenSent = true;
            }
        }

        /*void Subscribe(string token)
        {
            var pubSub = GcmPubSub.GetInstance(this);
            pubSub.Subscribe(token, "/topics/global", null);
        }*/
    }
}