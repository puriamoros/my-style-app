using MyStyleApp.Services;
using System.Threading.Tasks;
using XamarinFormsAutofacMvvmStarterKit;
using System;
using Xamarin.Forms;

namespace MyStyleApp.ViewModels
{
    public abstract class NavigableViewModelBase : ViewModelBase
    {
        private bool _isBusy;
        private LocalizedStringsService _localizedStringsService;
        private INavigator _navigator;

        public NavigableViewModelBase(
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

        public Task<TViewModel> SetMainPage<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel
        {
            return this._navigator.SetMainPage(setStateAction);
        }

        public Task<TViewModel> SetMainPage<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel
        {
            return this._navigator.SetMainPage(viewModel);
        }

        public Task<IViewModel> PopAsync()
        {
            return this._navigator.PopAsync(this.Navigation);
        }

        public Task<IViewModel> PopModalAsync()
        {
            return this._navigator.PopModalAsync(this.Navigation);
        }

        public Task PopToRootAsync()
        {
            return this._navigator.PopToRootAsync(this.Navigation);
        }

        public Task<TViewModel> PushAsync<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel
        {
            return this._navigator.PushAsync(this.Navigation, setStateAction);
        }

        public Task<TViewModel> PushAsync<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel
        {
            return this._navigator.PushAsync(this.Navigation, viewModel);
        }

        public Task<TViewModel> PushModalAsync<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel
        {
            return this._navigator.PushModalAsync(this.Navigation, setStateAction);
        }

        public Task<TViewModel> PushModalAsync<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel
        {
            return this._navigator.PushModalAsync(this.Navigation, viewModel);
        }

        public Task InsertPageBefore<TViewModel, TViewModelBefore>(
            Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel
            where TViewModelBefore : class, IViewModel
        {
            return this._navigator.
                InsertPageBefore<TViewModel, TViewModelBefore>(this.Navigation, setStateAction);
        }

        public Task InsertPageBefore<TViewModel, TViewModelBefore>(
            TViewModel viewModel, TViewModelBefore viewModelBefore)
            where TViewModel : class, IViewModel
            where TViewModelBefore : class, IViewModel
        {
            return this._navigator.
                InsertPageBefore(this.Navigation, viewModel, viewModelBefore);
        }

        public Task RemovePage<TViewModel>()
            where TViewModel : class, IViewModel
        {
            return this._navigator.RemovePage<TViewModel>(this.Navigation);
        }
    }
}
