using System.Threading.Tasks;
using MyStyleApp.Services;
using XamarinFormsAutofacMvvmStarterKit;

namespace MyStyleApp.ViewModels
{
    public class AccountViewModel : NavigableViewModelBase
    {
        public AccountViewModel(
            INavigator navigator,
            LocalizedStringsService localizedStringsService) :
            base(navigator, localizedStringsService)
        {
        }
    }
}
