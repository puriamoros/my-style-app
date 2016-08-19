using MyStyleApp.Models;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MyStyleApp.Views
{
    public partial class EstablishmentServicesView : CustomContentPage
    {
        public EstablishmentServicesView()
        {
            InitializeComponent();
            
            this.ClearButton1.Clicked += OnClearClicked;
            this.ClearButton2.Clicked += OnClearClicked;
        }

        private async void OnClearClicked(object sender, System.EventArgs e)
        {
            await Task.Delay(100);
            this.GroupedServiceList.Focus();
        }
    }
}
