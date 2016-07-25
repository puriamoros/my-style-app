﻿namespace MyStyleApp.Views
{
    public class MainClientView : CustomTabbedPage
    {
        private ClientAppointmentsView _clientAppointmentsView;
        private FavouritesView _favouritesView;
        private EstablishmentSearchView _establishmentSearchView;
        private AccountDetailsView _accountDetailsView;

        public MainClientView(
            ClientAppointmentsView clientAppointmentsView,
            FavouritesView favouritesView,
            EstablishmentSearchView establishmentSearchView,
            AccountDetailsView accountDetailsView)
        {
            this._clientAppointmentsView = clientAppointmentsView;
            this._favouritesView = favouritesView;
            this._establishmentSearchView = establishmentSearchView;
            this._accountDetailsView = accountDetailsView;

            this.AddNavigationChild(this._clientAppointmentsView, "LocalizedStrings[appointments]", "Calendar.png");
            this.AddNavigationChild(this._favouritesView, "LocalizedStrings[favourites]", "Star.png");
            this.AddNavigationChild(this._establishmentSearchView, "LocalizedStrings[establishments]", "Barbershop.png");
            this.AddNavigationChild(this._accountDetailsView, "LocalizedStrings[my_account]", "User.png");
        }
    }
}
