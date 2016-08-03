using MyStyleApp.Models;
using MyStyleApp.Utils;
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
            
            this.CancelButton.Clicked += OnCancelClicked;
        }

        private void OnServiceSwitchSizeChanged(object sender, System.EventArgs e)
        {
            if(Device.OS == TargetPlatform.Android && sender is Switch)
            {
                Switch sw = sender as Switch;
                if(sw.Parent != null && sw.Parent is VisualElement)
                {
                    VisualElement parent = sw.Parent as VisualElement;
                    var maxWidth = parent.Width * 0.2;
                    if(sw.Width > maxWidth)
                    {
                        var scale = sw.Width / maxWidth;
                        sw.Scale = scale;
                    }
                }
            }
        }

        private async void OnCancelClicked(object sender, System.EventArgs e)
        {
            await Task.Delay(100);
            this.GroupedServiceList.Focus();
        }
    }
}
