using System.Threading.Tasks;
using MyStyleApp.Services;
using MvvmCore;
using System.Windows.Input;
using Xamarin.Forms;
using MyStyleApp.Services.Backend;

namespace MyStyleApp.ViewModels
{
    public class AppointmentsViewModel : NavigableViewModelBase
    {
        //public ICommand NewAccountCommand { get; private set; }

        private IUsersService _userService;

        public AppointmentsViewModel(
            AppFlowController appFlowController,
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService,
            IUsersService userService) :
            base(appFlowController, navigator, userNotificator, localizedStringsService)
        {
            this._userService = userService;
            //this.NewAccountCommand = new Command(this.NewAccountAsync);
        }

        //public int MyProperty
        //{
        //    get;
        //    set;
        //}

        //public int MyProperty
        //{
        //    get;
        //    set;
        //}
        //private async void NewAccountAsync()
        //{
        //    await this._userService.LogoutAsync();
        //    await this.SetMainPageAsync<LoginViewModel>();
        //}
    }
}
