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

namespace MyStyleApp.ViewModels
{
    public abstract class EstablishmentViewModelBase : NavigableViewModelBase
    {
        
        public Command CreateEstablishmentCommand { get; private set; }
        public Command EditEstablishmentCommand { get; private set; }
        public Command SaveEstablishmentCommand { get; private set; }
        public Command CancelCommand { get; private set; }

        private Establishment _establishment;
        private string _errorText;

        //private ObservableCollection<UserType> _userTypeList;
        //private UserType _selectedUserType;

        //private ObservableCollection<Establishment> _establishmentList;
        //private Establishment _selectedEstablishment;

        private AccountModeEnum _mode;
        
        public EstablishmentViewModelBase(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService,
            IUsersService usersService) :
            base(navigator, userNotificator, localizedStringsService)
        {
            this.CreateEstablishmentCommand = new Command(this.CreateEstablishmentAsync, this.CanCreateEstablishment);            
            this.EditEstablishmentCommand = new Command(this.EditEstablishment);
            this.SaveEstablishmentCommand = new Command(this.SaveEstablishmentAsync, this.CanSaveEstablishment);
            this.CancelCommand = new Command(this.Cancel);
            
            this.Mode = AccountModeEnum.View;           
        }

        protected void Initialize(Establishment establishment, AccountModeEnum mode)
        {
            if (establishment != null)
            {
                this.Establishment = new Establishment()
                {
                    Name = establishment.Name,
                    Address = establishment.Address,
                    Phone = establishment.Phone,
                    IdProvince = establishment.IdProvince,
                    Concurrence = establishment.Concurrence,
                    Hours1 = establishment.Hours1,
                    Hours2 = establishment.Hours2,
                    ConfirmType = establishment.ConfirmType,
                    Latitude = establishment.Latitude,
                    Longitude = establishment.Longitude
                };
             }
            else
            {
                this.Establishment = new Establishment()
                {
                    Name = "",
                    Address = "",
                    Phone = "",
                    IdProvince = 1,
                    Concurrence = 1,
                    Hours1 = "",
                    Hours2 = "",
                    ConfirmType = ConfirmTypeEnum.Automatic,
                    Latitude = 0.0,
                    Longitude = 0.0 
                };
            }
            this.ErrorText = "";

            this.Mode = mode;
        }

        public Establishment Establishment
        {
            get { return _establishment; }
            set { SetProperty(ref _establishment, value); }
        }

        public AccountModeEnum Mode
        {
            get { return _mode; }
            set { SetProperty(ref _mode, value); }
        }

        public string ErrorText
        {
            get { return _errorText; }
            set { SetProperty(ref _errorText, value); }
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

        protected virtual bool CanSaveEstablishment()
        {
            return !string.IsNullOrEmpty(this.Establishment.Name) && !string.IsNullOrEmpty(this.Establishment.Address) && !string.IsNullOrEmpty(this.Establishment.Phone)
                && !string.IsNullOrEmpty(this.Establishment.Hours1) && !string.IsNullOrEmpty(this.Establishment.Hours2) && this.Establishment.IdProvince != 0 && this.Establishment.Concurrence != 0;
        }

        protected virtual bool CanCreateEstablishment()
        {
            return !string.IsNullOrEmpty(this.Establishment.Name) && !string.IsNullOrEmpty(this.Establishment.Address) && !string.IsNullOrEmpty(this.Establishment.Phone)
                && !string.IsNullOrEmpty(this.Establishment.Hours1) && !string.IsNullOrEmpty(this.Establishment.Hours2) && this.Establishment.IdProvince != 0 && this.Establishment.Concurrence != 0;
        }

    }
}

