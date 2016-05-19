using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinFormsAutofacMvvmStarterKit
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

        public Task<TViewModel> SetMainPage<TViewModel>(Action<TViewModel> setStateAction = null)
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

        public Task<TViewModel> SetMainPage<TViewModel>(TViewModel viewModel)
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

        public Task<IViewModel> PopAsync(INavigation navigation)
		{
			var tcs = new TaskCompletionSource<IViewModel>();
			_deviceService.BeginInvokeOnMainThread(async () =>
			{
				Page view = await navigation.PopAsync();
				tcs.SetResult(view.BindingContext as IViewModel);

			});
            return tcs.Task;
		}

		public Task<IViewModel> PopModalAsync(INavigation navigation)
		{
			var tcs = new TaskCompletionSource<IViewModel>();
			_deviceService.BeginInvokeOnMainThread(async () =>
			{
				Page view = await navigation.PopAsync();
				tcs.SetResult(view.BindingContext as IViewModel);
			});
            return tcs.Task;
		}

		public Task PopToRootAsync(INavigation navigation)
		{
			var tcs = new TaskCompletionSource<object>();
			_deviceService.BeginInvokeOnMainThread(async () =>
			{
				await navigation.PopToRootAsync();
				tcs.SetResult(null);
			});
			return tcs.Task;
		}

		public Task<TViewModel> PushAsync<TViewModel>(
            INavigation navigation, Action<TViewModel> setStateAction = null) 
			where TViewModel : class, IViewModel
		{
			var tcs = new TaskCompletionSource<TViewModel>();
			_deviceService.BeginInvokeOnMainThread(async () =>
			{
				TViewModel viewModel;
				var view = _viewFactory.Resolve(out viewModel, setStateAction);
				await navigation.PushAsync(view);
				tcs.SetResult(viewModel);
			});
			return tcs.Task;
		}

		public Task<TViewModel> PushAsync<TViewModel>(
            INavigation navigation, TViewModel viewModel) 
			where TViewModel : class, IViewModel
		{
			var tcs = new TaskCompletionSource<TViewModel>();
			_deviceService.BeginInvokeOnMainThread(async () =>
			{
				var view = _viewFactory.Resolve(viewModel);
				await navigation.PushAsync(view);
				tcs.SetResult(viewModel);
			});
			return tcs.Task;
		}

		public Task<TViewModel> PushModalAsync<TViewModel>(
            INavigation navigation, Action<TViewModel> setStateAction = null) 
			where TViewModel : class, IViewModel
		{
			var tcs = new TaskCompletionSource<TViewModel>();
			_deviceService.BeginInvokeOnMainThread(async () =>
			{
				TViewModel viewModel;
				var view = _viewFactory.Resolve<TViewModel>(out viewModel, setStateAction);
				await navigation.PushModalAsync(view);
				tcs.SetResult(viewModel);
			});
			return tcs.Task;
		}

		public Task<TViewModel> PushModalAsync<TViewModel>(
            INavigation navigation, TViewModel viewModel) 
			where TViewModel : class, IViewModel
		{
			var tcs = new TaskCompletionSource<TViewModel>();
			_deviceService.BeginInvokeOnMainThread(async () =>
			{
				var view = _viewFactory.Resolve(viewModel);
				await navigation.PushModalAsync(view);
				tcs.SetResult(viewModel);
			});
			return tcs.Task;
		}

        public Task InsertPageBefore<TViewModel, TViewModelBefore>(
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
                navigation.InsertPageBefore(view, viewBefore);
                tcs.SetResult(null);
            });
            return tcs.Task;
        }

        public Task InsertPageBefore<TViewModel, TViewModelBefore>(
            INavigation navigation, TViewModel viewModel, TViewModelBefore viewModelBefore)
            where TViewModel : class, IViewModel
            where TViewModelBefore : class, IViewModel
        {
            var tcs = new TaskCompletionSource<object>();
            _deviceService.BeginInvokeOnMainThread(() =>
            {
                var view = _viewFactory.Resolve<TViewModel>(viewModel);
                var viewBefore = _viewFactory.Resolve<TViewModelBefore>(viewModelBefore);
                navigation.InsertPageBefore(view, viewBefore);
                tcs.SetResult(null);
            });
            return tcs.Task;
        }

        public Task RemovePage<TViewModel>(INavigation navigation)
            where TViewModel : class, IViewModel
        {
            var tcs = new TaskCompletionSource<object>();
            _deviceService.BeginInvokeOnMainThread(() =>
            {
                TViewModel viewModel;
                var view = _viewFactory.Resolve<TViewModel>(out viewModel);
                navigation.RemovePage(view);
                tcs.SetResult(null);
            });
            return tcs.Task;
        }
    }
}
