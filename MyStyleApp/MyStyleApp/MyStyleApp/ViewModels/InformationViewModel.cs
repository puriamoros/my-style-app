using MyStyleApp.Constants;
using MyStyleApp.Services;
using MyStyleApp.Services.Backend;
using MyStyleApp.Validators;
using System;
using System.Windows.Input;
using Xamarin.Forms;
using MvvmCore;

namespace MyStyleApp.ViewModels
{
    public class InformationViewModel : NavigableViewModelBase
    {
        public InformationViewModel(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService) :
            base(navigator, userNotificator, localizedStringsService)
        {
            
        }      
    }
}
