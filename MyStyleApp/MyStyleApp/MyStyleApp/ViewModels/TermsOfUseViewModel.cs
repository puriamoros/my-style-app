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
    public class TermsOfUseViewModel : NavigableViewModelBase
    {
        public TermsOfUseViewModel(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService) :
            base(navigator, userNotificator, localizedStringsService)
        {
            this.Title = this.LocalizedStrings.GetString("terms_of_use_title");
        }      
    }
}
