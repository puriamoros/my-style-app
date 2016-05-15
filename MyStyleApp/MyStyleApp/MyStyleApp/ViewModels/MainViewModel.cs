using MyStyleApp.Services;
using MyStyleApp.Services.Backend;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinFormsAutofacMvvmStarterKit;

namespace MyStyleApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ILoginService _loginService;
        private string _message;

        public ICommand LogoutCommand { get; private set; }

        public MainViewModel(
            INavigator navigator,
            LocalizedStringsService localizedStringsService,
            ILoginService loginService) : 
            base(navigator, localizedStringsService)
        {
            this._loginService = loginService;
            this.Message = "Second Page";
            this.LogoutCommand = new Command(async () => await this.Logout());
        }

        private async Task Logout()
        {
            this.IsBusy = true;
            await this._loginService.Logout();
            await this.Navigator.PopToRootAsync();
            await this.Navigator.PushAsync<LoginViewModel>();
            await this.Navigator.RemovePage<MainViewModel>();
            this.IsBusy = false;
        }

        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        public override void OnDisappearing()
        {
            //base.OnDisappearing();
        }
    }
}
