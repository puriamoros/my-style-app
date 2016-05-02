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

            // Root page, we don't want a back button
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}
