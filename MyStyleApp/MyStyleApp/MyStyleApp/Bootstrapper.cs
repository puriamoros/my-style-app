using Autofac;
using XamarinFormsAutofacMvvmStarterKit;
using MyStyleApp.ViewModels;
using MyStyleApp.Views;
using Xamarin.Forms;
using MyStyleApp.Services;
using MyStyleApp.Services.Backend;

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
            builder.RegisterType<StartView>().SingleInstance();
            builder.RegisterType<LoginView>().SingleInstance();
            builder.RegisterType<MainView>().SingleInstance();

            // Register ViewModels
            builder.RegisterType<StartViewModel>().SingleInstance();
            builder.RegisterType<LoginViewModel>().SingleInstance();
            builder.RegisterType<MainViewModel>().SingleInstance();

            // Register Services
            builder.RegisterType<LocalizedStringsService>().SingleInstance();
            builder.Register(l => DependencyService.Get<ILocalizationService>()).
                As<ILocalizationService>().SingleInstance();
            builder.Register(c => DependencyService.Get<ICalendarService>()).
                As<ICalendarService>().SingleInstance();
            builder.Register(f => DependencyService.Get<IFileStorageService>()).
                As<IFileStorageService>().SingleInstance();
            builder.RegisterGeneric(typeof(ObjectStorageService<>));
            builder.RegisterType<HttpService>().SingleInstance();
            builder.RegisterType<UsersService>().As<IUsersService>().SingleInstance();
        }

        protected override void RegisterViews(IViewFactory viewFactory)
        {
            base.RegisterViews(viewFactory);

            // Register ViewModel <-> View relations
            viewFactory.Register<StartViewModel, StartView>();
            viewFactory.Register<LoginViewModel, LoginView>();
            viewFactory.Register<MainViewModel, MainView>();
        }

        protected override void ConfigureApplication(IContainer container)
        {
            // First view to show
            var viewFactory = container.Resolve<IViewFactory>();
            var mainPage = viewFactory.Resolve<StartViewModel>();
            this._app.MainPage = mainPage;
        }
    }
}
