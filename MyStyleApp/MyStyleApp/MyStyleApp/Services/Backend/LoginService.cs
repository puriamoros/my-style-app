using MyStyleApp.Constants;
using MyStyleApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MyStyleApp.Services.Backend
{
    public class LoginService : ILoginService
    {
        private HttpService _httpService;

        public LoginService(HttpService httpService)
        {
            this._httpService = httpService;
        }

        public async Task Login(string email, string password, bool rememberLogin)
        {
            ApiKey apiKey = await this._httpService.Invoke<ApiKey>(
                HttpMethod.Get,
                BackendConstants.LOGIN_URL,
                this._httpService.GetBasicAuthorization(email, password),
                null);

            await this._httpService.SaveApiKeyAuthorization(apiKey.Value, rememberLogin);
        }

        public async Task Logout()
        {
            await this._httpService.DeleteApiKeyAuthorization();
        }
    }
}
