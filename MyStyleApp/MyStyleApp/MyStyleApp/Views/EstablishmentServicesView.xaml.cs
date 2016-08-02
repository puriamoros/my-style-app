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
        List<Grouping<string, SelectedService>> originalList;

        public EstablishmentServicesView()
        {
            InitializeComponent();

            this.SearchEntry.TextChanged += OnSearchTextChanged;
            this.CancelButton.Clicked += OnCancelClicked;

            Xamarin.Forms.MessagingCenter.Subscribe<string>(this, "establishmentServicesInitialized", (ignored) =>
            {
                this.SearchEntry.Text = "";
                this.CancelButton.IsVisible = false;
                this.originalList = null;
            });
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
            if(this.GroupedServiceList.ItemsSource != null)
            {
                if (originalList == null)
                {
                    originalList = new List<Grouping<string, SelectedService>>();

                    var enumerator = this.GroupedServiceList.ItemsSource.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        if (enumerator.Current is Grouping<string, SelectedService>)
                        {
                            var grouping = enumerator.Current as Grouping<string, SelectedService>;
                            originalList.Add(grouping);
                        }
                    }
                }

                if (string.IsNullOrEmpty(this.SearchEntry.Text))
                {
                    this.GroupedServiceList.ItemsSource = originalList;
                }
                else
                {
                    this.CancelButton.IsVisible = true;
                    var text = this.SearchEntry.Text.ToLower();
                    var list = new List<Grouping<string, SelectedService>>();
                    foreach (var grouping in originalList)
                    {
                        var servicesList = new List<SelectedService>();
                        foreach (var service in grouping)
                        {
                            if (service.Name.ToLower().Contains(text))
                            {
                                servicesList.Add(service);
                            }
                        }
                        if (servicesList.Count > 0)
                        {
                            list.Add(new Grouping<string, SelectedService>(grouping.Key, servicesList));
                        }
                    }

                    this.GroupedServiceList.ItemsSource = list;
                }
            }
        }
    }
}
