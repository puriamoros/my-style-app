using MyStyleApp.Models;
using System.Threading.Tasks;

namespace MyStyleApp.Services.Backend
{
    public interface IUsersService
    {
        User LoggedUser { get; }
        Task Login(string email, string password, bool rememberLogin);
        void Logout();
        Task<User> Me();
    }
}
