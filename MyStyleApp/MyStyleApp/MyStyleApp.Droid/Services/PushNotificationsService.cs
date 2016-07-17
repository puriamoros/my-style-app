using Android.Gms.Common;
using Android.Content;
using Xamarin.Forms;

[assembly: Dependency(typeof(MyStyleApp.Droid.Services.PushNotificationsService))]
namespace MyStyleApp.Droid.Services
{
    public class PushNotificationsService : MyStyleApp.Services.IPushNotificationsService
    {
        public void RegisterDevice()
        {
            if (IsPlayServicesAvailable())
            {
                var intent = new Intent(Android.App.Application.Context, typeof(RegistrationIntentService));
                Android.App.Application.Context.StartService(intent);
            }
        }

        private bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(Android.App.Application.Context);
            if (resultCode != ConnectionResult.Success)
            {
                string error = "";
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