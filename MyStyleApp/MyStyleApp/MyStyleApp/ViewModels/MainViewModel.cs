using MyStyleApp.Services;
using MvvmCore;
using MyStyleApp.Services.Backend;
using MyStyleApp.Enums;

namespace MyStyleApp.ViewModels
{
    public class MainViewModel : NavigableViewModelBase
    {
        private AppointmentsViewModel _appointmentsViewModel;
        private FavouritesViewModel _favouritesViewModel;
        private SearchViewModel _searchViewModel;
        private AccountDetailsViewModel _accountDetailsViewModel;
        private IUsersService _userService;

        public MainViewModel(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService,
            IUsersService userService,
            AppointmentsViewModel appointmentsViewModel,
            FavouritesViewModel favouritesViewModel,
            SearchViewModel searchViewModel,
            AccountDetailsViewModel accountDetailsViewModel) : 
            base(navigator, userNotificator, localizedStringsService)
        {
            this._userService = userService;
            this._appointmentsViewModel = appointmentsViewModel;
            this._favouritesViewModel = favouritesViewModel;
            this._searchViewModel = searchViewModel;
            this._accountDetailsViewModel = accountDetailsViewModel;

            this._appointmentsViewModel.Parent = this;
            this._favouritesViewModel.Parent = this;
            this._searchViewModel.Parent = this;
            this._accountDetailsViewModel.Parent = this;
        }

        public void Initialize()
        {
            this._accountDetailsViewModel.Initialize(this._userService.LoggedUser);
            this.SetMainPageTabAsync<AppointmentsViewModel>();
        }

        public AppointmentsViewModel AppointmentsViewModel
        {
            get { return this._appointmentsViewModel; }
        }

        public FavouritesViewModel FavouritesViewModel
        {
            get { return this._favouritesViewModel; }
        }

        public SearchViewModel SearchViewModel
        {
            get { return this._searchViewModel; }
        }

        public AccountDetailsViewModel AccountDetailsViewModel
        {
            get { return this._accountDetailsViewModel; }
        }
    }
}
