using System.Threading.Tasks;
using MyStyleApp.Services;
using MvvmCore;
using System.Windows.Input;
using Xamarin.Forms;

namespace MyStyleApp.ViewModels
{
    public class AppointmentsViewModel : NavigableViewModelBase
    {
        public ICommand NewAccountCommand { get; private set; }

        public AppointmentsViewModel(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService) :
            base(navigator, userNotificator, localizedStringsService)
        {
            this.NewAccountCommand = new Command(async () => await NewAccount());
        }

        private async Task NewAccount()
        {
            await this.PushAsync<LoginViewModel>();
        }
    }
}
