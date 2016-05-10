using MyStyleApp.Models;
using MyStyleApp.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinFormsAutofacMvvmStarterKit;

namespace MyStyleApp.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        public ICommand LoginCommand { get; private set; }
        public ICommand NewAccountCommand { get; private set; }

        private string _user;
        private string _password;
        private bool _rememberMe;
        private string _errorText;

        public LoginViewModel(
            INavigator navigator,
            LocalizedStringsService localizedStringsService) :
            base(navigator, localizedStringsService)
        {
            this.LoginCommand = new Command(async () => await Login());
            this.NewAccountCommand = new Command(async () => await NewAccount());
        }

        public string User
        {
            get { return _user; }
            set { SetProperty(ref _user, value); }
        }

        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        public bool RememberMe
        {
            get { return _rememberMe; }
            set { SetProperty(ref _rememberMe, value); }
        }

        public string ErrorText
        {
            get { return _errorText; }
            set { SetProperty(ref _errorText, value); }
        }

        private async Task Login()
        {
            await this.Navigator.PushAsync<RegisteredStoresViewModel>();
        }

        private async Task NewAccount()
        {
            this.IsBusy = !this.IsBusy;
            //await this.Navigator.PushAsync<RegisteredStoresViewModel>();
        }
    }
}
