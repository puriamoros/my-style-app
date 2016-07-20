using MyStyleApp.Services;
using MvvmCore;
using MyStyleApp.Services.Backend;
using MyStyleApp.Enums;

namespace MyStyleApp.ViewModels
{
    public class MainOwnerViewModel : NavigableViewModelBase
    {
        private EstablishmentAppointmentsViewModel _establishmentAppointmentsViewModel;
        private MyEstablishmentsViewModel _myEstablishmentsViewModel;
        private StaffViewModel _staffViewModel;
        private AccountDetailsViewModel _accountDetailsViewModel;
        private IUsersService _userService;

        public MainOwnerViewModel(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService,
            IUsersService userService,
            EstablishmentAppointmentsViewModel establishmentAppointmentsViewModel,
            MyEstablishmentsViewModel myEstablishmentsViewModel,
            StaffViewModel staffViewModel,
            AccountDetailsViewModel accountDetailsViewModel) : 
            base(navigator, userNotificator, localizedStringsService)
        {
            this._userService = userService;
            this._establishmentAppointmentsViewModel = establishmentAppointmentsViewModel;
            this._myEstablishmentsViewModel = myEstablishmentsViewModel;
            this._staffViewModel = staffViewModel;
            this._accountDetailsViewModel = accountDetailsViewModel;

            this._establishmentAppointmentsViewModel.Parent = this;
            this._myEstablishmentsViewModel.Parent = this;
            this._staffViewModel.Parent = this;
            this._accountDetailsViewModel.Parent = this;
        }

        public void Initialize()
        {
            this._establishmentAppointmentsViewModel.InitializeAsync();
            this._myEstablishmentsViewModel.InitializeAsync();
            this._staffViewModel.InitializeAsync();
            this._accountDetailsViewModel.Initialize(this._userService.LoggedUser);

            this.SetMainPageTabAsync<EstablishmentAppointmentsViewModel>();
        }

        public EstablishmentAppointmentsViewModel EstablishmentAppointmentsViewModel
        {
            get { return this._establishmentAppointmentsViewModel; }
        }

        public MyEstablishmentsViewModel MyEstablishmentsViewModel
        {
            get { return this._myEstablishmentsViewModel; }
        }

        public StaffViewModel StaffViewModel
        {
            get { return this._staffViewModel; }
        }

        public AccountDetailsViewModel AccountDetailsViewModel
        {
            get { return this._accountDetailsViewModel; }
        }
    }
}
