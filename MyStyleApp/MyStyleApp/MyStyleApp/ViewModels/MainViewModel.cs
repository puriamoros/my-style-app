using MyStyleApp.Models;
using MyStyleApp.Services;
using MyStyleApp.Services.Backend;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinFormsAutofacMvvmStarterKit;

namespace MyStyleApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private const string STRING_WELCOME_USER = "welcome_user";
        private const string TOKEN_USER_NAME = "${USER_NAME}";

        private IUsersService _usersService;
        private string _welcomeUser;

        public ICommand LogoutCommand { get; private set; }

        public MainViewModel(
            INavigator navigator,
            LocalizedStringsService localizedStringsService,
            IUsersService usersService) : 
            base(navigator, localizedStringsService)
        {
            this._usersService = usersService;
            this.LogoutCommand = new Command(async () => await this.Logout());
        }

        public override void OnPushed()
        {
            base.OnPushed();

            this.WelcomeUser = this.LocalizedStrings.GetString(
                STRING_WELCOME_USER, TOKEN_USER_NAME, this._usersService.LoggedUser.Name);
        }

        private async Task Logout()
        {
            this.IsBusy = true;
            try
            {
                await this._usersService.Logout();
            }
            catch(Exception ex)
            {
            }
            finally
            {
                await this.Navigator.PopToRootAsync();
                await this.Navigator.PushAsync<LoginViewModel>();
                await this.Navigator.RemovePage<MainViewModel>();
                this.IsBusy = false;
            }
        }

        public string WelcomeUser
        {
            get { return _welcomeUser; }
            set { SetProperty(ref _welcomeUser, value); }
        }
    }
}
