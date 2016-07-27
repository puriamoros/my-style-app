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
    public class AppointmentDetailsViewModel : NavigableViewModelBase
    {
        public AppointmentDetailsViewModel(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService,
            IUsersService userService,
            IServicesService servicesService,
            IAppointmentsService appointmentsService,
            IEstablishmentsService establishmentsService) :
            base(navigator, userNotificator, localizedStringsService)
        { 
            
            this.Title = this.LocalizedStrings.GetString("appointment_details");

        }

        

        //protected override void ConfigureValidationService()
        //{
        //    base.ConfigureValidationService();
        //}

        //protected override async void SaveAccountAsync()
        //{
        //    string validationError = this.GetValidationError();

        //    if (validationError == null)
        //    {
        //        this.ErrorText = "";
        //        await this.ExecuteBlockingUIAsync(
        //            async () =>
        //            {
        //                try
        //                {
        //                    Staff staff = new Staff()
        //                    {
        //                        Id = this.User.Id,
        //                        Name = this.User.Name,
        //                        Surname = this.User.Surname,
        //                        Phone = this.User.Phone,
        //                        Email = this.User.Email,
        //                        Password = "",
        //                        UserType = (UserTypeEnum)this.SelectedUserType.Id,
        //                        IdEstablishment = this.SelectedEstablishment.Id
        //                    };

        //                    await this._usersService.UpdateStaffAsync(staff);

        //                    MessagingCenter.Send<Staff>(staff, "staffChanged");

        //                    // TODO:
        //                    // avisar al usuario con un popup de que el cambio se ha aplicado
        //                    // habría que mandar un push al empleado que se ha cambiado para que haga logout y entre con sus nuevos permisos

        //                    await this.UserNotificator.DisplayAlert(
        //                       this.LocalizedStrings.GetString("modified_account"),
        //                       this.LocalizedStrings.GetString("modified_account"),
        //                       this.LocalizedStrings.GetString("ok"));

        //                }
        //                catch (BackendException ex)
        //                {
        //                    if (ex.BackendError.State == (int)BackendStatusCodeEnum.StateDuplicatedKeyError)
        //                    {
        //                        this.ErrorText = this.LocalizedStrings.GetString("error_duplicated_email");
        //                        return;
        //                    }
        //                    else
        //                    {
        //                        throw;
        //                    }
        //                }
        //            });
        //    }
        //    else
        //    {
        //        this.ErrorText = validationError;
        //    }
        //}

        //protected override bool CanSaveAccount()
        //{
        //    return this.SelectedEstablishment != null && this.SelectedUserType != null;
        //}
    }
}
