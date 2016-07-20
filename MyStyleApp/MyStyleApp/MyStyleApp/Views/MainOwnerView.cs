namespace MyStyleApp.Views
{
    public class MainOwnerView : CustomTabbedPage
    {
        private EstablishmentAppointmentsView _establishmentAppointmentsView;
        private MyEstablishmentsView _myEstablishmentsView;
        private StaffView _staffView;
        private AccountDetailsView _accountDetailsView;

        public MainOwnerView(
            EstablishmentAppointmentsView establishmentAppointmentsView,
            MyEstablishmentsView myEstablishmentsView,
            StaffView staffView,
            AccountDetailsView accountDetailsView)
        {
            this._establishmentAppointmentsView = establishmentAppointmentsView;
            this._myEstablishmentsView = myEstablishmentsView;
            this._staffView = staffView;
            this._accountDetailsView = accountDetailsView;

            this.AddNavigationChild(this._establishmentAppointmentsView, "LocalizedStrings[appointments]", "Calendar.png");
            this.AddNavigationChild(this._myEstablishmentsView, "LocalizedStrings[establishments]", "Barbershop.png");
            this.AddNavigationChild(this._staffView, "LocalizedStrings[staff]", "Star.png");
            this.AddNavigationChild(this._accountDetailsView, "LocalizedStrings[my_account]", "User.png");
        }
    }
}
