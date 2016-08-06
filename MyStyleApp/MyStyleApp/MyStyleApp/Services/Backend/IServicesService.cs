using MyStyleApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyStyleApp.Services.Backend
{
    public interface IServicesService
    {
        Task<List<Service>> GetServicesAsync();
    }
}
