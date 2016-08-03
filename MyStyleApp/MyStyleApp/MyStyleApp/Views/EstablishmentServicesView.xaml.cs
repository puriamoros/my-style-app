using MyStyleApp.Models;
using MyStyleApp.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MyStyleApp.Views
{
    public partial class EstablishmentServicesView : CustomContentPage
    {
        public EstablishmentServicesView()
        {
            InitializeComponent();
            
            this.CancelButton.Clicked += OnCancelClicked;
        }

        private async void OnCancelClicked(object sender, System.EventArgs e)
        {
            await Task.Delay(100);
            this.GroupedServiceList.Focus();
        }
    }
}
