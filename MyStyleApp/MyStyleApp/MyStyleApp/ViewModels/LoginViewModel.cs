using System.Windows.Input;
using Xamarin.Forms;
using XamarinFormsAutofacMvvmStarterKit;

namespace MyStyleApp.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private string _message;
        private readonly INavigator _navigator;

        public ICommand NextViewCommand { get; private set; }

        public LoginViewModel(INavigator navigator)
        {
            this._navigator = navigator;
            this.Message = "First Page";
            this.NextViewCommand = new Command(async () => await this._navigator.PushAsync<RegisteredStoresViewModel>());
        }

        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }
    }
}
