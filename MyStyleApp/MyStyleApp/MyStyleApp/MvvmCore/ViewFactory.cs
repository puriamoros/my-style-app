using Autofac;
using Autofac.Features.OwnedInstances;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace XamarinFormsAutofacMvvmStarterKit
{
	public class ViewFactory : IViewFactory
	{
		private readonly IDictionary<Type, Type> _map = new Dictionary<Type, Type>();
        private readonly IDictionary<Type, Type> _reversedMap = new Dictionary<Type, Type>();
        private readonly IComponentContext _componentContext;

		public ViewFactory(IComponentContext componentContext)
		{
			_componentContext = componentContext;
			_registry = new Dictionary<IViewModel, Action>();
			Xamarin.Forms.MessagingCenter.Subscribe<XamarinFormsAutofacMvvmStarterKit.NavigationPage, IViewModel>(this
				, "IViewModelPopped"
				, (s, vm) => Dispose(vm));
		}

		public void Register<TViewModel, TView>() 
			where TViewModel : class, IViewModel 
			where TView : Page
		{
			_map[typeof(TViewModel)] = typeof(TView);
            _reversedMap[typeof(TView)] = typeof(TViewModel);
        }

		public void Register(Type viewModel, Type view)
		{
			if (!viewModel.IsAssignableTo<IViewModel>())
			{
				throw new ArgumentException("viewModel must be type of IViewModel");
			}

			if (!view.IsAssignableTo<Page>())
			{
				throw new ArgumentException("view must be type of Page");
			}

			_map[viewModel] = view;
            _reversedMap[view] = viewModel;
        }

		public Page Resolve<TViewModel>(Action<TViewModel> setStateAction = null) where TViewModel : class, IViewModel
		{
			TViewModel viewModel;
			return Resolve<TViewModel>(out viewModel, setStateAction);
		}

        public IViewModel ResolveReversed<TView>() where TView : Page
        {
            TView view;
            return ResolveReversed<TView>(out view);
        }

        public Page Resolve<TViewModel>(out TViewModel viewModel, Action<TViewModel> setStateAction = null) 
			where TViewModel : class, IViewModel 
		{
			var ownedVM = _componentContext.Resolve<Owned<TViewModel>> ();
			viewModel = ownedVM.Value;
            if(!_registry.ContainsKey(viewModel))
            {
                _registry.Add(viewModel, ownedVM.Dispose);
            }

			var viewType = _map[typeof(TViewModel)];
			var view = _componentContext.Resolve(viewType) as Page;

			if (setStateAction != null)
				viewModel.SetState(setStateAction);

            SetBindingContextAndNavigation(view, viewModel);
            return view;
		}

        public IViewModel ResolveReversed<TView>(out TView view)
            where TView : Page
        {
            var ownedView = _componentContext.Resolve<Owned<TView>>();
            view = ownedView.Value;

            var vmType = _reversedMap[typeof(TView)];
            var viewModel = _componentContext.Resolve(vmType) as IViewModel;

            SetBindingContextAndNavigation(view, viewModel);
            return viewModel;
        }

        public Page Resolve<TViewModel>(TViewModel viewModel) 
			where TViewModel : class, IViewModel 
		{
			var viewType = _map[viewModel.GetType()];
			var view = _componentContext.Resolve(viewType) as Page;
            SetBindingContextAndNavigation(view, viewModel);
            return view;
		}

        public IViewModel ResolveReversed<TView>(TView view)
            where TView : Page
        {
            var vmType = _reversedMap[view.GetType()];
            var viewModel = _componentContext.Resolve(vmType) as IViewModel;
            SetBindingContextAndNavigation(view, viewModel);
            return viewModel;
        }

        private void SetBindingContextAndNavigation<TView, TViewModel>(TView view, TViewModel viewModel)
            where TView : Page
            where TViewModel : class, IViewModel
        {
            // Set binding for current page
            if(view != null && viewModel != null)
            {
                view.BindingContext = viewModel;
                viewModel.Navigation = view.Navigation;
            }

            // Set binding for children pages
            if(view != null)
            {
                IList<Page> children = null;
                if (view is TabbedPage)
                {
                    var tabbedPage = (view as TabbedPage);
                    children = tabbedPage.Children;
                }
                else if (view is MasterDetailPage)
                {
                    var mdPage = (view as MasterDetailPage);
                    Page[] pages = { mdPage.Master, mdPage.Detail };
                    children = new List<Page>(pages);
                }
                else if (view is CarouselPage)
                {
                    var cPage = (view as CarouselPage);
                    children = new List<Page>(cPage.Children.Count);
                    foreach (var page in cPage.Children)
                    {
                        children.Add(page);
                    }
                }
                else if (view is NavigationPage)
                {
                    var navPage = (view as NavigationPage);
                    children = new List<Page>(1);
                    // TODO: how to get the child page?!?!?!
                    children.Add(navPage.CurrentPage);
                }

                if (children != null)
                {
                    foreach (var childView in children)
                    {
                        IViewModel childViewModel = null;
                        if (_reversedMap.ContainsKey(childView.GetType()))
                        {
                            var childVmType = _reversedMap[childView.GetType()];
                            childViewModel = _componentContext.Resolve(childVmType) as IViewModel;
                            
                        }
                        SetBindingContextAndNavigation(childView, childViewModel);
                    }
                }
            }
        }

        #region View Model Scope

        private readonly Dictionary<IViewModel, Action> _registry;

		private void Dispose(IViewModel viewModel)
		{
			var owned = _registry[viewModel];
			if (owned != null)
			{
				owned.Invoke(); ;
				_registry.Remove(viewModel);
			}
		}

		#endregion
	}
}

