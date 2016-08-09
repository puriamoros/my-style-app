using MyStyleApp.Services;
using MvvmCore;
using MyStyleApp.Services.Backend;
using MyStyleApp.Enums;
using System.Threading.Tasks;

namespace MyStyleApp.ViewModels
{
    public class MainOwnerViewModel : NavigableViewModelBase
    {
        private EstablishmentAppointmentsViewModel _establishmentAppointmentsViewModel;
        private OwnerEstablishmentsViewModel _ownerEstablishmentsViewModel;
        private EstablishmentStaffViewModel _establishmentStaffViewModel;
        private AccountDetailsViewModel _accountDetailsViewModel;
        private IUsersService _userService;

        public MainOwnerViewModel(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService,
            IUsersService userService,
            EstablishmentAppointmentsViewModel establishmentAppointmentsViewModel,
            OwnerEstablishmentsViewModel ownerEstablishmentsViewModel,
            EstablishmentStaffViewModel establishmentStaffViewModel,
            AccountDetailsViewModel accountDetailsViewModel) : 
            base(navigator, userNotificator, localizedStringsService)
        {
            this._userService = userService;
            this._establishmentAppointmentsViewModel = establishmentAppointmentsViewModel;
            this._ownerEstablishmentsViewModel = ownerEstablishmentsViewModel;
            this._establishmentStaffViewModel = establishmentStaffViewModel;
            this._accountDetailsViewModel = accountDetailsViewModel;

            this._establishmentAppointmentsViewModel.Parent = this;
            this._ownerEstablishmentsViewModel.Parent = this;
            this._establishmentStaffViewModel.Parent = this;
            this._accountDetailsViewModel.Parent = this;
        }

        public void Initialize()
        {
            this._establishmentAppointmentsViewModel.InitializeAsync();
            this._ownerEstablishmentsViewModel.InitializeAsync();
            this._establishmentStaffViewModel.InitializeAsync();
            this._accountDetailsViewModel.Initialize(this._userService.LoggedUser);
        }

        public EstablishmentAppointmentsViewModel EstablishmentAppointmentsViewModel
        {
            get { return this._establishmentAppointmentsViewModel; }
        }

        public OwnerEstablishmentsViewModel OwnerEstablishmentsViewModel
        {
            get { return this._ownerEstablishmentsViewModel; }
        }

        public EstablishmentStaffViewModel EstablishmentStaffViewModel
        {
            get { return this._establishmentStaffViewModel; }
        }

        public AccountDetailsViewModel AccountDetailsViewModel
        {
            get { return this._accountDetailsViewModel; }
        }
    }
}
