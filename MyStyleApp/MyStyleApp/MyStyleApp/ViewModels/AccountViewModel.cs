using System.Threading.Tasks;
using MyStyleApp.Services;
using MvvmCore;

namespace MyStyleApp.ViewModels
{
    public class AccountViewModel : NavigableViewModelBase
    {
        public AccountViewModel(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService) :
            base(navigator, userNotificator, localizedStringsService)
        {
        }
    }
}
