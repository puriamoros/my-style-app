﻿using Xamarin.Forms;

namespace MyStyleApp.Views
{
    public class CustomNavigationPage : MvvmCore.NavigationPage
    {
        public CustomNavigationPage() : base()
        {
        }

        public CustomNavigationPage(Page root) : base (root)
		{
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // We just want a navigation bar on iOS
            if (Device.OS != TargetPlatform.iOS)
            {
                NavigationPage.SetHasNavigationBar(this, false);
            }
        }
    }
}
