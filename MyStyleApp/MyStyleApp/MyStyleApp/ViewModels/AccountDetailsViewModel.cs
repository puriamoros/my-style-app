using System.Threading.Tasks;
using MyStyleApp.Services;
using MvvmCore;
using MyStyleApp.Validators;
using MyStyleApp.Constants;
using System;
using System.Windows.Input;
using Xamarin.Forms;
using MyStyleApp.Enums;
using MyStyleApp.Models;
using MyStyleApp.Services.Backend;
using MyStyleApp.Exceptions;

namespace MyStyleApp.ViewModels
{
    public class AccountDetailsViewModel : AccountViewModelBase
    {
        public AccountDetailsViewModel(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService,
            ValidationService validationService,
            IUsersService usersService) :
            base(navigator, userNotificator, localizedStringsService, validationService, usersService)
        {
            this.Title = this.LocalizedStrings.GetString("my_account");
            this.IsTitleVisible = false;
            this.IsOwnerOptionVisible = false;

            this.SubscribeToMessages();
        }

        protected virtual void SubscribeToMessages()
        {
            MessagingCenter.Subscribe<string>(this, "userLogin", (userType) =>
            {
                MessagingCenter.Subscribe<string>(this, "changeAccountMode", this.OnAccountModeChanged);
            });
            MessagingCenter.Subscribe<string>(this, "userLogout", (userType) =>
            {
                MessagingCenter.Unsubscribe<string>(this, "changeAccountMode");
            });

            // Need to subscribe on ctor as well since first userLogin message is delivered before this object is created
            MessagingCenter.Subscribe<string>(this, "changeAccountMode", this.OnAccountModeChanged);
        }

        public void Initialize(User user)
        {
            base.Initialize(user, BaseModeEnum.View);
        }

        protected string GetValidationError()
        {
            this.ConfigureValidationService();

            return this._validationService.GetValidationError();
        }

        protected override void EditAccount()
        {
            this.Mode = BaseModeEnum.Edit;
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
                            User user = new User()
                            {
                                Id = this._usersService.LoggedUser.Id,
                                Name = this.Name,
                                Surname = this.Surname,
                                Phone = this.Phone,
                                Email = this.Email,
                                UserType = this._usersService.LoggedUser.UserType
                            };
                            await this._usersService.UpdateUserAsync(user);
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


                        this.Mode = BaseModeEnum.View;

                        try
                        {
                            await this._usersService.MeAsync();
                            this.Initialize(this._usersService.LoggedUser);
                        }
                        catch
                        {
                            await this._usersService.LogoutAsync();
                            await this.SetMainPageNavPageAsync<LoginViewModel>();
                        }
                    });
            }
            else
            {
                this.ErrorText = validationError;
            } 
        }

        private void OnAccountModeChanged(string accountModeStr)
        {
            BaseModeEnum accountMode;
            if (Enum.TryParse(accountModeStr, out accountMode))
            {
                if (accountMode == BaseModeEnum.Edit || accountMode == BaseModeEnum.View)
                {
                    this.Initialize(this._usersService.LoggedUser);
                    this.Mode = accountMode;
                }
            }
        }

        protected override void Cancel()
        {
            this.Initialize(this._usersService.LoggedUser);
            this.Mode = BaseModeEnum.View;
        }

        protected override async void ChangePasswordAccountAsync()
        {
            await this.ExecuteBlockingUIAsync(
                async () =>
                {
                    await this.PushNavPageAsync<ChangePasswordViewModel>((changePVM) =>
                    {
                        changePVM.Initialize();
                    });
                });
        }

        protected override async void LogOutAsync()
        {
            bool result = await this.UserNotificator.DisplayAlert(
                this.LocalizedStrings.GetString("logout"),
                this.LocalizedStrings.GetString("logout_confirmation_body"),
                this.LocalizedStrings.GetString("yes"),
                this.LocalizedStrings.GetString("no"));

            if (result)
            {
                await this.ExecuteBlockingUIAsync(
                    async () =>
                    {
                        // Logout
                        await this._usersService.LogoutAsync();

                        await this.SetMainPageNavPageAsync<LoginViewModel>();
                    });
            }
        }
    }
}
