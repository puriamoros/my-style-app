namespace MyStyleApp.Views
{
    public class MainClientView : CustomTabbedPage
    {
        public MainClientView(
            AppointmentsView appointmentsView,
            FavouritesView favouritesView,
            SearchView searchView,
            AccountDetailsView accountDetailsView)
        {
            this.AddNavigationChild(appointmentsView, "LocalizedStrings[appointments]", "Calendar.png");
            this.AddNavigationChild(favouritesView, "LocalizedStrings[favourites]", "Star.png");
            this.AddNavigationChild(searchView, "LocalizedStrings[establishments]", "Barbershop.png");
            this.AddNavigationChild(accountDetailsView, "LocalizedStrings[my_account]", "User.png");
        }
    }
}
