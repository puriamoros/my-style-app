using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MyStyleApp.Views
{
    public partial class LoginView : CustomContentPage
    {
        public LoginView()
        {
            InitializeComponent();
        }

        //protected override bool OnBackButtonPressed()
        //{
        //    // Disable hardware back button
        //    return true;
        //    //return base.OnBackButtonPressed();
        //}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            // We don't want a navigation bar on LoginView
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}
