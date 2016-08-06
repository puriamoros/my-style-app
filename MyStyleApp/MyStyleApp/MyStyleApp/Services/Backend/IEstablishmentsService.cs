using MyStyleApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStyleApp.Services.Backend
{
    public interface IEstablishmentsService
    {
        Task<List<Establishment>> SearchEstablishmentsAsync(
            Province province,
            Service service);

        Task<Establishment> GetEstablishmentAsync(int id);

        Task<List<Establishment>> GetOwnerEstablishmentsAsync();

        Task<Establishment> CreateEstablishmentAsync(Establishment establishment);

        Task UpdateEstablishmentAsync(Establishment establishment);

        Task UpdateEstablishmentServicesAsync(Establishment establishment);
    }       
}
