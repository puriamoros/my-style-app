using MyStyleApp.Services;
using MvvmCore;
using MyStyleApp.Validators;
using MyStyleApp.Constants;
using System.Windows.Input;
using Xamarin.Forms;
using MyStyleApp.Enums;
using MyStyleApp.Models;
using MyStyleApp.Services.Backend;
using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using MyStyleApp.Utils;

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

        private string _name;
        private string _address;
        private string _phone;
        private DateTime _hours1;
        private DateTime _hours2;
        private string _concurrence;
        private string _latitude;
        private string _longitude;
        private bool _autoConfirm;

        protected List<ShortenService> _shortenServices;
        protected EstablishmentTypeEnum _establishmentType;

        private BaseModeEnum _mode;
        
        public EstablishmentViewModelBase(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService,
            ProvincesService provincesService,
            ValidationService validationService,
            IUsersService usersService) :
            base(navigator, userNotificator, localizedStringsService)
        {
            this._provincesService = provincesService;
            this._validationService = validationService;

            this.CreateEstablishmentCommand = new Command(this.CreateEstablishmentAsync, this.CanSaveOrCreateEstablishment);            
            this.EditEstablishmentCommand = new Command(this.EditEstablishment);
            this.SaveEstablishmentCommand = new Command(this.SaveEstablishmentAsync, this.CanSaveOrCreateEstablishment);
            this.OfferedServicesCommand = new Command(this.OfferedServices);
            this.CancelCommand = new Command(this.Cancel);

            this.ProvinceList = new ObservableCollection<Province>(this._provincesService.GetProvinces());

            this.Mode = BaseModeEnum.View;           
        }

        protected void Initialize(Establishment establishment, BaseModeEnum mode)
        {
            this._shortenServices = null;
            this._establishmentType = EstablishmentTypeEnum.Unknown;

            if (establishment != null)
            {
                this.Name = establishment.Name;
                this.Address = establishment.Address;
                this.Phone = establishment.Phone;
                //this.Hours1 = establishment.Hours1;
                //this.Hours2 = establishment.Hours2;
                this.AutoConfirm = (establishment.ConfirmType == ConfirmTypeEnum.Automatic);
                this.Concurrence = establishment.Concurrence.ToString();
                this.Latitude = establishment.Latitude.ToString();
                this.Longitude = establishment.Longitude.ToString();
             }
            else
            {
                this.Name = "";
                this.Address = "";
                this.Phone = "";
                //this.Hours1 = establishment.Hours1;
                //this.Hours2 = establishment.Hours2;
                this.AutoConfirm = false;
                this.Concurrence = "";
                this.Latitude = "";
                this.Longitude = "";
            }
            this.ErrorText = "";

            this.Mode = mode;
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
            }
        }
        public string Address
        {
            get { return _address; }
            set
            {
                SetProperty(ref _address, value);
                this.CreateEstablishmentCommand.ChangeCanExecute();
            }
        }
        public string Phone
        {
            get { return _phone; }
            set
            {
                SetProperty(ref _phone, value);
                this.CreateEstablishmentCommand.ChangeCanExecute();
            }
        }
        public DateTime Hours1
        {
            get { return _hours1; }
            set { SetProperty(ref _hours1, value); }
        }

        public DateTime Hours2
        {
            get { return _hours2; }
            set { SetProperty(ref _hours2, value); }
        }

        public string Concurrence
        {
            get { return _concurrence; }
            set
            {
                SetProperty(ref _concurrence, value);
                this.CreateEstablishmentCommand.ChangeCanExecute();
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
            this._validationService.AddValidator(
                new RequiredValidator(this.Latitude, STRING_LATITUDE));
            this._validationService.AddValidator(
                new RegexValidator(
                    this.Latitude, RegexConstants.DOUBLE,
                    "error_number_double", STRING_LATITUDE));

            // Longitude
            this._validationService.AddValidator(
                new RequiredValidator(this.Longitude, STRING_LONGITUDE));
            this._validationService.AddValidator(
                new RegexValidator(
                    this.Longitude, RegexConstants.DOUBLE,
                    "error_number_double", STRING_LONGITUDE));

            // Phone
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

            // Concurrence
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

        protected virtual void OfferedServices()
        {
        }

        protected virtual bool CanSaveOrCreateEstablishment()
        {
            return !string.IsNullOrEmpty(this.Name) && !string.IsNullOrEmpty(this.Address) && !string.IsNullOrEmpty(this.Phone)
                && this.SelectedProvince != null && !string.IsNullOrEmpty(this.Concurrence);
        }

    }
}

