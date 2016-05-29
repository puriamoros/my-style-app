using Xamarin.Forms;

namespace MyStyleApp.Views
{
    public partial class MainView : TabbedPage
    {
        private const string CURRENT_TOKEN = "Filled";
        public MainView()
        {
            InitializeComponent();
        }

        //public MainView(
        //    AppointmentsView appointmentsView,
        //    FavouritesView favouritesView,
        //    SearchView searchView,
        //    AccountView accountView)
        //{
        //    InitializeComponent();

        //    this.AddNavigationChild(appointmentsView, "LocalizedStrings[appointments]", "Calendar.png");
        //    this.AddNavigationChild(favouritesView, "LocalizedStrings[favourites]", "Star.png");
        //    this.AddNavigationChild(searchView, "LocalizedStrings[search]", "Search.png");
        //    this.AddNavigationChild(accountView, "LocalizedStrings[my_account]", "User.png");
        //}

        //private void AddNavigationChild(Page view, string titleBinding, string icon)
        //{
        //    var nav = new CustomNavigationPage(view);
        //    nav.SetBinding(CustomNavigationPage.TitleProperty, titleBinding);
        //    nav.Icon = icon;
        //    this.Children.Add(nav);
        //}

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
