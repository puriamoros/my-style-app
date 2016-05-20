using MyStyleApp.Models;
using MyStyleApp.Services;
using MyStyleApp.Services.Backend;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinFormsAutofacMvvmStarterKit;

namespace MyStyleApp.ViewModels
{
    public class MainViewModel : TabbedViewModelBase
    {
        public MainViewModel(
            INavigator navigator,
            LocalizedStringsService localizedStringsService,
            AppointmentsViewModel appointmentsViewModel,
            FavouritesViewModel favouritesViewModel,
            SearchViewModel searchViewModel,
            AccountViewModel accountViewModel) : 
            base(navigator, localizedStringsService)
        {
            var childViewModels = new List<IViewModel>(4);
            childViewModels.Add(appointmentsViewModel);
            childViewModels.Add(favouritesViewModel);
            childViewModels.Add(searchViewModel);
            childViewModels.Add(accountViewModel);

            this.Children = childViewModels;
        }
    }
}
