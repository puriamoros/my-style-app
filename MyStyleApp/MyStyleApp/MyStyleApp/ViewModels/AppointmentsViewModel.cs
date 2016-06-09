﻿using System.Threading.Tasks;
using MyStyleApp.Services;
using MvvmCore;
using System.Windows.Input;
using Xamarin.Forms;
using MyStyleApp.Services.Backend;

namespace MyStyleApp.ViewModels
{
    public class AppointmentsViewModel : NavigableViewModelBase
    {
        public ICommand NewAccountCommand { get; private set; }

        private IUsersService _userService;

        public AppointmentsViewModel(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService,
            IUsersService userService) :
            base(navigator, userNotificator, localizedStringsService)
        {
            this._userService = userService;
            this.NewAccountCommand = new Command(this.NewAccountAsync);
        }

        private async void NewAccountAsync()
        {
            await this._userService.LogoutAsync();
            await this.SetMainPageAsync<LoginViewModel>();
        }
    }
}