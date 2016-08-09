using MyStyleApp.Services;
using MvvmCore;
using MyStyleApp.Services.Backend;
using MyStyleApp.Enums;
using Xamarin.Forms;

namespace MyStyleApp.ViewModels
{
    public class MainStaffViewModel : NavigableViewModelBase
    {
        private EstablishmentAppointmentsViewModel _establishmentAppointmentsViewModel;
        private AccountDetailsViewModel _accountDetailsViewModel;
        private IUsersService _userService;

        public MainStaffViewModel(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService,
            IUsersService userService,
            EstablishmentAppointmentsViewModel establishmentAppointmentsViewModel,
            AccountDetailsViewModel accountDetailsViewModel) : 
            base(navigator, userNotificator, localizedStringsService)
        {
            this._userService = userService;
            this._establishmentAppointmentsViewModel = establishmentAppointmentsViewModel;
            this._accountDetailsViewModel = accountDetailsViewModel;

            this._establishmentAppointmentsViewModel.Parent = this;
            this._accountDetailsViewModel.Parent = this;

            this.SubscribeToMessages();
        }

        private void SubscribeToMessages()
        {
            MessagingCenter.Subscribe<string>(this, "userLogin", (userType) =>
            {
                if (userType == UserTypeEnum.Client.ToString())
                {
                    MessagingCenter.Subscribe<string>(this, "pushNotificationReceived", this.OnPushNotificacionReceived);
                }
            });
            MessagingCenter.Subscribe<string>(this, "userLogout", (userType) =>
            {
                if (userType == UserTypeEnum.Client.ToString())
                {
                    MessagingCenter.Unsubscribe<string>(this, "pushNotificationReceived");
                }
            });

            // Need to subscribe on ctor as well since first userLogin message is delivered before this object is created
            MessagingCenter.Subscribe<string>(this, "pushNotificationReceived", this.OnPushNotificacionReceived);
        }

        private async void OnPushNotificacionReceived(string context)
        {
            if (context == "staffModified")
            {
                await this.UserNotificator.DisplayAlert(
                    this.LocalizedStrings.GetString("account_modified"),
                    this.LocalizedStrings.GetString("staff_account_modified_body"),
                    this.LocalizedStrings.GetString("ok"));

                await this._userService.LogoutAsync();
                await this.SetMainPageNavPageAsync<LoginViewModel>();
            }
        }

        public void Initialize()
        {
            this._establishmentAppointmentsViewModel.InitializeAsync();
            this._accountDetailsViewModel.Initialize(this._userService.LoggedUser);
        }

        public EstablishmentAppointmentsViewModel EstablishmentAppointmentsViewModel
        {
            get { return this._establishmentAppointmentsViewModel; }
        }

        public AccountDetailsViewModel AccountDetailsViewModel
        {
            get { return this._accountDetailsViewModel; }
        }
    }
}
