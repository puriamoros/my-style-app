using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MyStyleApp.Views.Controls
{
    public class CustomButton : Button
    {
        private Color _enabledBackgroundColor = Color.FromRgb(0, 122, 255);
        private Color _disabledBackgroundColor = Color.FromRgb(189, 190, 194);

        public CustomButton(): base()
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                this.PropertyChanged += CustomButton_PropertyChanged;
                this.BackgroundColor = this._enabledBackgroundColor;
                this.BorderColor = this._enabledBackgroundColor;
                this.BorderRadius = 5;
                this.BorderWidth = 1;
                this.TextColor = Color.White;
                this.FontAttributes = FontAttributes.Bold;
            }
        }

        private async void CustomButton_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var name = e.PropertyName;
            if(name == "IsEnabled")
            {
                await Task.Delay(100);
                this.BackgroundColor = (this.IsEnabled) ? this._enabledBackgroundColor : this._disabledBackgroundColor;
                this.BorderColor = (this.IsEnabled) ? this._enabledBackgroundColor : this._disabledBackgroundColor;
            }
        }
    }
}
