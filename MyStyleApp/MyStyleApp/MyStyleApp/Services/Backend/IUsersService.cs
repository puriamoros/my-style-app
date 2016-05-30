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
    }
}
