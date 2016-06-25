using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MvvmCore
{
	public interface INavigator
	{
        Task<TViewModel> SetMainPageAsync<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel;

        Task<TViewModel> SetMainPageAsync<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel;

        Task<TViewModel> SetMainPageNavPageAsync<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel;

        Task<TViewModel> SetMainPageNavPageAsync<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel;

        Task<TViewModel> SetMainPageTabAsync<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel;

        Task<TViewModel> SetMainPageTabAsync<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel;

        Task<TViewModel> SetNavPageTabAsync<TViewModel>(INavigation navigation, Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel;

        Task<TViewModel> SetNavPageTabAsync<TViewModel>(INavigation navigation, TViewModel viewModel)
            where TViewModel : class, IViewModel;

        Task<IViewModel> PopNavPageAsync(INavigation navigation);

		Task<IViewModel> PopNavPageModalAsync(INavigation navigation);

		Task PopNavPageToRootAsync(INavigation navigation);

		Task<TViewModel> PushNavPageAsync<TViewModel>(
            INavigation navigation, Action<TViewModel> setStateAction = null) 
			where TViewModel : class, IViewModel;

		Task<TViewModel> PushNavPageAsync<TViewModel>(
            INavigation navigation, TViewModel viewModel) 
			where TViewModel : class, IViewModel;

		Task<TViewModel> PushNavPageModalAsync<TViewModel>(
            INavigation navigation, Action<TViewModel> setStateAction = null) 
			where TViewModel : class, IViewModel;

		Task<TViewModel> PushNavPageModalAsync<TViewModel>(
            INavigation navigation, TViewModel viewModel) 
			where TViewModel : class, IViewModel;

        Task InsertNavPageBeforeAsync<TViewModel, TViewModelBefore>(
            INavigation navigation, Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel
            where TViewModelBefore : class, IViewModel;

        Task InsertNavPageBeforeAsync<TViewModel, TViewModelBefore>(
            INavigation navigation, TViewModel viewModel, TViewModelBefore viewModelBefore)
            where TViewModel : class, IViewModel
            where TViewModelBefore : class, IViewModel;

        Task RemoveNavPageAsync<TViewModel>(INavigation navigation)
            where TViewModel : class, IViewModel;
    }
}

