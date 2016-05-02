using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MyStyleApp.Views
{
    public partial class RegisteredStoresView : ContentPage
    {
        public RegisteredStoresView()
        {
            InitializeComponent();

            // We just want a back button on iOS
            if (Device.OS != TargetPlatform.iOS)
                NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}
