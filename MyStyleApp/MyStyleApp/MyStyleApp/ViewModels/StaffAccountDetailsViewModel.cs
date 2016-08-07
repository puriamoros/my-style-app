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
    public class StaffAccountDetailsViewModel : AccountDetailsViewModel
    {
        private const string LOCALIZATION_TOKEN = "user_type_";

        private IEstablishmentsService _establishmentsService;

        private int _staffId;

        public StaffAccountDetailsViewModel(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService,
            ValidationService validationService,
            IUsersService usersService,
            IEstablishmentsService establishmentsService) :
            base(navigator, userNotificator, localizedStringsService, validationService, usersService)
        {
            this._establishmentsService = establishmentsService;

            this.Title = this.LocalizedStrings.GetString("staff_account");
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

        protected override void SubscribeToMessages()
        {
            // Nothing to subscribe
        }

        public async Task Initialize(Staff staff)
        {
            this.IsOwnerOptionVisible = (this._usersService.LoggedUser.UserType == UserTypeEnum.Owner);

            base.Initialize(staff);

            this._staffId = staff.Id;

            this.SelectedEstablishment = null;
            this.EstablishmentList = null;

            var list = await this._establishmentsService.GetOwnerEstablishmentsAsync();
            this.EstablishmentList = new ObservableCollection<Establishment>(list);

            var result1 =
                from item in this.EstablishmentList
                where item.Id == staff.IdEstablishment
                select item;

            if (result1.Count() > 0)
            {
                this.SelectedEstablishment = result1.ElementAt(0);
            }

            var result2 =
                from item in this.UserTypeList
                where item.Id == (int)staff.UserType
                select item;

            if(result2.Count() > 0)
            {
                this.SelectedUserType = result2.ElementAt(0);
            }
        }

        protected override async void SaveAccountAsync()
        {
            string validationError = this.GetValidationError();

            if (validationError == null)
            {
                this.ErrorText = "";
                await this.ExecuteBlockingUIAsync(
                    async () =>
                    {
                        try
                        {
                            Staff staff = new Staff()
                            {
                                Id = this._staffId,
                                Name = this.Name,
                                Surname = this.Surname,
                                Phone = this.Phone,
                                Email = this.Email,
                                UserType = (UserTypeEnum)this.SelectedUserType.Id,
                                IdEstablishment = this.SelectedEstablishment.Id
                            };

                            await this._usersService.UpdateStaffAsync(staff);

                            MessagingCenter.Send<Staff>(staff, "staffChanged");

                            // TODO:
                            // habría que mandar un push al empleado que se ha cambiado para que haga logout y entre con sus nuevos permisos

                            await this.UserNotificator.DisplayAlert(
                               this.LocalizedStrings.GetString("account_modified"),
                               this.LocalizedStrings.GetString("account_modified_body"),
                               this.LocalizedStrings.GetString("ok"));

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
                    });
            }
            else
            {
                this.ErrorText = validationError;
            }
        }

        protected override bool CanSaveAccount()
        {
            return base.CanSaveAccount() && this.SelectedEstablishment != null && this.SelectedUserType != null;
        }
    }
}
