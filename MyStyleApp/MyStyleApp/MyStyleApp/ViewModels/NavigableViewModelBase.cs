using MyStyleApp.Services;
using System.Threading.Tasks;
using MvvmCore;
using System;
using Xamarin.Forms;

namespace MyStyleApp.ViewModels
{
    public abstract class NavigableViewModelBase : ViewModelBase
    {
        private bool _isBusy;
        private LocalizedStringsService _localizedStringsService;
        private INavigator _navigator;
        private IUserNotificator _userNotificator;

        public NavigableViewModelBase(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService) : base()
        {
            this._navigator = navigator;
            this._userNotificator = userNotificator;
            this._localizedStringsService = localizedStringsService;
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }

        public IUserNotificator UserNotificator
        {
            get
            {
                return this._userNotificator;
            }
        }

        public LocalizedStringsService LocalizedStrings
        {
            get
            {
                return this._localizedStringsService;
            }
        }

        public async Task ExecuteBlockingUIAsync(Func<Task> action)
        {
            this.IsBusy = true;
            try
            {
                if (action != null)
                {
                    await action();
                }
            }
            catch (Exception ex)
            {
                await this.PushNavPageAsync<ErrorViewModel>((errorVM) =>
                {
                    errorVM.ErrorText = this.LocalizedStrings.GetString("generic_error");
                });
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        #region Navigation

        public Task<TViewModel> SetMainPageAsync<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel
        {
            return this._navigator.SetMainPageAsync(setStateAction);
        }

        public Task<TViewModel> SetMainPageAsync<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel
        {
            return this._navigator.SetMainPageAsync(viewModel);
        }

        public Task<TViewModel> SetMainPageNavPageAsync<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel
        {
            return this._navigator.SetMainPageNavPageAsync(setStateAction);
        }

        public Task<TViewModel> SetMainPageNavPageAsync<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel
        {
            return this._navigator.SetMainPageAsync(viewModel);
        }

        public Task<TViewModel> SetMainPageTabAsync<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel
        {
            return this._navigator.SetMainPageTabAsync(setStateAction);
        }

        public Task<TViewModel> SetMainPageTabAsync<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel
        {
            return this._navigator.SetMainPageTabAsync(viewModel);
        }

        public Task<TViewModel> SetNavPageTabAsync<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel
        {
            return this._navigator.SetNavPageTabAsync(this.Navigation, setStateAction);
        }

        public Task<TViewModel> SetNavPageTabAsync<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel
        {
            return this._navigator.SetNavPageTabAsync(this.Navigation, viewModel);
        }

        public Task<IViewModel> PopNavPageAsync()
        {
            return this._navigator.PopNavPageAsync(this.Navigation);
        }

        public Task<IViewModel> PopNavPageModalAsync()
        {
            return this._navigator.PopNavPageModalAsync(this.Navigation);
        }

        public Task PopNavPageToRootAsync()
        {
            return this._navigator.PopNavPageToRootAsync(this.Navigation);
        }

        public Task<TViewModel> PushNavPageAsync<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel
        {
            return this._navigator.PushNavPageAsync(this.Navigation, setStateAction);
        }

        public Task<TViewModel> PushNavPageAsync<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel
        {
            return this._navigator.PushNavPageAsync(this.Navigation, viewModel);
        }

        public Task<TViewModel> PushNavPageModalAsync<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel
        {
            return this._navigator.PushNavPageModalAsync(this.Navigation, setStateAction);
        }

        public Task<TViewModel> PushNavPageModalAsync<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel
        {
            return this._navigator.PushNavPageModalAsync(this.Navigation, viewModel);
        }

        public Task InsertNavPageBeforeAsync<TViewModel, TViewModelBefore>(
            Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel
            where TViewModelBefore : class, IViewModel
        {
            return this._navigator.
                InsertNavPageBeforeAsync<TViewModel, TViewModelBefore>(this.Navigation, setStateAction);
        }

        public Task InsertNavPageBeforeAsync<TViewModel, TViewModelBefore>(
            TViewModel viewModel, TViewModelBefore viewModelBefore)
            where TViewModel : class, IViewModel
            where TViewModelBefore : class, IViewModel
        {
            return this._navigator.
                InsertNavPageBeforeAsync(this.Navigation, viewModel, viewModelBefore);
        }

        public Task RemoveNavPageAsync<TViewModel>()
            where TViewModel : class, IViewModel
        {
            return this._navigator.RemoveNavPageAsync<TViewModel>(this.Navigation);
        }

        #endregion
    }
}
