using Autofac;
using XamarinFormsAutofacMvvmStarterKit;
using MyStyleApp.ViewModels;
using MyStyleApp.Views;
using Xamarin.Forms;
using MyStyleApp.Services;
using MyStyleApp.Localization;

namespace MyStyleApp
{
    public class Bootstrapper : CoreAutofacBootstrapper
    {
        private readonly Application _app;

        public Bootstrapper(Application app) : base()
        {
            this._app = app;
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
            builder.RegisterType<LocalizedStringsService>().SingleInstance();
            builder.Register(l => DependencyService.Get<ILocalizationService>()).
                As<ILocalizationService>().SingleInstance();
            builder.Register(c => DependencyService.Get<ICalendarService>()).
                As<ICalendarService>().SingleInstance();
        }

        protected override void RegisterViews(IViewFactory viewFactory)
        {
            base.RegisterViews(viewFactory);

            // Register ViewModel <-> View relations
            viewFactory.Register<LoginViewModel, LoginView>();
            viewFactory.Register<RegisteredStoresViewModel, RegisteredStoresView>();
        }

        protected override void ConfigureApplication(IContainer container)
        {
            var viewFactory = container.Resolve<IViewFactory>();

            // First view to show
            var mainPage = viewFactory.Resolve<LoginViewModel>();
            var navigationPage = new XamarinFormsAutofacMvvmStarterKit.NavigationPage(mainPage);
            this._app.MainPage = navigationPage;
        }
    }
}
