using MyStyleApp.Services;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinFormsAutofacMvvmStarterKit;

namespace MyStyleApp.ViewModels
{
    public class RegisteredStoresViewModel : ViewModelBase
    {
        private string _message;

        public ICommand BackCommand { get; private set; }

        public RegisteredStoresViewModel(
            INavigator navigator,
            LocalizedStringsService localizedStringsService) : 
            base(navigator, localizedStringsService)
        {
            this.Message = "Second Page";
            this.BackCommand = new Command(async () => await this.Navigator.PopAsync());
        }

        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }
    }
}
