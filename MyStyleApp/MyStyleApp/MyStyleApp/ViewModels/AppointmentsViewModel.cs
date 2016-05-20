using System.Threading.Tasks;
using MyStyleApp.Services;
using XamarinFormsAutofacMvvmStarterKit;
using System.Windows.Input;
using Xamarin.Forms;

namespace MyStyleApp.ViewModels
{
    public class AppointmentsViewModel : ViewModelBase
    {
        public ICommand NewAccountCommand { get; private set; }
        Views.AppointmentsView _asdf;

        public AppointmentsViewModel(
            INavigator navigator,
            LocalizedStringsService localizedStringsService,
            Views.AppointmentsView asdf) :
            base(navigator, localizedStringsService)
        {
            _asdf = asdf;
            this.NewAccountCommand = new Command(async () => await NewAccount());
        }

        private async Task NewAccount()
        {
            await this.Navigator.PushAsync<LoginViewModel>(_asdf.Navigation);
        }
    }
}
