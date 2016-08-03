using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MyStyleApp.Views
{
    public partial class EstablishmentDetailsView : CustomContentPage
    {
        public EstablishmentDetailsView()
        {
            InitializeComponent();
            if(Device.OS == TargetPlatform.Android)
            {
                this.SizeChanged += OnPageSizeChanged;
            }
        }

        private void OnPageSizeChanged(object sender, EventArgs e)
        {
            if(this.MainGrid.HeightRequest < this.Height)
            {
                this.MainGrid.HeightRequest = this.Height;
            }
        }
    }
}
