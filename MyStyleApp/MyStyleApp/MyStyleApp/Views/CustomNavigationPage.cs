using Xamarin.Forms;

namespace MyStyleApp.Views
{
    public class CustomNavigationPage : MvvmCore.NavigationPage
    {
        private Page _root;

        public CustomNavigationPage() : base()
        {
        }

        public CustomNavigationPage(Page root) : base (root)
		{
            this._root = root;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // We just want a navigation bar on iOS
            // We have to disable it in both, the NavigationPage and its ContentPage
            if (Device.OS != TargetPlatform.iOS)
            {
                NavigationPage.SetHasNavigationBar(this, false);
                if (this._root != null)
                {
                    NavigationPage.SetHasNavigationBar(this._root, false);
                }
            }
        }
    }
}
