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

        //    this.AddNavigationChild(appointmentsView, "LocalizedStrings[appointments]");
        //    this.AddNavigationChild(favouritesView, "LocalizedStrings[favourites]");
        //    this.AddNavigationChild(searchView, "LocalizedStrings[search]");
        //    this.AddNavigationChild(accountView, "LocalizedStrings[my_account]");
        //}

        //private void AddNavigationChild(Page view, string titleBindingExpression)
        //{
        //    var nav = new CustomNavigationPage(view);
        //    nav.SetBinding(CustomNavigationPage.TitleProperty, titleBindingExpression);
        //    this.Children.Add(nav);
        //}
    }
}
