using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyStyleApp.Models;
using System.Net.Http;
using MyStyleApp.Constants;
using Xamarin.Forms;
using MyStyleApp.Enums;

namespace MyStyleApp.Services.Backend
{
    public class UsersService : BackendServiceBase, IUsersService
    {
        private IPushNotificationsService _pushNotificationsService;
        private LocalizedStringsService _localizedStringsService;


        public UsersService(
            HttpService httpService,
            LocalizedStringsService localizedStringsService,
            IPushNotificationsService pushNotificationsService) :
            base(httpService)
        {
            this._localizedStringsService = localizedStringsService;
            this._pushNotificationsService = pushNotificationsService;
            MessagingCenter.Subscribe<string>(this, "pushNotificationTokenReceived", this.OnPushNotificationTokenReceived);
        }

        private async void OnPushNotificationTokenReceived(string token)
        {
            if(this.LoggedUser != null)
            {
                try
                {
                    // Create user<->platform association
                    UserPlatform userPlatform = new UserPlatform()
                    {
                        Platform = Device.OS.ToString(),
                        PushToken = token,
                        LanguageCode = this._localizedStringsService.GetString("language_code")
                    };
                    await this.UpdatePlatformAsync(userPlatform);
                }
                catch (Exception)
                {
                    System.Diagnostics.Debug.WriteLine("Error while updating platform");
                }
            }
        }

        public User LoggedUser { get; private set; }

        public async Task LoginAsync(string email, string password, bool rememberLogin)
        {
            ApiKey apiKey = await this.HttpService.InvokeAsync<ApiKey>(
                HttpMethod.Get,
                BackendConstants.LOGIN_URL,
                this.HttpService.GetBasicAuthorization(email, password),
                null);
            await this.HttpService.SaveApiKeyAuthorizationAsync(apiKey.Value, rememberLogin);
            await this.MeAsync();
        }

        public async Task LogoutAsync()
        {
            UserTypeEnum userType = this.LoggedUser.UserType;

            // Delete user<->platform association
            UserPlatform userPlatform = new UserPlatform()
            {
                Platform = "",
                PushToken = "",
                LanguageCode = ""
            };
            await this.UpdatePlatformAsync(userPlatform);

            await this.HttpService.DeleteApiKeyAuthorizationAsync();
            this.LoggedUser = null;

            MessagingCenter.Send<string>(userType.ToString(), "userLogout");
        }

        public async Task<User> MeAsync()
        {
            int oldLoggedUserId = (this.LoggedUser != null) ? this.LoggedUser.Id : - 1;

            string apiKey = await this.HttpService.GetApiKeyAuthorizationAsync();
            if(apiKey == null)
            {
                throw new Exception("ApiKey not found. User is not logged in.");
            }

            this.LoggedUser = await this.HttpService.InvokeAsync<User>(
                HttpMethod.Get,
                BackendConstants.ME_URL,
                apiKey,
                null);

            if(oldLoggedUserId != this.LoggedUser.Id)
            {
                MessagingCenter.Send<string>(this.LoggedUser.UserType.ToString(), "userLogin");

                this._pushNotificationsService.RegisterDevice();
            }

            return LoggedUser;
        }

        public async Task<ApiKey> RegisterUserAsync(User user)
        {
            ApiKey apiKey = await this.HttpService.InvokeWithContentAsync<ApiKey, User>(
                HttpMethod.Post,
                BackendConstants.REGISTER_URL,
                null,
                user,
                null);

            return apiKey;
        }

        public async Task UpdateUserAsync(User user)
        {
            string apiKey = await this.HttpService.GetApiKeyAuthorizationAsync();
            await this.HttpService.InvokeWithContentAsync<User>(
                HttpMethod.Put,
                BackendConstants.USER_URL,
                apiKey,
                user,
                new object[] { this.LoggedUser.Id });
        }

        public async Task UpdatePasswordAsync(int id, string oldPassword, string newPassword)
        {
            UserPassword userPassword = new UserPassword();
            userPassword.Value = newPassword;

            string authorization = this.HttpService.GetBasicAuthorization(this.LoggedUser.Email, oldPassword);
            await this.HttpService.InvokeWithContentAsync<UserPassword>(
                HttpMethod.Put,
                BackendConstants.PASSWORD_URL,
                authorization,
                userPassword,
                new object[] { id });
        }

        public async Task UpdatePlatformAsync(UserPlatform userPlatform)
        {
            string authorization = await this.HttpService.GetApiKeyAuthorizationAsync();
            await this.HttpService.InvokeWithContentAsync<UserPlatform>(
                HttpMethod.Put,
                BackendConstants.PLATFORM_URL,
                authorization,
                userPlatform,
                new object[] { this.LoggedUser.Id });
        }

        public async Task<User> GetUserAsync(int idUser)
        {
            string authorization = await this.HttpService.GetApiKeyAuthorizationAsync();

            User user = await this.HttpService.InvokeAsync<User>(
                    HttpMethod.Get,
                    BackendConstants.USER_URL,
                    authorization,
                    new object[] { idUser });

            return user;
        }

        public async Task<IList<Staff>> GetStaffAsync(Establishment establishment)
        {
            string authorization = await this.HttpService.GetApiKeyAuthorizationAsync();

            IList<Staff> staffList = await this.HttpService.InvokeAsync<IList<Staff>>(
                    HttpMethod.Get,
                    BackendConstants.GET_STAFF_URL,
                    authorization,
                    new object[] { establishment.Id });

            return staffList;
        }

        public async Task UpdateStaffAsync(Staff staff)
        {
            string apiKey = await this.HttpService.GetApiKeyAuthorizationAsync();

            await this.HttpService.InvokeWithContentAsync<Staff>(
                HttpMethod.Put,
                BackendConstants.UPDATE_STAFF_URL,
                apiKey,
                staff,
                new object[] { staff.Id });
        }

        public async Task<Staff> CreateStaffAsync(Staff staff)
        {
            string apiKey = await this.HttpService.GetApiKeyAuthorizationAsync();

            return await this.HttpService.InvokeWithContentAsync<Staff, Staff>(
                HttpMethod.Post,
                BackendConstants.CREATE_STAFF_URL,
                apiKey,
                staff,
                null);        
        }

        public async Task DeleteStaffAsync(Staff staff)
        {
            string apiKey = await this.HttpService.GetApiKeyAuthorizationAsync();

            await this.HttpService.InvokeWithContentAsync<Staff>(
                HttpMethod.Delete,
                BackendConstants.DELETE_STAFF_URL,
                apiKey,
                staff,
                new object[] { staff.Id });
        }
    }
}
