using MyStyleApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyStyleApp.Services.Backend
{
    public interface IUsersService
    {
        User LoggedUser { get; }
        Task LoginAsync(string email, string password, bool rememberLogin);
        Task LogoutAsync();
        Task<User> MeAsync();
        Task<ApiKey> RegisterUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task UpdatePasswordAsync(int id, string oldPassword, string newPassword);
        Task UpdatePlatformAsync(UserPlatform userPlatform);

        Task<IList<Staff>> GetStaffAsync(Establishment establishment);
        Task UpdateStaffAsync(Staff staff);
        Task<Staff> CreateStaffAsync(Staff staff);
        Task DeleteStaffAsync(Staff staff);
    }
}
