using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MyStyleApp.Views
{
    public partial class EstablishmentAppointmentsView : CustomContentPage
    {
        public EstablishmentAppointmentsView()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<string>(this, "establishmentAppointmentsRefreshed", (ignored) =>
            {
                this.AppointmentList.IsRefreshing = false;
            });
        }

        private void OnViewCellAppearing(object sender, EventArgs e)
        {
            if(sender is ViewCell &&
                this.AppointmentList.BindingContext is ViewModels.EstablishmentAppointmentsViewModel)
            {
                ViewCell viewCell = sender as ViewCell;
                ViewModels.EstablishmentAppointmentsViewModel viewModel =
                    this.AppointmentList.BindingContext as ViewModels.EstablishmentAppointmentsViewModel;

                if (!viewModel.IsStaffAuthorized)
                {
                    // Remove context actions for authorized staff only
                    List<MenuItem> removeList = new List<MenuItem>();
                    foreach (var menuItem in viewCell.ContextActions)
                    {
                        if (menuItem.ClassId == "AuthorizedOnly")
                        {
                            removeList.Add(menuItem);
                        }
                    }
                    foreach (var menuItem in removeList)
                    {
                        viewCell.ContextActions.Remove(menuItem);
                    }
                }
            }
        }
    }
}
