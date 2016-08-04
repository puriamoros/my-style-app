using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MvvmCore
{
	public class Navigator : INavigator
	{
		private readonly IViewFactory _viewFactory;
		private readonly IDeviceService _deviceService;

        public Navigator(IViewFactory viewFactory, IDeviceService deviceService)
		{
			_viewFactory = viewFactory;
			_deviceService = deviceService;
		}

        private INavigation GetNavigation(INavigation navigation)
        {
            return navigation;
            //return (Application.Current.MainPage is Xamarin.Forms.NavigationPage) ?
            //    ((Xamarin.Forms.NavigationPage)Application.Current.MainPage).Navigation :
            //    navigation;
        }

        public Task<TViewModel> SetMainPageAsync<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel
        {
            var tcs = new TaskCompletionSource<TViewModel>();
            _deviceService.BeginInvokeOnMainThread(() =>
            {
                TViewModel viewModel;
                var view = _viewFactory.Resolve<TViewModel>(out viewModel, setStateAction);
                Application.Current.MainPage = view;

                tcs.SetResult(viewModel);
            });
            return tcs.Task;
        }

        public Task<TViewModel> SetMainPageAsync<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel
        {
            var tcs = new TaskCompletionSource<TViewModel>();
            _deviceService.BeginInvokeOnMainThread(() =>
            {
                var view = _viewFactory.Resolve(viewModel);
                Application.Current.MainPage = view;
                tcs.SetResult(viewModel);
            });
            return tcs.Task;
        }

        public Task<TViewModel> SetMainPageNavPageAsync<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel
        {
            var tcs = new TaskCompletionSource<TViewModel>();
            _deviceService.BeginInvokeOnMainThread(() =>
            {
                TViewModel viewModel;
                var view = _viewFactory.Resolve<TViewModel>(out viewModel, setStateAction);
                Application.Current.MainPage = new NavigationPage(view);

                tcs.SetResult(viewModel);
            });
            return tcs.Task;
        }

        public Task<TViewModel> SetMainPageNavPageAsync<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel
        {
            var tcs = new TaskCompletionSource<TViewModel>();
            _deviceService.BeginInvokeOnMainThread(() =>
            {
                var view = _viewFactory.Resolve(viewModel);
                Application.Current.MainPage = new NavigationPage(view);
                tcs.SetResult(viewModel);
            });
            return tcs.Task;
        }

        public Task<TViewModel> SetMainPageTabAsync<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel
        {
            var tcs = new TaskCompletionSource<TViewModel>();
            _deviceService.BeginInvokeOnMainThread(() =>
            {
                TViewModel viewModel;
                var view = _viewFactory.Resolve<TViewModel>(out viewModel, setStateAction);
                this.SetTab(Application.Current.MainPage, view);
                tcs.SetResult(viewModel);
            });
            return tcs.Task;
        }

        public Task<TViewModel> SetMainPageTabAsync<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel
        {
            var tcs = new TaskCompletionSource<TViewModel>();
            _deviceService.BeginInvokeOnMainThread(() =>
            {
                var view = _viewFactory.Resolve(viewModel);
                this.SetTab(Application.Current.MainPage, view);
                tcs.SetResult(viewModel);
            });
            return tcs.Task;
        }

        public Task<TViewModel> SetNavPageTabAsync<TViewModel>(INavigation navigation, Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel
        {
            var tcs = new TaskCompletionSource<TViewModel>();
            if(this.GetNavigation(navigation) == null)
            {
                tcs.SetResult(null);
            }
            else
            {
                _deviceService.BeginInvokeOnMainThread(() =>
                {
                    TViewModel viewModel;
                    var view = _viewFactory.Resolve<TViewModel>(out viewModel, setStateAction);
                    bool found = false;
                    for (int i = this.GetNavigation(navigation).NavigationStack.Count - 1; i >= 0 && !found; i++)
                    {
                        found = this.SetTab(this.GetNavigation(navigation).NavigationStack[i], view);
                    }
                    tcs.SetResult(viewModel);
                });
            }
            return tcs.Task;
        }

        public Task<TViewModel> SetNavPageTabAsync<TViewModel>(INavigation navigation, TViewModel viewModel)
            where TViewModel : class, IViewModel
        {
            var tcs = new TaskCompletionSource<TViewModel>();
            if (this.GetNavigation(navigation) == null)
            {
                tcs.SetResult(null);
            }
            else
            {
                _deviceService.BeginInvokeOnMainThread(() =>
                {
                    var view = _viewFactory.Resolve(viewModel);
                    bool found = false;
                    for (int i = this.GetNavigation(navigation).NavigationStack.Count - 1; i >= 0 && !found; i++)
                    {
                        found = this.SetTab(this.GetNavigation(navigation).NavigationStack[i], view);
                    }
                    tcs.SetResult(viewModel);
                });
            }
            return tcs.Task;
        }

        private bool SetTab(Page tabbedPage, Page tab)
        {
            if (tabbedPage is TabbedPage)
            {
                var tp = tabbedPage as TabbedPage;
                var child = this.GetTab(tp, tab);
                if (child != null)
                {
                    tp.CurrentPage = child;
                    return true;
                }
            }

            return false;
        }

        private Page GetTab(TabbedPage tabbedPage, Page tab)
        {
            foreach(Page child in tabbedPage.Children)
            {
                if(child == tab)
                {
                    return child;
                }

                if(child is NavigationPage)
                {
                    NavigationPage nav = child as NavigationPage;
                    foreach(Page page in nav.Navigation.NavigationStack)
                    {
                        // We should compare "page == tab" but maybe we have not
                        // created child pages in code but in xaml, so we compare types instead
                        //if (page.GetType() == tab.GetType())
                        //{
                        //    return child;
                        //}

                        if (page == tab)
                        {
                            return child;
                        }
                    }
                }
            }

            return null;
        }

        public Task<IViewModel> PopNavPageAsync(INavigation navigation)
		{
			var tcs = new TaskCompletionSource<IViewModel>();
            if (this.GetNavigation(navigation) == null)
            {
                tcs.SetResult(null);
            }
            else
            {
                _deviceService.BeginInvokeOnMainThread(async () =>
                {
                    Page view = await this.GetNavigation(navigation).PopAsync();
                    tcs.SetResult(view.BindingContext as IViewModel);

                });
            }
            return tcs.Task;
		}

		public Task<IViewModel> PopNavPageModalAsync(INavigation navigation)
		{
			var tcs = new TaskCompletionSource<IViewModel>();
            if (this.GetNavigation(navigation) == null)
            {
                tcs.SetResult(null);
            }
            else
            {
                _deviceService.BeginInvokeOnMainThread(async () =>
                {
                    Page view = await this.GetNavigation(navigation).PopModalAsync();
                    tcs.SetResult(view.BindingContext as IViewModel);
                });
            }
            return tcs.Task;
		}

		public Task PopNavPageToRootAsync(INavigation navigation)
		{
			var tcs = new TaskCompletionSource<object>();
            if (this.GetNavigation(navigation) == null)
            {
                tcs.SetResult(null);
            }
            else
            {
                _deviceService.BeginInvokeOnMainThread(async () =>
                {
                    // WinPhone hack: navigation PopToRootAsync is not working as
                    // spected on WinPhone, so I have to use a workaround
                    if (_deviceService.OS == TargetPlatform.Windows ||
                        _deviceService.OS == TargetPlatform.WinPhone)
                    {
                        INavigation rootNavigation = this.GetNavigation(navigation).NavigationStack[0].Navigation;
                        while (rootNavigation.NavigationStack.Count > 1)
                        {
                            await rootNavigation.PopAsync();
                        }
                    }
                    else
                    {
                        await this.GetNavigation(navigation).PopToRootAsync();
                    }

                    tcs.SetResult(null);
                });
            }
			return tcs.Task;
		}

		public Task<TViewModel> PushNavPageAsync<TViewModel>(
            INavigation navigation, Action<TViewModel> setStateAction = null) 
			where TViewModel : class, IViewModel
		{
			var tcs = new TaskCompletionSource<TViewModel>();
			_deviceService.BeginInvokeOnMainThread(async () =>
			{
				TViewModel viewModel;
				var view = _viewFactory.Resolve(out viewModel, setStateAction);
                await this.GetNavigation(navigation).PushAsync(view);
                tcs.SetResult(viewModel);
			});
			return tcs.Task;
		}

		public Task<TViewModel> PushNavPageAsync<TViewModel>(
            INavigation navigation, TViewModel viewModel) 
			where TViewModel : class, IViewModel
		{
			var tcs = new TaskCompletionSource<TViewModel>();
			_deviceService.BeginInvokeOnMainThread(async () =>
			{
				var view = _viewFactory.Resolve(viewModel);
				await this.GetNavigation(navigation).PushAsync(view);
				tcs.SetResult(viewModel);
			});
			return tcs.Task;
		}

		public Task<TViewModel> PushNavPageModalAsync<TViewModel>(
            INavigation navigation, Action<TViewModel> setStateAction = null) 
			where TViewModel : class, IViewModel
		{
			var tcs = new TaskCompletionSource<TViewModel>();
			_deviceService.BeginInvokeOnMainThread(async () =>
			{
				TViewModel viewModel;
				var view = _viewFactory.Resolve<TViewModel>(out viewModel, setStateAction);
				await this.GetNavigation(navigation).PushModalAsync(view);
				tcs.SetResult(viewModel);
			});
			return tcs.Task;
		}

		public Task<TViewModel> PushNavPageModalAsync<TViewModel>(
            INavigation navigation, TViewModel viewModel) 
			where TViewModel : class, IViewModel
		{
			var tcs = new TaskCompletionSource<TViewModel>();
			_deviceService.BeginInvokeOnMainThread(async () =>
			{
				var view = _viewFactory.Resolve(viewModel);
				await this.GetNavigation(navigation).PushModalAsync(view);
				tcs.SetResult(viewModel);
			});
			return tcs.Task;
		}

        public Task InsertNavPageBeforeAsync<TViewModel, TViewModelBefore>(
            INavigation navigation, Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel
            where TViewModelBefore : class, IViewModel
        {
            var tcs = new TaskCompletionSource<object>();
            _deviceService.BeginInvokeOnMainThread(() =>
            {
                TViewModel viewModel;
                var view = _viewFactory.Resolve<TViewModel>(out viewModel, setStateAction);
                TViewModelBefore viewModelBefore;
                var viewBefore = _viewFactory.Resolve<TViewModelBefore>(out viewModelBefore);
                this.GetNavigation(navigation).InsertPageBefore(view, viewBefore);
                tcs.SetResult(null);
            });
            return tcs.Task;
        }

        public Task InsertNavPageBeforeAsync<TViewModel, TViewModelBefore>(
            INavigation navigation, TViewModel viewModel, TViewModelBefore viewModelBefore)
            where TViewModel : class, IViewModel
            where TViewModelBefore : class, IViewModel
        {
            var tcs = new TaskCompletionSource<object>();
            _deviceService.BeginInvokeOnMainThread(() =>
            {
                var view = _viewFactory.Resolve<TViewModel>(viewModel);
                var viewBefore = _viewFactory.Resolve<TViewModelBefore>(viewModelBefore);
                this.GetNavigation(navigation).InsertPageBefore(view, viewBefore);
                tcs.SetResult(null);
            });
            return tcs.Task;
        }

        public Task RemoveNavPageAsync<TViewModel>(INavigation navigation)
            where TViewModel : class, IViewModel
        {
            var tcs = new TaskCompletionSource<object>();
            if (this.GetNavigation(navigation) == null)
            {
                tcs.SetResult(null);
            }
            else
            {
                _deviceService.BeginInvokeOnMainThread(() =>
                {
                    TViewModel viewModel;
                    var view = _viewFactory.Resolve<TViewModel>(out viewModel);
                    this.GetNavigation(navigation).RemovePage(view);
                    tcs.SetResult(null);
                });
            }
            return tcs.Task;
        }
    }
}
