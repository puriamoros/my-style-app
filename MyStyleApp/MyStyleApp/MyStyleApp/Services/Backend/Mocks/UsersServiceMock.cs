using MyStyleApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyStyleApp.Services.Backend.Mocks
{
    public class UsersServiceMock : IUsersService
    {
        public User LoggedUser { get; private set; }

        public async Task LoginAsync(string email, string password, bool rememberLogin)
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

        public async Task LogoutAsync()
        {
            await (Task.Delay(1000));
            this.LoggedUser = null;
        }

        public async Task<User> MeAsync()
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

        public async Task<ApiKey> RegisterUserAsync(User user)
        {
            ApiKey apiKey = new ApiKey();
            apiKey.Value = "lkajsdlkjsdlkkjfñaksdjfakjsdf";

            return apiKey;
        }

        public async Task UpdateUserAsync(User user)
        {
        }

        public async Task UpdatePasswordAsync(int id, string oldPassword, string newPassword)
        { 
        }

        public async Task UpdatePlatformAsync(UserPlatform userPlatform)
        {
        }

        public async Task<User> GetUserAsync(int idUser)
        {
            User user = new User();
            return user;
        }

        public async Task<List<Staff>> GetStaffAsync(Establishment establishment)
        {
            List<Staff> listStaff = new List<Staff>();

            listStaff.Add(new Staff()
            {
                Id = 1,
                Name = "Empleado1",
            }
            );

            listStaff.Add(new Staff()
            {
                Id = 2,
                Name = "Empleado2",
            }
            );

            listStaff.Add(new Staff()
            {
                Id = 3,
                Name = "Empleado3",
            }
            );

            return listStaff;
        }

        public async Task UpdateStaffAsync(Staff staff)
        {
        }

        public async Task<Staff> CreateStaffAsync(Staff staff)
        {
            return new Staff()
            {
                Id = 1,
                Name = "Empleado1",
            };
        }

        public async Task DeleteStaffAsync(Staff staff)
        {
        }
    }
}
