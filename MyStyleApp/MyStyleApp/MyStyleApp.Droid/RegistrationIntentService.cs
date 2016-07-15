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
        private bool tokenRegenerated;

        public RegistrationIntentService() : base("RegistrationIntentService")
        {
            tokenSent = false;
            tokenRegenerated = false;
        }

        protected override void OnHandleIntent(Intent intent)
        {
            try
            {
                
                lock (locker)
                {
                    var instanceID = InstanceID.GetInstance(this);

                    if(!tokenRegenerated)
                    {
                        try
                        {
                            Log.Info("MyStyleApp", "Calling: InstanceID.DeleteInstanceID");
                            instanceID.DeleteInstanceID();
                        }
                        catch (Exception e)
                        {
                            Log.Info("MyStyleApp", "Exception on: InstanceID.DeleteInstanceID");
                        }

                        Log.Info("MyStyleApp", "Calling InstanceID.GetToken");
                        var token = instanceID.GetToken("1021355216197", GoogleCloudMessaging.InstanceIdScope, null);
                        Log.Info("MyStyleApp", "GCM Registration Token: " + token);

                        SendRegistrationToAppServer(token);

                        tokenRegenerated = true;
                    }
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
    }
}