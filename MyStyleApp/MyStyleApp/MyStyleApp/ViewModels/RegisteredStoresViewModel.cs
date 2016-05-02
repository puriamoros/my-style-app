using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinFormsAutofacMvvmStarterKit;

namespace MyStyleApp.ViewModels
{
    public class RegisteredStoresViewModel : ViewModelBase
    {
        private string _message;
        private readonly INavigator _navigator;

        public ICommand BackCommand { get; private set; }

        public RegisteredStoresViewModel(INavigator navigator)
        {
            this._navigator = navigator;
            this.Message = "Second Page";
            this.BackCommand = new Command(async () => await this._navigator.PopAsync());
        }

        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }
    }
}
