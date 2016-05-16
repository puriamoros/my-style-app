using MyStyleApp.Models;
using System.Threading.Tasks;

namespace MyStyleApp.Services.Backend
{
    public interface IUsersService
    {
        User LoggedUser { get; }
        Task Login(string email, string password, bool rememberLogin);
        Task Logout();
        Task<User> Me();
    }
}
