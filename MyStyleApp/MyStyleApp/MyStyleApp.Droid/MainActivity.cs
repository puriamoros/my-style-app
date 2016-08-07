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
            base.Window.RequestFeature(WindowFeatures.ActionBar);
            base.SetTheme(global::Android.Resource.Style.ThemeDeviceDefault);
            base.OnCreate(bundle);

            base.ActionBar.Hide();

            global::Xamarin.Forms.Forms.Init(this, bundle);
            Xamarin.FormsMaps.Init(this, bundle);
            LoadApplication(new App());

            // Hide navigation bar icon
            base.ActionBar.SetIcon(Android.Resource.Color.Transparent);

            //// Center navigation bar title: DO NOT WORK WELL IN ALL DEVICES
            //int titleId = 0;
            //if (Build.VERSION.SdkInt >= BuildVersionCodes.Honeycomb)
            //{
            //    titleId = Resources.GetIdentifier("action_bar_title", "id", "android");
            //}
            //else
            //{
            //    titleId = Resource.Id.action_bar_title;
            //}
            //var titleTextView = FindViewById<TextView>(titleId);
            //var layoutParams = (LinearLayout.LayoutParams)titleTextView.LayoutParameters;
            //layoutParams.Gravity = GravityFlags.CenterHorizontal;
            //layoutParams.Width = Resources.DisplayMetrics.WidthPixels;
            //titleTextView.LayoutParameters = layoutParams;
            //titleTextView.Gravity = GravityFlags.Center;
            //// Added padding because it is slightly off centered
            ////titleTextView.SetPadding(-150, 0, 0, 0);

            base.ActionBar.Show();
        }
    }
}

