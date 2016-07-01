using MyStyleApp.Constants;
using MyStyleApp.Models;
using MyStyleApp.Services;
using MyStyleApp.Services.Backend;
using MyStyleApp.Validators;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using MvvmCore;
using MyStyleApp.Enums;

namespace MyStyleApp.ViewModels
{
    public class LoginViewModel : NavigableViewModelBase
    {
        private const string STRING_ERROR_INVALID_FIELD = "error_invalid_field";
        private const string STRING_ERROR_INSECURE_CHARS = "error_insecure_chars";
        private const string STRING_ERROR = "error";
        private const string STRING_EMAIL = "email";
        private const string STRING_PASSWORD = "password";
        private const string STRING_LOGIN_ERROR = "login_error";

        public ICommand LoginCommand { get; private set; }
        public ICommand NewAccountCommand { get; private set; }

        private IUsersService _usersService;
        private ValidationService _validationService;

        private string _email;
        private string _password;
        private bool _rememberMe;
        private string _errorText;

        public LoginViewModel(
            AppFlowController appFlowController,
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService,
            IUsersService usersService,
            ValidationService validationService) :
            base(appFlowController, navigator, userNotificator, localizedStringsService)
        {
            this._usersService = usersService;
            this._validationService = validationService;
            this.LoginCommand = new Command(this.LoginAsync);
            this.NewAccountCommand = new Command(this.NewAccountAsync);

            // REMOVE!!!
            FillWithMockData();
        }

        private void FillWithMockData()
        {
            this.Email = "helio.huete@gmail.com";
            this.Password = "helio";
        }

        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
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

        private string GetValidationError()
        {
            // Alwais clear validators before adding
            this._validationService.ClearValidators();

            // Email
            this._validationService.AddValidator(
                new RequiredValidator(this.Email, STRING_EMAIL));
            this._validationService.AddValidator(
                new RegexValidator(
                    this.Email, RegexConstants.NOT_INSECURE_CHARS,
                    STRING_ERROR_INSECURE_CHARS, STRING_EMAIL));
            this._validationService.AddValidator(
                new RegexValidator(
                    this.Email, RegexConstants.EMAIL,
                    STRING_ERROR_INVALID_FIELD, STRING_EMAIL));

            // Password
            this._validationService.AddValidator(
                new RequiredValidator(this.Password, STRING_PASSWORD));

            return this._validationService.GetValidationError();
        }

        private async void LoginAsync()
        {
            string validationError = this.GetValidationError();
            if(validationError != null)
            {
                this.ErrorText = this.LocalizedStrings.GetString(STRING_ERROR) +": " + validationError;
                return;
            }

            this.ErrorText = "";

            await this.ExecuteBlockingUIAsync(
                async () =>
                {
                    try
                    {
                        await this._usersService.LoginAsync(this.Email, this.Password, this.RememberMe);

                        //await this.AppFlowController.AddViewModel<MainViewModel>(null, (mainVM) =>
                        //{
                        //    mainVM.Initialize();
                        //});

                        await this.AppFlowController.AddContainer(null, "MainTabbed", ContainerTypeEnum.TabbedContainer, async () =>
                        {
                            await this.AppFlowController.AddContainer("MainTabbed", "AppointmentNavigation", ContainerTypeEnum.NavigationContainer, async () =>
                            {
                                await this.AppFlowController.AddViewModel<AppointmentsViewModel>("AppointmentNavigation");
                            });
                            await this.AppFlowController.AddContainer("MainTabbed", "FavouritesNavigation", ContainerTypeEnum.NavigationContainer, async () =>
                            {
                                await this.AppFlowController.AddViewModel<FavouritesViewModel>("FavouritesNavigation");
                            });
                            await this.AppFlowController.AddContainer("MainTabbed", "SearchNavigation", ContainerTypeEnum.NavigationContainer, async () =>
                            {
                                await this.AppFlowController.AddViewModel<SearchViewModel>("SearchNavigation");
                                await this.AppFlowController.SetCurrentViewModel<SearchViewModel>();
                            });
                            await this.AppFlowController.AddContainer("MainTabbed", "AccountNavigation", ContainerTypeEnum.NavigationContainer, async () =>
                            {
                                await this.AppFlowController.AddViewModel<AccountDetailsViewModel>("AccountNavigation", (accountVM) =>
                                {
                                    accountVM.Initialize(this._usersService.LoggedUser);
                                });
                            });
                        });

                        //await this.AppFlowController.AddContainer("MainTabbed", "AppointmentNavigation", ContainerTypeEnum.NavigationContainer);
                        //await this.AppFlowController.AddContainer("MainTabbed", "FavouritesNavigation", ContainerTypeEnum.NavigationContainer);
                        //await this.AppFlowController.AddContainer("MainTabbed", "SearchNavigation", ContainerTypeEnum.NavigationContainer);
                        //await this.AppFlowController.AddContainer("MainTabbed", "AccountNavigation", ContainerTypeEnum.NavigationContainer);
                        //await this.AppFlowController.AddViewModel<AppointmentsViewModel>("AppointmentNavigation");
                        //await this.AppFlowController.AddViewModel<FavouritesViewModel>("FavouritesNavigation");
                        //await this.AppFlowController.AddViewModel<SearchViewModel>("SearchNavigation");
                        //await this.AppFlowController.AddViewModel<AccountDetailsViewModel>("AccountNavigation", (accountVM) =>
                        //{
                        //    accountVM.Initialize(this._usersService.LoggedUser);
                        //});
                    }
                    catch (Exception)
                    {
                        ErrorText = this.LocalizedStrings[STRING_LOGIN_ERROR];
                    }
                });
        }

        private async void NewAccountAsync()
        {
            await this.ExecuteBlockingUIAsync(
                async () =>
                {
                    await this.AppFlowController.AddViewModel<CreateAccountViewModel>("LoginNavigation",(accountVM) => 
                    {
                        accountVM.Initialize(null);
                    });
                });
        }
    }
}
