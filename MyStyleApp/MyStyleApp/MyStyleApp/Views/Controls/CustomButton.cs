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
                this.BackgroundColor = Color.FromRgb(0, 122, 255);
                this.BorderColor = Color.FromRgb(0, 122, 255);
                this.BorderRadius = 5;
                this.BorderWidth = 1;
                this.TextColor = Color.White;
                this.FontAttributes = FontAttributes.Bold;
            }
        }
    }
}
