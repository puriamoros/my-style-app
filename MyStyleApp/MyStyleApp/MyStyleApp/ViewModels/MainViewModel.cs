using MyStyleApp.Services;
using MvvmCore;
using MyStyleApp.Services.Backend;
using MyStyleApp.Enums;

namespace MyStyleApp.ViewModels
{
    public class MainViewModel : NavigableViewModelBase
    {
        private AccountDetailsViewModel _accountDetailsViewModel;
        private IUsersService _userService;

        public MainViewModel(
            AppFlowController appFlowController,
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService,
            IUsersService userService,
            AccountDetailsViewModel accountDetailsViewModel) : 
            base(appFlowController, navigator, userNotificator, localizedStringsService)
        {
            this._userService = userService;
            this._accountDetailsViewModel = accountDetailsViewModel;
        }

        public void Initialize()
        {
            this._accountDetailsViewModel.Initialize(this._userService.LoggedUser);
            this.SetMainPageTabAsync<AppointmentsViewModel>();
        }
    }
}
