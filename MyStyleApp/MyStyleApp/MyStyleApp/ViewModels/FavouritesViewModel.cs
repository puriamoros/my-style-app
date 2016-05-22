using System;
using System.Threading.Tasks;
using MyStyleApp.Services;
using XamarinFormsAutofacMvvmStarterKit;

namespace MyStyleApp.ViewModels
{
    public class FavouritesViewModel : NavigableViewModelBase
    {
        public FavouritesViewModel(
            INavigator navigator,
            LocalizedStringsService localizedStringsService) :
            base(navigator, localizedStringsService)
        {
        }
    }
}
