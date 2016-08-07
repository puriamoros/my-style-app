namespace MyStyleApp.Views
{
    public class MainStaffView : CustomTabbedPage
    {
        private EstablishmentAppointmentsView _establishmentAppointmentsView;
        private EstablishmentStaffView _establishmentStaffView;
        private AccountDetailsView _accountDetailsView;

        public MainStaffView(
            EstablishmentAppointmentsView establishmentAppointmentsView,
            AccountDetailsView accountDetailsView)
        {
            this._establishmentAppointmentsView = establishmentAppointmentsView;
            this._accountDetailsView = accountDetailsView;

            this.AddChild(this._establishmentAppointmentsView, "LocalizedStrings[appointments]", "Appointments.png");
            this.AddChild(this._accountDetailsView, "LocalizedStrings[my_account]", "User.png");
        }
    }
}
