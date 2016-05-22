using System.Threading.Tasks;
using MyStyleApp.Services;
using XamarinFormsAutofacMvvmStarterKit;

namespace MyStyleApp.ViewModels
{
    public class SearchViewModel : NavigableViewModelBase
    {
        public SearchViewModel(
            INavigator navigator,
            LocalizedStringsService localizedStringsService) :
            base(navigator, localizedStringsService)
        {
        }
    }
}
