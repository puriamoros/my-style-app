using System.Threading.Tasks;
using MyStyleApp.Services;
using MvvmCore;

namespace MyStyleApp.ViewModels
{
    public class SearchViewModel : NavigableViewModelBase
    {
        public SearchViewModel(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService) :
            base(navigator, userNotificator, localizedStringsService)
        {
        }
    }
}
