using MyStyleApp.Models;
using MyStyleApp.Services;
using MyStyleApp.Services.Backend;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using MvvmCore;

namespace MyStyleApp.ViewModels
{
    public class MainViewModel : NavigableViewModelBase
    {
        public MainViewModel(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService) : 
            base(navigator, userNotificator, localizedStringsService)
        {
        }
    }
}
