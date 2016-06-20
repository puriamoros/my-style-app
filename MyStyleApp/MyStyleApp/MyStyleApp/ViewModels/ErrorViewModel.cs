using MvvmCore;
using MyStyleApp.Services;
using System.Windows.Input;
using Xamarin.Forms;

namespace MyStyleApp.ViewModels
{
    public class ErrorViewModel : NavigableViewModelBase
    {
        public ICommand CloseCommand { get; private set; }

        private string _errorText;

        public ErrorViewModel(
           INavigator navigator,
           IUserNotificator userNotificator,
           LocalizedStringsService localizedStringsService
           ) :
            base(navigator, userNotificator, localizedStringsService)
        {
            this.CloseCommand = new Command(this.CloseAsync);
        }

        public string ErrorText
        {
            get { return _errorText; }
            set { SetProperty(ref _errorText, value); }
        }

        private async void CloseAsync()
        {
            await this.PopNavPageModalAsync();
        }
    }
}
