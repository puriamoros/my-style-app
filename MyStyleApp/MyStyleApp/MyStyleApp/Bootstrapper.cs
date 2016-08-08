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
            builder.RegisterType<MainClientView>().SingleInstance();
            builder.RegisterType<ClientAppointmentsView>().SingleInstance();
            builder.RegisterType<FavouritesView>().SingleInstance();
            builder.RegisterType<EstablishmentSearchView>().SingleInstance();
            builder.RegisterType<EstablishmentSearchResultsView>().SingleInstance();
            builder.RegisterType<EstablishmentDetailsView>().SingleInstance();
            builder.RegisterType<BookView>().SingleInstance();
            builder.RegisterType<ErrorView>().SingleInstance();
            builder.RegisterType<ChangePasswordView>().SingleInstance();
            builder.RegisterType<AccountDetailsView>().SingleInstance();
            builder.RegisterType<CreateAccountView>().SingleInstance();
            builder.RegisterType<OwnerEstablishmentsView>().SingleInstance();
            builder.RegisterType<EstablishmentAppointmentsView>().SingleInstance();
            builder.RegisterType<EstablishmentStaffView>().SingleInstance();
            builder.RegisterType<MainOwnerView>().SingleInstance();
            builder.RegisterType<MapView>().SingleInstance();
            builder.RegisterType<StaffAccountDetailsView>().SingleInstance();
            builder.RegisterType<EstablishmentServicesView>().SingleInstance();
            builder.RegisterType<CreateStaffAccountView>().SingleInstance();
            builder.RegisterType<AppointmentDetailsView>().SingleInstance();
            builder.RegisterType<ClientHistoryView>().SingleInstance();
            builder.RegisterType<CreateEstablishmentView>().SingleInstance();
            builder.RegisterType<OwnerEstablishmentDetailsView>().SingleInstance();
            builder.RegisterType<MainStaffView>().SingleInstance();
            builder.RegisterType<InformationView>().SingleInstance();

            // Register ViewModels
            builder.RegisterType<StartViewModel>().SingleInstance();
            builder.RegisterType<LoginViewModel>().SingleInstance();
            builder.RegisterType<MainClientViewModel>().SingleInstance();
            builder.RegisterType<ClientAppointmentsViewModel>().SingleInstance();
            builder.RegisterType<FavouritesViewModel>().SingleInstance();
            builder.RegisterType<EstablishmentSearchViewModel>().SingleInstance();
            builder.RegisterType<EstablishmentSearchResultsViewModel>().SingleInstance();
            builder.RegisterType<EstablishmentDetailsViewModel>().SingleInstance();
            builder.RegisterType<BookViewModel>().SingleInstance();
            builder.RegisterType<ErrorViewModel>().SingleInstance();
            builder.RegisterType<ChangePasswordViewModel>().SingleInstance();
            builder.RegisterType<AccountDetailsViewModel>().SingleInstance();
            builder.RegisterType<CreateAccountViewModel>().SingleInstance();
            builder.RegisterType<OwnerEstablishmentsViewModel>().SingleInstance();
            builder.RegisterType<EstablishmentAppointmentsViewModel>().SingleInstance();
            builder.RegisterType<EstablishmentStaffViewModel>().SingleInstance();
            builder.RegisterType<MainOwnerViewModel>().SingleInstance();
            builder.RegisterType<MapViewModel>().SingleInstance();
            builder.RegisterType<StaffAccountDetailsViewModel>().SingleInstance();
            builder.RegisterType<EstablishmentServicesViewModel>().SingleInstance();
            builder.RegisterType<CreateStaffAccountViewModel>().SingleInstance();
            builder.RegisterType<AppointmentDetailsViewModel>().SingleInstance();
            builder.RegisterType<ClientHistoryViewModel>().SingleInstance();
            builder.RegisterType<CreateEstablishmentViewModel>().SingleInstance();
            builder.RegisterType<OwnerEstablishmentDetailsViewModel>().SingleInstance();
            builder.RegisterType<MainStaffViewModel>().SingleInstance();
            builder.RegisterType<InformationViewModel>().SingleInstance();

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
            builder.RegisterType<AppointmentsService>().As<IAppointmentsService>().SingleInstance();
            builder.Register(p => DependencyService.Get<IPushNotificationsService>()).
                As<IPushNotificationsService>().SingleInstance();
        }

        protected override void RegisterViews(IViewFactory viewFactory)
        {
            base.RegisterViews(viewFactory);

            // Register ViewModel <-> View relations
            viewFactory.Register<StartViewModel, StartView>();
            viewFactory.Register<LoginViewModel, LoginView>();
            viewFactory.Register<MainClientViewModel, MainClientView>();
            viewFactory.Register<ClientAppointmentsViewModel, ClientAppointmentsView>();
            viewFactory.Register<FavouritesViewModel, FavouritesView>();
            viewFactory.Register<EstablishmentSearchViewModel, EstablishmentSearchView>();            
            viewFactory.Register<EstablishmentSearchResultsViewModel, EstablishmentSearchResultsView>();
            viewFactory.Register<EstablishmentDetailsViewModel, EstablishmentDetailsView>();
            viewFactory.Register<BookViewModel, BookView>();
            viewFactory.Register<ErrorViewModel, ErrorView>();
            viewFactory.Register<ChangePasswordViewModel, ChangePasswordView>();
            viewFactory.Register<AccountDetailsViewModel, AccountDetailsView>();
            viewFactory.Register<CreateAccountViewModel, CreateAccountView>();
            viewFactory.Register<OwnerEstablishmentsViewModel, OwnerEstablishmentsView>();
            viewFactory.Register<EstablishmentAppointmentsViewModel, EstablishmentAppointmentsView>();
            viewFactory.Register<EstablishmentStaffViewModel, EstablishmentStaffView>();
            viewFactory.Register<MainOwnerViewModel, MainOwnerView>();
            viewFactory.Register<MapViewModel, MapView>();
            viewFactory.Register<StaffAccountDetailsViewModel, StaffAccountDetailsView>();
            viewFactory.Register<EstablishmentServicesViewModel, EstablishmentServicesView>();
            viewFactory.Register<CreateStaffAccountViewModel, CreateStaffAccountView>();
            viewFactory.Register<AppointmentDetailsViewModel, AppointmentDetailsView>();
            viewFactory.Register<ClientHistoryViewModel, ClientHistoryView>();
            viewFactory.Register<CreateEstablishmentViewModel, CreateEstablishmentView>();
            viewFactory.Register<OwnerEstablishmentDetailsViewModel, OwnerEstablishmentDetailsView>();
            viewFactory.Register<MainStaffViewModel, MainStaffView>();
            viewFactory.Register<InformationViewModel, InformationView>();
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
