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

        //private ISharedPreferences prefs;
        //private bool tokenRegenerated;

        public RegistrationIntentService() : base("RegistrationIntentService")
        {
            //prefs = PreferenceManager.GetDefaultSharedPreferences(Android.App.Application.Context);
            //tokenRegenerated = prefs.GetBoolean("tokenRegenerated", false);
            //Log.Info("MyStyleApp", "tokenRegenerated: " + tokenRegenerated);
        }

        protected override void OnHandleIntent(Intent intent)
        {
            try
            {
                lock (locker)
                {
                    var instanceID = InstanceID.GetInstance(Android.App.Application.Context);

                    //Log.Info("MyStyleApp", "tokenRegenerated2: " + tokenRegenerated);
                    //if (!tokenRegenerated)
                    //{
                    //try
                    //{
                    //    Log.Info("MyStyleApp", "Calling: InstanceID.DeleteInstanceID");
                    //    instanceID.DeleteInstanceID();
                    //}
                    //catch (Exception e)
                    //{
                    //    Log.Info("MyStyleApp", "Exception on: InstanceID.DeleteInstanceID");
                    //}

                    //    tokenRegenerated = true;
                    //    ISharedPreferencesEditor editor = prefs.Edit();
                    //    editor.PutBoolean("tokenRegenerated", true);
                    //    editor.Apply();
                    //}

                    Log.Info("MyStyleApp", "Calling InstanceID.GetToken");
                    var token = instanceID.GetToken("1021355216197", GoogleCloudMessaging.InstanceIdScope, null);
                    Log.Info("MyStyleApp", "GCM Registration Token: " + token);
                    
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