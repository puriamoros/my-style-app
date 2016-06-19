using Autofac;
using MvvmCore;
using MyStyleApp.ViewModels;
using MyStyleApp.Views;
using Xamarin.Forms;
using MyStyleApp.Services;
using MyStyleApp.Services.Backend;
using MyStyleApp.Services.Backend.Mocks;

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
            builder.RegisterType<AppointmentsView>().SingleInstance();
            builder.RegisterType<FavouritesView>().SingleInstance();
            builder.RegisterType<SearchView>().SingleInstance();
            builder.RegisterType<AccountView>().SingleInstance();
            builder.RegisterType<EstablishmentsView>().SingleInstance();
            builder.RegisterType<EstablishmentDetailsView>().SingleInstance();
            builder.RegisterType<BookView>().SingleInstance();

            // Register ViewModels
            builder.RegisterType<StartViewModel>().SingleInstance();
            builder.RegisterType<LoginViewModel>().SingleInstance();
            builder.RegisterType<MainViewModel>().SingleInstance();
            builder.RegisterType<AppointmentsViewModel>().SingleInstance();
            builder.RegisterType<FavouritesViewModel>().SingleInstance();
            builder.RegisterType<SearchViewModel>().SingleInstance();
            builder.RegisterType<AccountViewModel>().SingleInstance();
            builder.RegisterType<EstablishmentsViewModel>().SingleInstance();
            builder.RegisterType<EstablishmentDetailsViewModel>().SingleInstance();
            builder.RegisterType<BookViewModel>().SingleInstance();

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
            builder.RegisterType<ValidationService>();
            builder.RegisterType<ProvincesService>().SingleInstance();
            builder.RegisterType<EstablishmentTypesService>().SingleInstance();
            builder.RegisterType<ServiceCategoriesService>().As<IServiceCategoriesService>().SingleInstance();
            builder.RegisterType<ServicesService>().As<IServicesService>().SingleInstance();
            builder.RegisterType<EstablishmentsService>().As<IEstablishmentsService>().SingleInstance();
            builder.RegisterType<FavouritesService>().As<IFavouritesService>().SingleInstance();
        }

        protected override void RegisterViews(IViewFactory viewFactory)
        {
            base.RegisterViews(viewFactory);

            // Register ViewModel <-> View relations
            viewFactory.Register<StartViewModel, StartView>();
            viewFactory.Register<LoginViewModel, LoginView>();
            viewFactory.Register<MainViewModel, MainView>();
            viewFactory.Register<AppointmentsViewModel, AppointmentsView>();
            viewFactory.Register<FavouritesViewModel, FavouritesView>();
            viewFactory.Register<SearchViewModel, SearchView>();
            viewFactory.Register<AccountViewModel, AccountView>();
            viewFactory.Register<EstablishmentsViewModel, EstablishmentsView>();
            viewFactory.Register<EstablishmentDetailsViewModel, EstablishmentDetailsView>();
            viewFactory.Register<BookViewModel, BookView>();
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
