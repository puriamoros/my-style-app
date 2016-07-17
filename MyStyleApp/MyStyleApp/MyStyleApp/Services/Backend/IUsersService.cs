using MyStyleApp.Models;
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
        Task UpdateUserAsync(int id, User user);
        Task UpdatePasswordAsync(int id, string oldPassword, string newPassword);
        Task UpdatePlatformAsync(int id, string platform, string pushToken);
    }
}
