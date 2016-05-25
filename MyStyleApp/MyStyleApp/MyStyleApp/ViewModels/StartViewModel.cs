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
            this.NavigateToMainPage();
        }

        //public override void OnAppearing()
        //{
        //    base.OnAppearing();
        //    this.NavigateToMainPage();
        //}

        public async void NavigateToMainPage()
        {
            this.IsBusy = true;
            try
            {
                // Try getting the logged user
                await this._usersService.Me();

                // There is a logged user, go to main view
                await this.SetMainPage<MainViewModel>();
            }
            catch (Exception)
            {
                // There is no logged user, go to login view
                await this.SetMainPage<LoginViewModel>();
            }
            finally
            {
                this.IsBusy = false;
            }
        }
    }
}
