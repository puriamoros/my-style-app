using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyStyleApp.Services;
using XamarinFormsAutofacMvvmStarterKit;
using MyStyleApp.Services.Backend;

namespace MyStyleApp.ViewModels
{
    public class StartViewModel : NavigableViewModelBase
    {
        private IUsersService _usersService;

        public StartViewModel(
            INavigator navigator,
            LocalizedStringsService localizedStringsService,
            IUsersService usersService) :
            base(navigator, localizedStringsService)
        {
            this._usersService = usersService;
        }

        public override void OnAppearing()
        {
            base.OnAppearing();
            this.NavigateToMainPage();
        }

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
