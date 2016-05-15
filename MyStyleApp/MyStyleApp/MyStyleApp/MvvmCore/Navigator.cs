using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinFormsAutofacMvvmStarterKit
{
	public class Navigator : INavigator
	{
		private readonly Lazy<INavigation> _navigation;
		private readonly IViewFactory _viewFactory;
		private readonly IDeviceService _deviceService;

		public Navigator(Lazy<INavigation> navigation, IViewFactory viewFactory, IDeviceService deviceService)
		{
			_navigation = navigation;
			_viewFactory = viewFactory;
			_deviceService = deviceService;
		}

		private INavigation Navigation
		{
			get { return _navigation.Value; }
		}

		public Task<IViewModel> PopAsync()
		{
			var tcs = new TaskCompletionSource<IViewModel>();
			_deviceService.BeginInvokeOnMainThread(async () =>
			{
				Page view = await Navigation.PopAsync();
				tcs.SetResult(view.BindingContext as IViewModel);

			});
            return tcs.Task;
		}

		public Task<IViewModel> PopModalAsync()
		{
			var tcs = new TaskCompletionSource<IViewModel>();
			_deviceService.BeginInvokeOnMainThread(async () =>
			{
				Page view = await Navigation.PopAsync();
				tcs.SetResult(view.BindingContext as IViewModel);
			});
            return tcs.Task;
		}

		public Task PopToRootAsync()
		{
			var tcs = new TaskCompletionSource<object>();
			_deviceService.BeginInvokeOnMainThread(async () =>
			{
				await Navigation.PopToRootAsync();
				tcs.SetResult(null);
			});
			return tcs.Task;
		}

		public Task<TViewModel> PushAsync<TViewModel>(Action<TViewModel> setStateAction = null) 
			where TViewModel : class, IViewModel
		{
			var tcs = new TaskCompletionSource<TViewModel>();
			_deviceService.BeginInvokeOnMainThread(async () =>
			{
				TViewModel viewModel;
				var view = _viewFactory.Resolve(out viewModel, setStateAction);
				await Navigation.PushAsync(view);
				tcs.SetResult(viewModel);
			});
			return tcs.Task;
		}

		public Task<TViewModel> PushAsync<TViewModel>(TViewModel viewModel) 
			where TViewModel : class, IViewModel
		{
			var tcs = new TaskCompletionSource<TViewModel>();
			_deviceService.BeginInvokeOnMainThread(async () =>
			{
				var view = _viewFactory.Resolve(viewModel);
				await Navigation.PushAsync(view);
				tcs.SetResult(viewModel);
			});
			return tcs.Task;
		}

		public Task<TViewModel> PushModalAsync<TViewModel>(Action<TViewModel> setStateAction = null) 
			where TViewModel : class, IViewModel
		{
			var tcs = new TaskCompletionSource<TViewModel>();
			_deviceService.BeginInvokeOnMainThread(async () =>
			{
				TViewModel viewModel;
				var view = _viewFactory.Resolve<TViewModel>(out viewModel, setStateAction);
				await Navigation.PushModalAsync(view);
				tcs.SetResult(viewModel);
			});
			return tcs.Task;
		}

		public Task<TViewModel> PushModalAsync<TViewModel>(TViewModel viewModel) 
			where TViewModel : class, IViewModel
		{
			var tcs = new TaskCompletionSource<TViewModel>();
			_deviceService.BeginInvokeOnMainThread(async () =>
			{
				var view = _viewFactory.Resolve(viewModel);
				await Navigation.PushModalAsync(view);
				tcs.SetResult(viewModel);
			});
			return tcs.Task;
		}

        public Task InsertPageBefore<TViewModel, TViewModelBefore>(Action<TViewModel> setStateAction = null)
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
                Navigation.InsertPageBefore(view, viewBefore);
                tcs.SetResult(null);
            });
            return tcs.Task;
        }

        public Task InsertPageBefore<TViewModel, TViewModelBefore>(TViewModel viewModel, TViewModelBefore viewModelBefore)
            where TViewModel : class, IViewModel
            where TViewModelBefore : class, IViewModel
        {
            var tcs = new TaskCompletionSource<object>();
            _deviceService.BeginInvokeOnMainThread(() =>
            {
                var view = _viewFactory.Resolve<TViewModel>(viewModel);
                var viewBefore = _viewFactory.Resolve<TViewModelBefore>(viewModelBefore);
                Navigation.InsertPageBefore(view, viewBefore);
                tcs.SetResult(null);
            });
            return tcs.Task;
        }

        public Task RemovePage<TViewModel>()
            where TViewModel : class, IViewModel
        {
            var tcs = new TaskCompletionSource<object>();
            _deviceService.BeginInvokeOnMainThread(() =>
            {
                TViewModel viewModel;
                var view = _viewFactory.Resolve<TViewModel>(out viewModel);
                Navigation.RemovePage(view);
                tcs.SetResult(null);
            });
            return tcs.Task;
        }
    }
}
