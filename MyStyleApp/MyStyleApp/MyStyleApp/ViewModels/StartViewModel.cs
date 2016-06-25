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
            this.NavigateToMainPageAsync();
        }

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
                        await this.SetMainPageAsync<MainViewModel>((mainVM) =>
                        {
                            mainVM.Initialize();
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
