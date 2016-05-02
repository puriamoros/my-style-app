using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using XamarinFormsAutofacMvvmStarterKit;
using MyStyleApp.ViewModels;
using MyStyleApp.Views;

namespace MyStyleApp
{
    public class Bootstrapper : CoreAutofacBootstrapper
    {
        public Bootstrapper(Xamarin.Forms.Application app) : base()
        {
            App = app;
        }

        protected override void ConfigureApplication(IContainer container)
        {
            var viewFactory = container.Resolve<IViewFactory>();
            var mainPage = viewFactory.Resolve<LoginViewModel>();
            var navigationPage = new NavigationPage(mainPage);
            App.MainPage = navigationPage;
        }

        protected override void ConfigureContainer(ContainerBuilder builder)
        {
            base.ConfigureContainer(builder);

            // Register Views
            builder.RegisterType<LoginView>().SingleInstance();
            builder.RegisterType<RegisteredStoresView>().SingleInstance();

            // Register ViewModels
            builder.RegisterType<LoginViewModel>().SingleInstance();
            builder.RegisterType<RegisteredStoresViewModel>().SingleInstance();

            // Register Services
        }

        protected override void RegisterViews(IViewFactory viewFactory)
        {
            base.RegisterViews(viewFactory);

            // Register ViewModel <-> View relations
            viewFactory.Register<LoginViewModel, LoginView>();
            viewFactory.Register<RegisteredStoresViewModel, RegisteredStoresView>();
        }

        private readonly Xamarin.Forms.Application App;
    }
}
