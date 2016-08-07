using Xamarin.Forms;

namespace MyStyleApp.Views
{
    public class CustomTabbedPage : TabbedPage
    {
        private const string CURRENT_TOKEN = "Filled";

        public CustomTabbedPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected void AddNavigationChild(Page view, string titleBinding, string icon)
        {
            var nav = new CustomNavigationPage(view);
            nav.SetBinding(CustomNavigationPage.TitleProperty, titleBinding);
            nav.Icon = icon;
            this.Children.Add(nav);
        }

        protected void AddChild(Page view, string titleBinding, string icon)
        {
            view.SetBinding(CustomNavigationPage.TitleProperty, titleBinding);
            view.Icon = icon;
            this.Children.Add(view);
        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();

            foreach (var child in this.Children)
            {
                SetIcon(child, child == this.CurrentPage);
            }
        }

        private void SetIcon(Page page, bool isCurrentPage)
        {
            string icon = page.Icon;
            if (!isCurrentPage && icon.Contains(CURRENT_TOKEN))
            {
                page.Icon = icon.Replace(CURRENT_TOKEN, "");
            }
            else if (isCurrentPage && !icon.Contains(CURRENT_TOKEN))
            {
                int pos = icon.LastIndexOf(".");
                page.Icon = icon.Substring(0, pos) + CURRENT_TOKEN + icon.Substring(pos);
            }
        }
    }
}
