﻿namespace MyStyleApp.Views
{
    public class MainOwnerView : CustomTabbedPage
    {
        private EstablishmentAppointmentsView _establishmentAppointmentsView;
        private OwnerEstablishmentsView _ownerEstablishmentsView;
        private EstablishmentStaffView _establishmentStaffView;
        private AccountDetailsView _accountDetailsView;

        public MainOwnerView(
            EstablishmentAppointmentsView establishmentAppointmentsView,
            OwnerEstablishmentsView ownerEstablishmentsView,
            EstablishmentStaffView establishmentStaffView,
            AccountDetailsView accountDetailsView)
        {
            this._establishmentAppointmentsView = establishmentAppointmentsView;
            this._ownerEstablishmentsView = ownerEstablishmentsView;
            this._establishmentStaffView = establishmentStaffView;
            this._accountDetailsView = accountDetailsView;

            this.AddNavigationChild(this._establishmentAppointmentsView, "LocalizedStrings[appointments]", "Calendar.png");
            this.AddNavigationChild(this._ownerEstablishmentsView, "LocalizedStrings[establishments]", "Barbershop.png");
            this.AddNavigationChild(this._establishmentStaffView, "LocalizedStrings[staff]", "Star.png");
            this.AddNavigationChild(this._accountDetailsView, "LocalizedStrings[my_account]", "User.png");
        }
    }
}
