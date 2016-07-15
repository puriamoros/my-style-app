using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Gms.Common;

namespace MyStyleApp.Droid
{
    [Activity(Theme = "@style/Theme.Splash", Label = "MyStyleApp", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            // Revert theme and window features from the splash.
            base.Window.RequestFeature(WindowFeatures.ActionBar);
            base.SetTheme(global::Android.Resource.Style.ThemeDeviceDefault);
            base.OnCreate(bundle);

            base.ActionBar.Hide();

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());

            base.ActionBar.Show();

            // Push notifications
            if (IsPlayServicesAvailable())
            {
                var intent = new Intent(this, typeof(RegistrationIntentService));
                StartService(intent);
            }
        }

        private bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                String error = "";
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                {
                    error = GoogleApiAvailability.Instance.GetErrorString(resultCode);
                }
                else
                {
                    error = "This device is not supported";
                }
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}

