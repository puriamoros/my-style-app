using MyStyleApp.Models;
using System;
using System.Threading.Tasks;

namespace MyStyleApp.Services.Backend.Mocks
{
    public class LoginServiceMock : ILoginService
    {
        public async Task Login(string email, string password, bool rememberLogin)
        {
            await Task.Delay(3000);

            if(!email.Equals("puri.amoros@gmail.com", StringComparison.CurrentCultureIgnoreCase) ||
                password != "puri")
            {
                throw new Exception("User or password does not match.");
            }
        }

        public async Task Logout()
        {
            await Task.Delay(1000);
        }
    }
}
