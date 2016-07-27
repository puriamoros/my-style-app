using MyStyleApp.Models;
using MyStyleApp.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MyStyleApp.Views
{
    public partial class EstablishmentServicesView : CustomContentPage
    {
        List<Grouping<string, SelectedService>> originalList;

        public EstablishmentServicesView()
        {
            InitializeComponent();
            this.SearchEntry.TextChanged += SearchEntry_TextChanged;
        }

        private void SearchEntry_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            var text = this.SearchEntry.Text.ToLower();

            if(originalList == null)
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

            var list = new List<Grouping<string, SelectedService>>();
            foreach(var grouping in originalList)
            {
                var servicesList = new List<SelectedService>();
                foreach(var service in grouping)
                {
                    if(service.Name.ToLower().Contains(text))
                    {
                        servicesList.Add(service);
                    }
                }
                if(servicesList.Count > 0)
                {
                    list.Add(new Grouping<string, SelectedService>(grouping.Key, servicesList));
                }
            }

            this.GroupedServiceList.ItemsSource = list;
        }
    }
}
