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

        public async Task Login(string email, string password, bool rememberLogin)
        {
            ApiKey apiKey = await this.HttpService.Invoke<ApiKey>(
                HttpMethod.Get,
                BackendConstants.LOGIN_URL,
                this.HttpService.GetBasicAuthorization(email, password),
                null);
            await this.HttpService.SaveApiKeyAuthorization(apiKey.Value, rememberLogin);
            await this.Me();
        }

        public async Task Logout()
        {
            await this.HttpService.DeleteApiKeyAuthorization();
            this.LoggedUser = null;
        }

        public async Task<User> Me()
        {
            string apiKey = await this.HttpService.GetApiKeyAuthorization();
            this.LoggedUser = await this.HttpService.Invoke<User>(
                HttpMethod.Get,
                BackendConstants.ME_URL,
                apiKey,
                null);
            return LoggedUser;
        }
    }
}
