using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MvvmCore
{
	public interface INavigator
	{
        Task<TViewModel> SetMainPage<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel;

        Task<TViewModel> SetMainPage<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel;

        Task<IViewModel> PopAsync(INavigation navigation);

		Task<IViewModel> PopModalAsync(INavigation navigation);

		Task PopToRootAsync(INavigation navigation);

		Task<TViewModel> PushAsync<TViewModel>(
            INavigation navigation, Action<TViewModel> setStateAction = null) 
			where TViewModel : class, IViewModel;

		Task<TViewModel> PushAsync<TViewModel>(
            INavigation navigation, TViewModel viewModel) 
			where TViewModel : class, IViewModel;

		Task<TViewModel> PushModalAsync<TViewModel>(
            INavigation navigation, Action<TViewModel> setStateAction = null) 
			where TViewModel : class, IViewModel;

		Task<TViewModel> PushModalAsync<TViewModel>(
            INavigation navigation, TViewModel viewModel) 
			where TViewModel : class, IViewModel;

        Task InsertPageBefore<TViewModel, TViewModelBefore>(
            INavigation navigation, Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel
            where TViewModelBefore : class, IViewModel;

        Task InsertPageBefore<TViewModel, TViewModelBefore>(
            INavigation navigation, TViewModel viewModel, TViewModelBefore viewModelBefore)
            where TViewModel : class, IViewModel
            where TViewModelBefore : class, IViewModel;

        Task RemovePage<TViewModel>(INavigation navigation)
            where TViewModel : class, IViewModel;
    }
}

