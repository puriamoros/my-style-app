using MyStyleApp.Models;
using System;
using System.Threading.Tasks;

namespace MyStyleApp.Services.Backend.Mocks
{
    public class UsersServiceMock : IUsersService
    {
        public User LoggedUser { get; private set; }

        public async Task Login(string email, string password, bool rememberLogin)
        {
            await Task.Delay(3000);

            if(!email.Equals("puri.amoros@gmail.com", StringComparison.CurrentCultureIgnoreCase) ||
                password != "puri")
            {
                throw new Exception("User or password does not match.");
            }

            this.LoggedUser = new User()
            {
                Name = "Puri",
                Surname = "Amoros",
                Email = "puri.amoros@gmail.com"
            };
        }

        public void Logout()
        {
            this.LoggedUser = null;
        }

        public async Task<User> Me()
        {
            await Task.Delay(1000);
            this.LoggedUser = new User()
            {
                Name = "Puri",
                Surname = "Amoros",
                Email = "puri.amoros@gmail.com"
            };
            return LoggedUser;
        }
    }
}
