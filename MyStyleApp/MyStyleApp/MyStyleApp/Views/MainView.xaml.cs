using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MyStyleApp.Views
{
    public partial class MainView : TabbedPage
    {
        public MainView()
        {
            InitializeComponent();
        }

        //public MainView(
        //    AppointmentsView appointmentsView,
        //    FavouritesView favouritesView,
        //    SearchView searchView,
        //    AccountView accountView)
        //{
        //    InitializeComponent();

        //    this.AddNavigationChild(appointmentsView, "LocalizedStrings[appointments]", "Calendar.png");
        //    this.AddNavigationChild(favouritesView, "LocalizedStrings[favourites]", "Star.png");
        //    this.AddNavigationChild(searchView, "LocalizedStrings[search]", "Search.png");
        //    this.AddNavigationChild(accountView, "LocalizedStrings[my_account]", "User.png");
        //}

        //private void AddNavigationChild(Page view, string titleBinding, string iconBinding)
        //{
        //    var nav = new CustomNavigationPage(view);
        //    nav.SetBinding(CustomNavigationPage.TitleProperty, titleBinding);
        //    nav.SetBinding(CustomNavigationPage.IconProperty, iconBinding);
        //    this.Children.Add(nav);
        //}
    }
}
