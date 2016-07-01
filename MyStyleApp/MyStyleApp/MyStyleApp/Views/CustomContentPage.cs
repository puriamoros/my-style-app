using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MyStyleApp.Views
{
    public class CustomContentPage: ContentPage
    {
        public CustomContentPage() : base()
        {
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // We just want a navigation bar on iOS
            if (Device.OS != TargetPlatform.iOS)
            {
                NavigationPage.SetHasNavigationBar(this, false);
                if(this.Parent is NavigationPage)
                {
                    NavigationPage.SetHasNavigationBar(this.Parent, false);
                }
            }
        }
    }
}
