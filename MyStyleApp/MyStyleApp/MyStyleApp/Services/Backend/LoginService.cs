using MyStyleApp.Constants;
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

        public async Task Login(string email, string password)
        {
            await this._httpService.Invoke(
                HttpMethod.Get,
                BackendConstants.LOGIN_URL,
                this._httpService.GetBasicAuthorization(email, password),
                null);
        }
    }
}
