using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MyStyleApp.Views.Controls
{
    public class CustomButton : Button
    {
        public CustomButton(): base()
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                this.BorderColor = Color.FromRgb(0, 122, 255);
                this.BorderRadius = 10;
                this.BorderWidth = 2;
                this.FontAttributes = FontAttributes.Bold;
            }
        }
    }
}
