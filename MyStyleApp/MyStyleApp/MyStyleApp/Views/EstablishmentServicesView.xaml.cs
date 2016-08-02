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
            
            this.SearchEntry.TextChanged += OnSearchTextChanged;
            this.CancelButton.Clicked += OnCancelClicked;
        }

        private async void OnCancelClicked(object sender, System.EventArgs e)
        {
            this.SearchEntry.Text = "";
            await Task.Delay(100);
            this.GroupedServiceList.Focus();
            this.CancelButton.IsVisible = false;
        }

        private void OnSearchTextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (this.GroupedServiceList.ItemsSource != null && this.SearchEntry.Text != null)
            {
                var text = this.SearchEntry.Text.ToLower();
                this.CancelButton.IsVisible = text.Length > 0;
                var enumerator = this.GroupedServiceList.ItemsSource.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current is Grouping<string, SelectedService>)
                    {
                        var grouping = enumerator.Current as Grouping<string, SelectedService>;
                        foreach (var service in grouping)
                        {
                            bool show = service.Name.ToLower().Contains(text);
                            service.Visible = show;
                            service.HeightRequest = (show) ? -1 : 0;
                        }
                    }
                }
            }
        }
    }
}
