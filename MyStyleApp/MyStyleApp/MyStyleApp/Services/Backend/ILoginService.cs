using MyStyleApp.Models;
using System.Threading.Tasks;

namespace MyStyleApp.Services.Backend
{
    public interface ILoginService
    {
        Task Login(string email, string password, bool rememberLogin);
        Task Logout();
    }
}
