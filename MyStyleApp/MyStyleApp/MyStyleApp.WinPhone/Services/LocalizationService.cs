using Xamarin.Forms;

[assembly: Dependency(typeof(MyStyleApp.WinPhone.Services.LocalizationService))]
namespace MyStyleApp.WinPhone.Services
{
    public class LocalizationService : MyStyleApp.Services.ILocalizationService
    {
        public System.Globalization.CultureInfo GetCurrentCultureInfo()
        {
            //return System.Threading.Thread.CurrentThread.CurrentUICulture;
            string asdf = Windows.System.UserProfile.GlobalizationPreferences.Languages[0];
            return new System.Globalization.CultureInfo(Windows.System.UserProfile.GlobalizationPreferences.Languages[0]);
        }

        public void SetLocale()
        {
            // Nothing to do here. WinPhone does it automatically
        }
    }
}
