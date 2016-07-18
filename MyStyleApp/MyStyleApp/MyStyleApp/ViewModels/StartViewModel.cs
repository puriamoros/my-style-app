using System;
using System.Threading.Tasks;
using MyStyleApp.Services;
using MvvmCore;
using MyStyleApp.Services.Backend;

namespace MyStyleApp.ViewModels
{
    public class StartViewModel : NavigableViewModelBase
    {
        private IUsersService _usersService;

        public StartViewModel(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService,
            IUsersService usersService) :
            base(navigator, userNotificator, localizedStringsService)
        {
            this._usersService = usersService;

            //TODO: UNCOMMENT WHEN APP IS READY
            //this.NavigateToMainPageAsync();

            //------------------------------
            //TODO: DELETE WHEN APP IS READY
            _IP = Constants.BackendConstants.BASE_URL;
            _IPVisible = false;
            OkCommand = new Xamarin.Forms.Command(() =>
            {
                Constants.BackendConstants.BASE_URL = this.IP;
                TestIP();
            });
            TestIP();
            //TODO: DELETE WHEN APP IS READY
            //------------------------------
        }

        //------------------------------
        //TODO: DELETE WHEN APP IS READY
        private string _IP;
        public System.Windows.Input.ICommand OkCommand { get; private set; }
        public string IP
        {
            get { return _IP; }
            set { SetProperty(ref _IP, value); }
        }
        private bool _IPVisible;
        public bool IPVisible
        {
            get { return _IPVisible; }
            set { SetProperty(ref _IPVisible, value); }
        }
        private async void TestIP()
        {
            await this.ExecuteBlockingUIAsync(
                async () =>
                {
                    try
                    {
                        HttpService http = new HttpService(null);
                        string result = await http.InvokeExternalAsync(
                            System.Net.Http.HttpMethod.Get,
                            Constants.BackendConstants.BASE_URL.Replace("/v1/", ""),
                            null,
                            null);

                        if(!result.Contains("It's running!!"))
                        {
                            throw new Exception();
                        }

                        IPVisible = false;
                        NavigateToMainPageAsync();
                    }
                    catch (Exception ex)
                    {
                        IPVisible = true;
                    }
                });
        }
        //TODO: DELETE WHEN APP IS READY
        //------------------------------

        //public override void OnAppearing()
        //{
        //    base.OnAppearing();
        //    this.NavigateToMainPage();
        //}

        public async void NavigateToMainPageAsync()
        {
            await this.ExecuteBlockingUIAsync(
                async () =>
                {
                    try
                    {
                        // Try getting the logged user
                        await this._usersService.MeAsync();

                        // There is a logged user, go to main view
                        //await this.SetMainPageAsync<MainViewModel>((mainVM) =>
                        //{
                        //    mainVM.Initialize();
                        //});

                        //await this.SetMainPageAsync<EstablishmentAppointmentsViewModel>((EstablishmentAppointmentsVM) =>
                        //{
                        //    EstablishmentAppointmentsVM.InitializeAsync();
                        //});

                        await this.SetMainPageAsync<MyEstablishmentsViewModel>((MyEstablishmentsVM) =>
                        {
                            MyEstablishmentsVM.InitializeAsync();
                        });
                    }
                    catch (Exception)
                    {
                        // There is no logged user, go to login view
                        await this.SetMainPageNavPageAsync<LoginViewModel>();
                    }
                });
        }
    }
}
