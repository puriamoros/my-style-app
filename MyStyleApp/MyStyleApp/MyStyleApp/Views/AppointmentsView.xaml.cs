using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MyStyleApp.Views
{
    public partial class AppointmentsView : CustomContentPage
    {
        public AppointmentsView()
        {
            InitializeComponent();
        }

        private void OnLabelSizeChanged(object sender, EventArgs e)
        {
            if(sender is Label)
            {
                Label label = sender as Label;
                if(label.Parent is StackLayout)
                {
                    StackLayout stackLayout = label.Parent as StackLayout;
                    stackLayout.HeightRequest = label.Height;
                }
            }
        }
    }
}
