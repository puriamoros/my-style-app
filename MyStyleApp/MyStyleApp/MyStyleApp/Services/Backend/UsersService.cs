using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyStyleApp.Models;
using System.Net.Http;
using MyStyleApp.Constants;

namespace MyStyleApp.Services.Backend
{
    public class UsersService : BackendServiceBase, IUsersService
    {
        public UsersService(HttpService httpService) :
            base(httpService)
        {
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
            await this.HttpService.DeleteApiKeyAuthorizationAsync();
            this.LoggedUser = null;
        }

        public async Task<User> MeAsync()
        {
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

            return LoggedUser;
        }
    }
}
