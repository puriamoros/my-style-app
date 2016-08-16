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
    [Activity(Theme = "@style/Theme.Splash", Label = "MyStyleApp", Icon = "@drawable/icon", MainLauncher = true,
        ScreenOrientation = ScreenOrientation.Portrait,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            // Revert theme and window features from the splash.
            //base.SetTheme(global::Android.Resource.Style.ThemeDeviceDefault);
            base.SetTheme(Resource.Style.Theme_Activity);
            base.OnCreate(bundle);

            base.ActionBar.Hide();

            global::Xamarin.Forms.Forms.Init(this, bundle);
            Xamarin.FormsMaps.Init(this, bundle);
            LoadApplication(new App());

            // Hide navigation bar icon
            base.ActionBar.SetIcon(Android.Resource.Color.Transparent);
            
            base.ActionBar.Show();
        }
    }
}

