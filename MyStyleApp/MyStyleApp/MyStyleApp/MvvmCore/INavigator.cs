using System;
using System.Threading.Tasks;

namespace XamarinFormsAutofacMvvmStarterKit
{
	public interface INavigator
	{
		Task<IViewModel> PopAsync();

		Task<IViewModel> PopModalAsync();

		Task PopToRootAsync();

		Task<TViewModel> PushAsync<TViewModel>(Action<TViewModel> setStateAction = null) 
			where TViewModel : class, IViewModel;

		Task<TViewModel> PushAsync<TViewModel>(TViewModel viewModel) 
			where TViewModel : class, IViewModel;

		Task<TViewModel> PushModalAsync<TViewModel>(Action<TViewModel> setStateAction = null) 
			where TViewModel : class, IViewModel;

		Task<TViewModel> PushModalAsync<TViewModel>(TViewModel viewModel) 
			where TViewModel : class, IViewModel;

        Task InsertPageBefore<TViewModel, TViewModelBefore>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel
            where TViewModelBefore : class, IViewModel;

        Task InsertPageBefore<TViewModel, TViewModelBefore>(TViewModel viewModel, TViewModelBefore viewModelBefore)
            where TViewModel : class, IViewModel
            where TViewModelBefore : class, IViewModel;

        Task RemovePage<TViewModel>()
            where TViewModel : class, IViewModel;
    }
}

