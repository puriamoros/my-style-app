using MyStyleApp.Services;
using MvvmCore;
using MyStyleApp.Services.Backend;
using MyStyleApp.Enums;

namespace MyStyleApp.ViewModels
{
    public class MainClientViewModel : NavigableViewModelBase
    {
        private ClientAppointmentsViewModel _clientAppointmentsViewModel;
        private FavouritesViewModel _favouritesViewModel;
        private EstablishmentSearchViewModel _establishmentSearchViewModel;
        private AccountDetailsViewModel _accountDetailsViewModel;
        private IUsersService _userService;

        public MainClientViewModel(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService,
            IUsersService userService,
            ClientAppointmentsViewModel clientAppointmentsViewModel,
            FavouritesViewModel favouritesViewModel,
            EstablishmentSearchViewModel establishmentSearchViewModel,
            AccountDetailsViewModel accountDetailsViewModel) : 
            base(navigator, userNotificator, localizedStringsService)
        {
            this._userService = userService;
            this._clientAppointmentsViewModel = clientAppointmentsViewModel;
            this._favouritesViewModel = favouritesViewModel;
            this._establishmentSearchViewModel = establishmentSearchViewModel;
            this._accountDetailsViewModel = accountDetailsViewModel;

            this._clientAppointmentsViewModel.Parent = this;
            this._favouritesViewModel.Parent = this;
            this._establishmentSearchViewModel.Parent = this;
            this._accountDetailsViewModel.Parent = this;
        }

        public void Initialize()
        {
            this._clientAppointmentsViewModel.InitializeAsync();
            this._favouritesViewModel.InitializeAsync();
            this._establishmentSearchViewModel.InitializeAsync();
            this._accountDetailsViewModel.Initialize(this._userService.LoggedUser);

            this.SetMainPageTabAsync<ClientAppointmentsViewModel>();
        }

        public ClientAppointmentsViewModel ClientAppointmentsViewModel
        {
            get { return this._clientAppointmentsViewModel; }
        }

        public FavouritesViewModel FavouritesViewModel
        {
            get { return this._favouritesViewModel; }
        }

        public EstablishmentSearchViewModel EstablishmentSearchViewModel
        {
            get { return this._establishmentSearchViewModel; }
        }

        public AccountDetailsViewModel AccountDetailsViewModel
        {
            get { return this._accountDetailsViewModel; }
        }
    }
}
