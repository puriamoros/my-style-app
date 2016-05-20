using MyStyleApp.Services;
using XamarinFormsAutofacMvvmStarterKit;

namespace MyStyleApp.ViewModels
{
    public class TabbedViewModelBase : XamarinFormsAutofacMvvmStarterKit.TabbedViewModelBase
    {
        private bool _isBusy;
        private LocalizedStringsService _localizedStringsService;
        private INavigator _navigator;

        public TabbedViewModelBase(
            INavigator navigator,
            LocalizedStringsService localizedStringsService) : base()
        {
            this._navigator = navigator;
            this._localizedStringsService = localizedStringsService;
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }

        public INavigator Navigator
        {
            get
            {
                return this._navigator;
            }
        }

        public LocalizedStringsService LocalizedStrings
        {
            get
            {
                return this._localizedStringsService;
            }
        }
    }
}
