namespace MyStyleApp.Views
{
    public class MainClientView : CustomTabbedPage
    {
        private AppointmentsView _appointmentsView;
        private FavouritesView _favouritesView;
        private SearchView _searchView;
        private AccountDetailsView _accountDetailsView;

        public MainClientView(
            AppointmentsView appointmentsView,
            FavouritesView favouritesView,
            SearchView searchView,
            AccountDetailsView accountDetailsView)
        {
            this._appointmentsView = appointmentsView;
            this._favouritesView = favouritesView;
            this._searchView = searchView;
            this._accountDetailsView = accountDetailsView;

            this.AddNavigationChild(this._appointmentsView, "LocalizedStrings[appointments]", "Calendar.png");
            this.AddNavigationChild(this._favouritesView, "LocalizedStrings[favourites]", "Star.png");
            this.AddNavigationChild(this._searchView, "LocalizedStrings[establishments]", "Barbershop.png");
            this.AddNavigationChild(this._accountDetailsView, "LocalizedStrings[my_account]", "User.png");
        }
    }
}
