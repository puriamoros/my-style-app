using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStyleApp.Views
{
    public class MainOwnerView : CustomTabbedPage
    {
        public MainOwnerView(
            EstablishmentAppointmentsView establishmentAppointmentsView,
            MyEstablishmentsView myEstablishmentsView,
            StaffView staffView,
            AccountDetailsView accountDetailsView)
        {
            this.AddNavigationChild(establishmentAppointmentsView, "LocalizedStrings[appointments]", "Calendar.png");
            this.AddNavigationChild(myEstablishmentsView, "LocalizedStrings[establishments]", "Barbershop.png");
            this.AddNavigationChild(staffView, "LocalizedStrings[staff]", "Star.png");
            this.AddNavigationChild(accountDetailsView, "LocalizedStrings[my_account]", "User.png");
        }
    }
}
