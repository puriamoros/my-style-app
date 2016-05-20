using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MyStyleApp.Views
{
    public partial class LoginView : ContentPage
    {
        public LoginView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        //protected override bool OnBackButtonPressed()
        //{
        //    // Disable hardware back button
        //    return true;
        //    //return base.OnBackButtonPressed();
        //}
    }
}
