using System.Threading.Tasks;
using MyStyleApp.Services;
using XamarinFormsAutofacMvvmStarterKit;
using System.Windows.Input;
using Xamarin.Forms;

namespace MyStyleApp.ViewModels
{
    public class AppointmentsViewModel : NavigableViewModelBase
    {
        public ICommand NewAccountCommand { get; private set; }

        public AppointmentsViewModel(
            INavigator navigator,
            LocalizedStringsService localizedStringsService) :
            base(navigator, localizedStringsService)
        {
            this.NewAccountCommand = new Command(async () => await NewAccount());
        }

        private async Task NewAccount()
        {
            await this.PushAsync<LoginViewModel>();
        }
    }
}
