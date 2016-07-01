using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MvvmCore
{
    class AppFlowElement
    {
        public string ParentName { get; set; }
        public string Name { get; set; }
        public Page Page { get; set; }
    }

    public class AppFlowController
    {
        private readonly IDeviceService _deviceService;
        private readonly IViewFactory _viewFactory;
        private readonly ViewContainerFactory _containerFactory;
        private Dictionary<string, AppFlowElement> _elements;

        public AppFlowController(IViewFactory viewFactory, IDeviceService deviceService, ViewContainerFactory containerFactory)
        {
            this._viewFactory = viewFactory;
            this._deviceService = deviceService;
            this._containerFactory = containerFactory;
            this._elements = new Dictionary<string, AppFlowElement>();
        }

        private string GetElementName(object obj)
        {
            return obj.GetType().ToString() + obj.GetHashCode();
        }

        private AppFlowElement CreateElement(string parentName, string name, ContainerTypeEnum containerType)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Name can not be null or empty");
            }

            if(this._elements.ContainsKey(name))
            {
                return this._elements[name];
            }
            else
            {
                AppFlowElement element = new AppFlowElement();
                element.ParentName = parentName;
                element.Name = name;
                element.Page = this._containerFactory.GetContainer(containerType);
                return element;
            }
        }

        private AppFlowElement CreateElement<TViewModel>(string parentName, Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel
        {
            TViewModel viewModel;
            Page page = this._viewFactory.Resolve<TViewModel>(out viewModel, setStateAction);
            string name = GetElementName(viewModel);

            if (this._elements.ContainsKey(name))
            {
                return this._elements[name];
            }
            else
            {
                AppFlowElement element = new AppFlowElement();
                element.ParentName = parentName;
                element.Name = name;
                element.Page = page;
                return element;
            }
        }

        private AppFlowElement GetElement(string name)
        {
            if (!this._elements.ContainsKey(name))
            {
                throw new KeyNotFoundException(string.Format("Element with name '{0}' not found", name));
            }

            return this._elements[name];
        }

        private AppFlowElement GetElement<TViewModel>()
            where TViewModel : class, IViewModel
        {
            TViewModel viewModel;
            var page = this._viewFactory.Resolve<TViewModel>(out viewModel);
            var name = GetElementName(viewModel);

            if (!this._elements.ContainsKey(name))
            {
                throw new KeyNotFoundException(string.Format("Element with name '{0}' not found", name));
            }

            return this._elements[name];
        }

        private async Task InsertElementIntoContainer(string containerName, AppFlowElement element, int index = -1)
        {
            if(!this._elements.ContainsKey(containerName))
            {
                throw new KeyNotFoundException(string.Format("Element with name '{0}' not found", containerName));
            }
            AppFlowElement container = this._elements[containerName];
            if(container.Page is MasterDetailPage)
            {
                MasterDetailPage page = container.Page as MasterDetailPage;
                if (index != 0 && index != 1)
                {
                    throw new IndexOutOfRangeException(string.Format("Index '{0}' is not valid for a MasterDetailPage. Only '0' and '1' are valid index values for a MasterDetailPage", index));
                }

                if(index == 0)
                {
                    page.Master = element.Page;
                }
                else
                {
                    page.Detail = element.Page;
                }
            }
            else if (container.Page is NavigationPage)
            {
                NavigationPage page = container.Page as NavigationPage;
                if (index < -1 || index > page.Navigation.NavigationStack.Count)
                {
                    throw new IndexOutOfRangeException(string.Format("Index '{0}' is out of NavigationStack's bounds", index));
                }

                if(index == -1 || index == page.Navigation.NavigationStack.Count)
                {
                    await page.Navigation.PushAsync(element.Page);
                }
                else
                {
                    Page before = page.Navigation.NavigationStack[index];
                    page.Navigation.InsertPageBefore(element.Page, before);
                }
            }
            else if (container.Page is MultiPage<Page>) // Tabbed or Carousel
            {
                MultiPage<Page> page = container.Page as MultiPage<Page>;
                if (index < -1 || index > page.Children.Count)
                {
                    throw new IndexOutOfRangeException(string.Format("Index '{0}' is out of Children's bounds", index));
                }

                if (index == -1 || index == page.Navigation.NavigationStack.Count)
                {
                    page.Children.Add(element.Page);
                }
                else
                {
                    page.Children.Insert(index, element.Page);
                }
            }
            else
            {
                throw new InvalidOperationException(string.Format("Container type '{0}' not supported", container.Page.GetType().ToString()));
            }
        }

        private Task InsertElement(string parentContainerName, int index, AppFlowElement element)
        {
            this._elements.Add(element.Name, element);
            var tcs = new TaskCompletionSource<object>();
            _deviceService.BeginInvokeOnMainThread(async () =>
            {
                if (parentContainerName == null)
                {
                    Application.Current.MainPage = element.Page;
                }
                else
                {
                    await this.InsertElementIntoContainer(parentContainerName, element, index);
                }
                tcs.SetResult(null);
            });

            return tcs.Task;
        }

        private async Task InsertModalElementIntoContainer(string containerName, AppFlowElement element)
        {
            if (!this._elements.ContainsKey(containerName))
            {
                throw new KeyNotFoundException(string.Format("Element with name '{0}' not found", containerName));
            }
            AppFlowElement container = this._elements[containerName];
            if (container.Page is MasterDetailPage)
            {
                throw new InvalidOperationException(string.Format("Element '{0}' is inside a MasterDetailPage. Use a navigation Page.", element.Name));
            }
            else if (container.Page is NavigationPage)
            {
                NavigationPage page = container.Page as NavigationPage;
                await page.Navigation.PushModalAsync(element.Page);
            }
            else if (container.Page is MultiPage<Page>) // Tabbed or Carousel
            {
                throw new InvalidOperationException(string.Format("Element '{0}' is inside a TabbedPage or Carousel. Use a NavigationPage.", element.Name));
            }
            else
            {
                throw new InvalidOperationException(string.Format("Container type '{0}' not supported", container.Page.GetType().ToString()));
            }
        }

        private Task InsertModalElement(string parentContainerName, AppFlowElement element)
        {
            this._elements.Add(element.Name, element);
            var tcs = new TaskCompletionSource<object>();
            _deviceService.BeginInvokeOnMainThread(async () =>
            {
                if (parentContainerName == null)
                {
                    Application.Current.MainPage = element.Page;
                }
                else
                {
                    await this.InsertModalElement(parentContainerName, element);
                }
                tcs.SetResult(null);
            });

            return tcs.Task;
        }

        private async Task RemoveElementFromContainer(String containerName, AppFlowElement element)
        {
            if (!this._elements.ContainsKey(containerName))
            {
                throw new KeyNotFoundException(string.Format("Element with name '{0}' not found", containerName));
            }

            AppFlowElement container = this._elements[containerName];
            if (container.Page is MasterDetailPage)
            {
                MasterDetailPage page = container.Page as MasterDetailPage;
                if (page.Master == element.Page)
                {
                    page.Master = null;
                }
                else if (page.Detail == element.Page)
                {
                    page.Detail = null;
                }
            }
            else if (container.Page is NavigationPage)
            {
                NavigationPage page = container.Page as NavigationPage;

                if (element.Page == page.Navigation.NavigationStack[page.Navigation.NavigationStack.Count-1])
                {
                    await page.Navigation.PopAsync();
                }
                else
                {
                    page.Navigation.RemovePage(element.Page);
                }
            }
            else if (container.Page is MultiPage<Page>) // Tabbed or Carousel
            {
                MultiPage<Page> page = container.Page as MultiPage<Page>;
                page.Children.Remove(element.Page);
            }
            else
            {
                throw new InvalidOperationException(string.Format("Container type '{0}' not supported", container.Page.GetType().ToString()));
            }
        }

        private Task RemoveElement(AppFlowElement element)
        {
            //this._elements.Remove(element.Name);

            var tcs = new TaskCompletionSource<object>();
            _deviceService.BeginInvokeOnMainThread(async () =>
            {
                if (element.ParentName == null)
                {
                    Application.Current.MainPage = null;
                }
                else
                {
                    await this.RemoveElementFromContainer(element.ParentName, element);
                }
                tcs.SetResult(null);
            });

            return tcs.Task;
        }

        private async Task RemoveModalElementFromContainer(String containerName, AppFlowElement element)
        {
            if (!this._elements.ContainsKey(containerName))
            {
                throw new KeyNotFoundException(string.Format("Element with name '{0}' not found", containerName));
            }

            AppFlowElement container = this._elements[containerName];
            if (container.Page is MasterDetailPage)
            {
                throw new InvalidOperationException(string.Format("Element '{0}' is inside a MasterDetailPage. Use a navigation Page.", element.Name));
            }
            else if (container.Page is NavigationPage)
            {
                NavigationPage page = container.Page as NavigationPage;
                if (element.Page == page.Navigation.ModalStack[page.Navigation.NavigationStack.Count - 1])
                {
                    await page.Navigation.PopModalAsync();
                }
                else
                {
                    throw new ArgumentException(string.Format("Element '{0}' is not in the top of the ModalStack.", element.Name));
                }
            }
            else if (container.Page is MultiPage<Page>) // Tabbed or Carousel
            {
                throw new InvalidOperationException(string.Format("Element '{0}' is inside a TabbedPage or CarouselPage. Use a navigation Page.", element.Name));
            }
            else
            {
                throw new InvalidOperationException(string.Format("Container type '{0}' not supported", container.Page.GetType().ToString()));
            }
        }

        private Task RemoveModalElement(AppFlowElement element)
        {
            //this._elements.Remove(element.Name);

            var tcs = new TaskCompletionSource<object>();
            _deviceService.BeginInvokeOnMainThread(async () =>
            {
                if (element.ParentName == null)
                {
                    Application.Current.MainPage = null;
                }
                else
                {
                    await this.RemoveModalElementFromContainer(element.ParentName, element);
                }
                tcs.SetResult(null);
            });

            return tcs.Task;
        }

        private void SetCurrentElementInContainer(String containerName, AppFlowElement element)
        {
            if (!this._elements.ContainsKey(containerName))
            {
                throw new KeyNotFoundException(string.Format("Element with name '{0}' not found", containerName));
            }

            AppFlowElement container = this._elements[containerName];
            if (container.Page is MasterDetailPage)
            {
                throw new InvalidOperationException(string.Format("Element '{0}' is inside a MasterDetailPage", element.Name));
            }
            else if (container.Page is NavigationPage)
            {
                throw new InvalidOperationException(string.Format("Element '{0}' is inside a NavigationPage", element.Name));
            }
            else if (container.Page is MultiPage<Page>) // Tabbed or Carousel
            {
                MultiPage<Page> page = container.Page as MultiPage<Page>;
                page.CurrentPage = element.Page;
            }
            else
            {
                throw new InvalidOperationException(string.Format("Container type '{0}' not supported", container.Page.GetType().ToString()));
            }
        }

        private Task SetCurrentElement(AppFlowElement element)
        {
            var tcs = new TaskCompletionSource<object>();
            _deviceService.BeginInvokeOnMainThread(() =>
            {
                if (element.ParentName == null)
                {
                    throw new InvalidOperationException(string.Format("Element '{0}' is currently the main element"));
                }
                else
                {
                    this.SetCurrentElementInContainer(element.ParentName, element);
                }
                tcs.SetResult(null);
            });

            return tcs.Task;
        }

        private Task ClearElementsInContainer(String containerName)
        {
            var tcs = new TaskCompletionSource<object>();
            _deviceService.BeginInvokeOnMainThread(async () =>
            {
                if (!this._elements.ContainsKey(containerName))
                {
                    throw new KeyNotFoundException(string.Format("Element with name '{0}' not found", containerName));
                }

                AppFlowElement container = this._elements[containerName];
                if (container.Page is MasterDetailPage)
                {
                    MasterDetailPage page = container.Page as MasterDetailPage;
                    page.Master = null;
                    page.Detail = null;
                }
                else if (container.Page is NavigationPage)
                {
                    NavigationPage page = container.Page as NavigationPage;

                    // WinPhone hack: navigation PopToRootAsync is not working as
                    // spected on WinPhone, so I have to use a workaround
                    if (_deviceService.OS == TargetPlatform.Windows ||
                        _deviceService.OS == TargetPlatform.WinPhone)
                    {
                        INavigation rootNavigation = page.Navigation.NavigationStack[0].Navigation;
                        while (rootNavigation.NavigationStack.Count > 1)
                        {
                            await rootNavigation.PopAsync();
                        }
                    }
                    else
                    {
                        await page.Navigation.PopToRootAsync();
                    }
                }
                else if (container.Page is MultiPage<Page>) // Tabbed or Carousel
                {
                    MultiPage<Page> page = container.Page as MultiPage<Page>;
                    page.Children.Clear();
                }
                else
                {
                    throw new InvalidOperationException(string.Format("Container type '{0}' not supported", container.Page.GetType().ToString()));
                }

                tcs.SetResult(null);
            });

            return tcs.Task;
        }

        public Task AddContainer(string parentContainerName, string name, ContainerTypeEnum containerType)
        {
            return this.InsertContainer(parentContainerName, -1, name, containerType);
        }

        public Task InsertContainer(string parentContainerName, int index, string name, ContainerTypeEnum containerType)
        {
            AppFlowElement element = this.CreateElement(parentContainerName, name, containerType);
            return this.InsertElement(parentContainerName, index, element);
        }

        public Task AddViewModel<TViewModel>(string parentContainerName, Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel
        {
            return this.InsertViewModel<TViewModel>(parentContainerName, -1, setStateAction);
        }

        public Task InsertViewModel<TViewModel>(string parentContainerName, int index, Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel
        {
            AppFlowElement element = this.CreateElement<TViewModel>(parentContainerName, setStateAction);
            return this.InsertElement(parentContainerName, index, element);
        }

        public Task AddViewModelModal<TViewModel>(string parentContainerName, string name, Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel
        {
            AppFlowElement element = this.CreateElement<TViewModel>(parentContainerName, setStateAction);
            return this.InsertModalElement(parentContainerName, element);
        }

        public Task RemoveViewModelModal<TViewModel>(string parentContainerName, string name, Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel
        {
            AppFlowElement element = this.CreateElement<TViewModel>(parentContainerName, setStateAction);
            return this.RemoveModalElement(element);
        }

        public Task RemoveContainer(string name)
        {
            AppFlowElement element = this.GetElement(name);
            return this.RemoveElement(element);
        }

        public Task RemoveViewModel<TViewModel>()
            where TViewModel : class, IViewModel
        {
            AppFlowElement element = this.GetElement<TViewModel>();
            return this.RemoveElement(element);
        }

        public Task SetCurrentContainer(string name)
        {
            AppFlowElement element = this.GetElement(name);
            return this.SetCurrentElement(element);
        }

        public Task SetCurrentViewModel<TViewModel>()
            where TViewModel : class, IViewModel
        {
            AppFlowElement element = this.GetElement<TViewModel>();
            return this.SetCurrentElement(element);
        }

        public Task ClearContainer(string name)
        {
            return this.ClearElementsInContainer(name);
        }
    }
}
