using System.Threading.Tasks;
using MyStyleApp.Services;
using MvvmCore;
using System;
using System.Linq;
using MyStyleApp.Enums;
using MyStyleApp.Models;
using MyStyleApp.Services.Backend;
using System.Collections.ObjectModel;
using MyStyleApp.Exceptions;
using Xamarin.Forms;

namespace MyStyleApp.ViewModels
{
    public class CreateStaffAccountViewModel : CreateAccountViewModel
    {
        private const string LOCALIZATION_TOKEN = "user_type_";

        private IEstablishmentsService _establishmentsService;
        private IUsersService _usersService;

        public CreateStaffAccountViewModel(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService,
            ValidationService validationService,
            IUsersService usersService,
            IEstablishmentsService establishmentsService) :
            base(navigator, userNotificator, localizedStringsService, validationService, usersService)
        {
            this._establishmentsService = establishmentsService;
            this._usersService = usersService;

            this.Title = this.LocalizedStrings.GetString("new_staff");
            this.IsTitleVisible = true;
            this.IsOwnerOptionVisible = false;

            this.UserTypeList = new ObservableCollection<UserType>();
            foreach (UserTypeEnum userType in Enum.GetValues(typeof(UserTypeEnum)))
            {
                if (userType != UserTypeEnum.Client && userType != UserTypeEnum.Owner)
                {
                    int id = (int) userType;
                    this.UserTypeList.Add(new UserType()
                    {
                        Id = id,
                        Name = this.LocalizedStrings.GetString(LOCALIZATION_TOKEN + id)
                    });
                }
            }
        }

        public async Task Initialize()
        {
            this.IsOwnerOptionVisible = (this._usersService.LoggedUser.UserType == UserTypeEnum.Owner);

            base.Initialize();

            this.SelectedEstablishment = null;
            this.EstablishmentList = null;

            var list = await this._establishmentsService.GetOwnerEstablishmentsAsync();
            this.EstablishmentList = new ObservableCollection<Establishment>(list);

            this.SelectedUserType = null;
        }

        protected override void ConfigureValidationService()
        {
            base.ConfigureValidationService();
        }

        private string GetValidationError()
        {
            this.ConfigureValidationService();

            return this._validationService.GetValidationError();
        }

        protected override async void CreateAccountAsync()
        {
            string validationError = this.GetValidationError();
            if (validationError != null)
            {
                this.ErrorText = this.LocalizedStrings.GetString("error") + ": " + validationError;
                return;
            }

            this.ErrorText = "";

            await this.ExecuteBlockingUIAsync(
                async () =>
                {
                    try
                    {
                        Staff staff = new Staff()
                        {
                            Name = this.Name,
                            Surname = this.Surname,
                            Phone = this.Phone,
                            Email = this.Email,
                            Password = this.Password,
                            UserType = (UserTypeEnum)this.SelectedUserType.Id,
                            IdEstablishment = this.SelectedEstablishment.Id
                        };
                        Staff newStaff = await this._usersService.CreateStaffAsync(staff);
                    }
                    catch (BackendException ex)
                    {
                        if (ex.BackendError.State == (int)BackendStatusCodeEnum.StateDuplicatedKeyError)
                        {
                            this.ErrorText = this.LocalizedStrings.GetString("error_duplicated_email");
                            return;
                        }
                        else
                        {
                            throw;
                        }
                    }

                    this.IsBusy = false;

                    await this.UserNotificator.DisplayAlert(
                       this.LocalizedStrings.GetString("created_account"),
                       this.LocalizedStrings.GetString("please_login"),
                       this.LocalizedStrings.GetString("ok"));

                    await this.PopNavPageAsync();
                });
        }
        
        protected override bool CanSaveAccount()
        {
            return base.CanSaveAccount() && this.SelectedEstablishment != null && this.SelectedUserType != null;
        }
    }
}
