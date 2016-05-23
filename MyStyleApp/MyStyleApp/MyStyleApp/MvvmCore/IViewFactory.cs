using System;
using Xamarin.Forms;

namespace MvvmCore
{
	public interface IViewFactory
	{
		void Register<TViewModel, TView>() 
			where TViewModel : class, IViewModel 
			where TView : Page;

		void Register(Type viewModel, Type view);

        Page Resolve<TViewModel>(Action<TViewModel> setStateAction = null) 
			where TViewModel : class, IViewModel;

        IViewModel ResolveReversed<TView>() where TView : Page;

        Page Resolve<TViewModel>(out TViewModel viewModel, Action<TViewModel> setStateAction = null) 
			where TViewModel : class, IViewModel;

        IViewModel ResolveReversed<TView>(out TView view)
            where TView : Page;

        Page Resolve<TViewModel>(TViewModel viewModel) 
			where TViewModel : class, IViewModel;

        IViewModel ResolveReversed<TView>(TView view)
            where TView : Page;

    }
}

