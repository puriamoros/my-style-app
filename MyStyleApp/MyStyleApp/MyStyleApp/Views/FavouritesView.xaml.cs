using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MyStyleApp.Views
{
    public partial class FavouritesView : CustomContentPage
    {
        public FavouritesView()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<string>(this, "favouritesRefreshed", (ignored) =>
            {
                this.FavouritesList.IsRefreshing = false;
            });
        }
    }
}
