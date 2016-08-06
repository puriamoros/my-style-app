using MyStyleApp.Services;
using MvvmCore;
using MyStyleApp.Validators;
using MyStyleApp.Constants;
using System.Linq;
using Xamarin.Forms;
using MyStyleApp.Enums;
using MyStyleApp.Models;
using MyStyleApp.Services.Backend;
using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using MyStyleApp.Utils;
using System.Globalization;
using System.Threading.Tasks;

namespace MyStyleApp.ViewModels
{
    public abstract class EstablishmentViewModelBase : NavigableViewModelBase
    {
        private const string STRING_NAME = "name";
        private const string STRING_ADDRESS = "addres";
        private const string STRING_PHONE = "phone";
        private const string STRING_CONCURRENCE = "concurrence";
        //private const string STRING_HOURS1 = "hours1";
        //private const string STRING_HOURS2 = "hours2";
        private const string STRING_LATITUDE = "latitude";
        private const string STRING_LONGITUDE = "longitude";

        private IServicesService _servicesService;
        private IServiceCategoriesService _serviceCategoriesService;
        private IUsersService _usersService;
        protected IEstablishmentsService _establishmentsService;

        public Command CreateEstablishmentCommand { get; private set; }
        public Command EditEstablishmentCommand { get; private set; }
        public Command SaveEstablishmentCommand { get; private set; }
        public Command CancelCommand { get; private set; }
        public Command OfferedServicesCommand { get; private set; }

        private ProvincesService _provincesService;
        protected ValidationService _validationService;

        private string _errorText;
        private ObservableCollection<Province> _provinceList;
        private Province _selectedProvince;
        
        protected Establishment _establishment;
        private string _name;
        private string _address;
        private string _phone;
        private TimeSpan _hours1Start;
        private TimeSpan _hours1End;
        private TimeSpan _hours2Start;
        private TimeSpan _hours2End;
        private string _concurrence;
        private string _latitude;
        private string _longitude;
        private bool _autoConfirm;
        private bool _hours1Selected;
        private bool _hours2Selected;

        protected List<ShortenService> _shortenServices;
        protected EstablishmentTypeEnum _establishmentType;

        private BaseModeEnum _mode;
        
        public EstablishmentViewModelBase(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService,
            ProvincesService provincesService,
            ValidationService validationService,
            IUsersService usersService,
            IServicesService servicesService,
            IServiceCategoriesService serviceCategoriesService,
            IEstablishmentsService establishmentsService) :
            base(navigator, userNotificator, localizedStringsService)
        {
            this._provincesService = provincesService;
            this._validationService = validationService;
            this._usersService = usersService;
            this._servicesService = servicesService;
            this._serviceCategoriesService = serviceCategoriesService;
            this._establishmentsService = establishmentsService;

            this.CreateEstablishmentCommand = new Command(this.CreateEstablishmentAsync, this.CanSaveOrCreateEstablishment);            
            this.EditEstablishmentCommand = new Command(this.EditEstablishment);
            this.SaveEstablishmentCommand = new Command(this.SaveEstablishmentAsync, this.CanSaveOrCreateEstablishment);
            this.OfferedServicesCommand = new Command(this.OfferedServices);
            this.CancelCommand = new Command(this.Cancel);

            this.ProvinceList = new ObservableCollection<Province>(this._provincesService.GetProvinces());

            this.Mode = BaseModeEnum.View;           
        }

        protected async void InitializeAsync(Establishment establishment, BaseModeEnum mode)
        {
            this._establishment = establishment;
            this._establishmentType = EstablishmentTypeEnum.Unknown;

            if (establishment != null)
            {
                this.Name = establishment.Name;
                this.Address = establishment.Address;
                this.Phone = establishment.Phone;

                this.Hours1Selected = !string.IsNullOrEmpty(establishment.Hours1);
                var openingHours = this.GetOpeningHours(establishment.Hours1);
                this.Hours1Start = openingHours[0];
                this.Hours1End = openingHours[1];

                this.Hours2Selected = !string.IsNullOrEmpty(establishment.Hours2);
                openingHours = this.GetOpeningHours(establishment.Hours2);
                this.Hours2Start = openingHours[0];
                this.Hours2End = openingHours[1];

                this.AutoConfirm = (establishment.ConfirmType == ConfirmTypeEnum.Automatic);
                this.Concurrence = establishment.Concurrence.ToString();
                this.Latitude = establishment.Latitude.ToString();
                this.Longitude = establishment.Longitude.ToString();

                var result = from item in this.ProvinceList
                             where item.Id == establishment.IdProvince
                             select item;

                if(result.Count() > 0)
                {
                    this.SelectedProvince = result.ElementAt(0);
                }  
                else
                {
                    this.SelectedProvince = null;
                }

                this._shortenServices = establishment.ShortenServices;      
             }
            else
            {
                this.Name = "";
                this.Address = "";
                this.Phone = "";
                this.Hours1Selected = false;
                this.Hours1Start = new TimeSpan();
                this.Hours1End = new TimeSpan();
                this.Hours2Selected = false;
                this.Hours2Start = new TimeSpan();
                this.Hours2End = new TimeSpan();
                this.AutoConfirm = false;
                this.Concurrence = "";
                this.Latitude = "";
                this.Longitude = "";
                this.SelectedProvince = null;
                this._shortenServices = null;
            }
            this.ErrorText = "";

            if(Device.OS == TargetPlatform.Android && mode == BaseModeEnum.View)
            {
                // Hack to solve an strange behaviour with CheckBoxImage on Android
                this.Mode = BaseModeEnum.Edit;
                await Task.Delay(100);
                this.Mode = BaseModeEnum.View;
            }
            else
            {
                this.Mode = mode;
            }
        }

        private TimeSpan[] GetOpeningHours(string hours)
        {
            TimeSpan[] hoursTs = new TimeSpan[2];
            hoursTs[0] = new TimeSpan();
            hoursTs[1] = new TimeSpan();

            string [] openingHours = hours.Split(new char[] { '-' });
            if(openingHours.Length == 2)
            {
                string openingStart = openingHours[0];
                string openingEnd = openingHours[1];

                hoursTs[0] = GetOpeningHour(openingStart);
                hoursTs[1] = GetOpeningHour(openingEnd);
            }

            return hoursTs;
        }

        private TimeSpan GetOpeningHour(string hour)
        {
            TimeSpan hourTs;

            string[] openingHour = hour.Split(new char[] { ':' });
            if (openingHour.Length == 2)
            {
                int hours, minutes;
                if (int.TryParse(openingHour[0], out hours) && int.TryParse(openingHour[1], out minutes))
                {
                    hourTs = new TimeSpan(hours, minutes, 0);
                }
            }
            return hourTs;
        }

        protected void SetShortenServicesList(EstablishmentTypeEnum establishmentType, List<ShortenService> shortenServices)
        {
            this._establishmentType = establishmentType;
            this._shortenServices = shortenServices;
        }

        public ObservableCollection<Province> ProvinceList
        {
            get { return _provinceList; }
            set { SetProperty(ref _provinceList, value); }
        }

        public Province SelectedProvince
        {
            get { return _selectedProvince; }
            set
            {
                SetProperty(ref _selectedProvince, value);
                this.CreateEstablishmentCommand.ChangeCanExecute();
                this.SaveEstablishmentCommand.ChangeCanExecute();
            }
        }

        public BaseModeEnum Mode
        {
            get { return _mode; }
            set { SetProperty(ref _mode, value); }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                SetProperty(ref _name, value);
                this.CreateEstablishmentCommand.ChangeCanExecute();
                this.SaveEstablishmentCommand.ChangeCanExecute();
            }
        }
        public string Address
        {
            get { return _address; }
            set
            {
                SetProperty(ref _address, value);
                this.CreateEstablishmentCommand.ChangeCanExecute();
                this.SaveEstablishmentCommand.ChangeCanExecute();
            }
        }
        public string Phone
        {
            get { return _phone; }
            set
            {
                SetProperty(ref _phone, value);
                this.CreateEstablishmentCommand.ChangeCanExecute();
                this.SaveEstablishmentCommand.ChangeCanExecute();
            }
        }
        public TimeSpan Hours1Start
        {
            get { return _hours1Start; }
            set { SetProperty(ref _hours1Start, this.CheckHour(value)); }
        }

        public TimeSpan Hours1End
        {
            get { return _hours1End; }
            set { SetProperty(ref _hours1End, this.CheckHour(value)); }
        }

        public TimeSpan Hours2Start
        {
            get { return _hours2Start; }
            set { SetProperty(ref _hours2Start, this.CheckHour(value)); }
        }

        public TimeSpan Hours2End
        {
            get { return _hours2End; }
            set { SetProperty(ref _hours2End, this.CheckHour(value)); }
        }

        public bool Hours1Selected
        {
            get { return _hours1Selected; }
            set { SetProperty(ref _hours1Selected, value); }
        }

        public bool Hours2Selected
        {
            get { return _hours2Selected; }
            set { SetProperty(ref _hours2Selected, value); }
        }

        private TimeSpan CheckHour(TimeSpan value)
        {
            if(value.Minutes != 0 && value.Minutes != 30)
            {
                int minutes = (value.Minutes > 0 && value.Minutes < 30) ? 30 : 0;
                var ts = new TimeSpan(value.Hours, minutes, value.Seconds);
                if(minutes == 0)
                {
                    ts = ts.Add(TimeSpan.FromHours(1));
                }
                return ts;
            }
            else
            {
                return value;
            }
        }

        public string Concurrence
        {
            get { return _concurrence; }
            set
            {
                SetProperty(ref _concurrence, value);
                this.CreateEstablishmentCommand.ChangeCanExecute();
                this.SaveEstablishmentCommand.ChangeCanExecute();
            }
        }

        public string Latitude
        {
            get { return _latitude; }
            set { SetProperty(ref _latitude, value); }
        }

        public string Longitude
        {
            get { return _longitude; }
            set { SetProperty(ref _longitude, value); }
        }

        public bool AutoConfirm
        {
            get { return _autoConfirm; }
            set { SetProperty(ref _autoConfirm, value); }
        }

        public string ErrorText
        {
            get { return _errorText; }
            set { SetProperty(ref _errorText, value); }
        }

        public void ConfigureValidationService()
        {
            // Alwais clear validators before adding
            this._validationService.ClearValidators();

            // Name
            this._validationService.AddValidator(
                new RequiredValidator(this.Name, STRING_NAME));
            this._validationService.AddValidator(
                new RegexValidator(
                    this.Name, RegexConstants.NOT_INSECURE_CHARS,
                    "error_insecure_chars", STRING_NAME));
            this._validationService.AddValidator(
                new LengthValidator(this.Name, STRING_NAME, 2, 200));

            // Address
            this._validationService.AddValidator(
                new RequiredValidator(this.Address, STRING_ADDRESS));
            this._validationService.AddValidator(
                new RegexValidator(
                    this.Address, RegexConstants.NOT_INSECURE_CHARS,
                    "error_insecure_chars", STRING_ADDRESS));
            this._validationService.AddValidator(
                new LengthValidator(this.Address, STRING_ADDRESS, 2, 500));


            // Latitude
            if(!string.IsNullOrWhiteSpace(this.Latitude))
            {
                this.Latitude = this.Latitude.Replace(',', '.').Trim();
                this._validationService.AddValidator(
                    new RegexValidator(
                        this.Latitude, RegexConstants.DOUBLE,
                        "error_number_double", STRING_LATITUDE));
            }
            

            // Longitude
            if (!string.IsNullOrWhiteSpace(this.Longitude))
            {
                this.Longitude = this.Longitude.Replace(',', '.').Trim();
                this._validationService.AddValidator(
                    new RegexValidator(
                        this.Longitude, RegexConstants.DOUBLE,
                        "error_number_double", STRING_LONGITUDE));
            }


            // Phone
            this.Phone = this.Phone.Trim();
            this._validationService.AddValidator(
                new RequiredValidator(this.Phone, STRING_PHONE));
            this._validationService.AddValidator(
                new RegexValidator(
                    this.Phone, RegexConstants.NOT_INSECURE_CHARS,
                    "error_insecure_chars", STRING_PHONE));
            this._validationService.AddValidator(
                new LengthValidator(this.Phone, STRING_PHONE, 9, 9));
            this._validationService.AddValidator(
                new RegexValidator(
                    this.Phone, RegexConstants.PHONE,
                    "error_invalid_field", STRING_PHONE));


            // Hours1
            if (this.Hours1Selected)
            {
                this._validationService.AddValidator(
                    new SimpleValidator<KeyValuePair<TimeSpan, TimeSpan>>(
                        new KeyValuePair<TimeSpan, TimeSpan>(this.Hours1Start, this.Hours1End),
                        (hours) => { return hours.Key < hours.Value; },
                        this.LocalizedStrings.GetString("error_opening_hours", "${FIELD_NAME}", this.LocalizedStrings.GetString("hours1")
                        )
                    )
                );
            }


            // Hours2
            if (this.Hours2Selected)
            {
                this._validationService.AddValidator(
                    new SimpleValidator<KeyValuePair<TimeSpan, TimeSpan>>(
                        new KeyValuePair<TimeSpan, TimeSpan>(this.Hours2Start, this.Hours2End),
                        (hours) => { return hours.Key < hours.Value; },
                        this.LocalizedStrings.GetString("error_opening_hours", "${FIELD_NAME}", this.LocalizedStrings.GetString("hours2")
                        )
                    )
                );
            }

            // Concurrence
            this.Concurrence = this.Concurrence.Trim();
            this._validationService.AddValidator(
                new RequiredValidator(this.Concurrence, STRING_CONCURRENCE));
            this._validationService.AddValidator(
                new RegexValidator(
                    this.Concurrence, RegexConstants.POSITIVE_INT,
                    "error_number_positive_int", STRING_CONCURRENCE));
        }

        public string GetValidationError()
        {
            this.ConfigureValidationService();

            return this._validationService.GetValidationError();
        }

        protected virtual async void CreateEstablishmentAsync()
        {
        }

        protected virtual void EditEstablishment()
        {
        }

        protected virtual async void SaveEstablishmentAsync()
        {
        }

        protected virtual void Cancel()
        {
        }

        protected virtual async void OfferedServices()
        {
            await this.ExecuteBlockingUIAsync(
                async () =>
                {
                    var servicesCategories = await this._serviceCategoriesService.GetServiceCategoriesAsync();
                    var services = await this._servicesService.GetServicesAsync();

                    // There is a problem with one element of the view when showing page as modal on WinPhone
                    if (Device.OS == TargetPlatform.Windows || Device.OS == TargetPlatform.WinPhone)
                    {
                        await this.PushNavPageAsync<EstablishmentServicesViewModel>((establishmentServicesVM) =>
                        {
                            establishmentServicesVM.Initialize(this._shortenServices, servicesCategories, services, this.SetShortenServicesList);
                        });
                    }
                    else
                    {
                        await this.PushNavPageModalAsync<EstablishmentServicesViewModel>((establishmentServicesVM) =>
                        {
                            establishmentServicesVM.Initialize(this._shortenServices, servicesCategories, services, this.SetShortenServicesList);
                        });
                    }
                });
        }

        protected Establishment CreateEstablishmentFromData()
        {
            Establishment establishment = new Establishment()
            {
                Id = (this._establishment == null) ? 0 : this._establishment.Id,
                IdOwner = this._usersService.LoggedUser.Id,
                Name = this.Name,
                IdProvince = this.SelectedProvince.Id,
                Address = this.Address,
                Phone = this.Phone,
                Hours1 = (this.Hours1Selected) ?
                    string.Format("{0:00}:{1:00}-{2:00}:{3:00}", Hours1Start.Hours, Hours1Start.Minutes, Hours1End.Hours, Hours1Start.Minutes) :
                    "",
                Hours2 = (this.Hours2Selected) ?
                    string.Format("{0:00}:{1:00}-{2:00}:{3:00}", Hours2Start.Hours, Hours2Start.Minutes, Hours2End.Hours, Hours2Start.Minutes) :
                    "",
                ConfirmType = (this.AutoConfirm) ? ConfirmTypeEnum.Automatic : ConfirmTypeEnum.Manual,
                Concurrence = int.Parse(this.Concurrence),
                Latitude = (string.IsNullOrWhiteSpace(this.Latitude)) ? 0 :
                                        double.Parse(this.Latitude, CultureInfo.InvariantCulture),
                Longitude = (string.IsNullOrWhiteSpace(this.Longitude)) ? 0 :
                                        double.Parse(this.Longitude, CultureInfo.InvariantCulture),
                IdEstablishmentType = (int)this._establishmentType,
                ShortenServices = this._shortenServices
            };

            return establishment;
        }
        
        private bool CanSaveOrCreateEstablishment()
        {
            return !string.IsNullOrEmpty(this.Name) && !string.IsNullOrEmpty(this.Address) && !string.IsNullOrEmpty(this.Phone)
                && this.SelectedProvince != null && !string.IsNullOrEmpty(this.Concurrence);
        }
    }
}

