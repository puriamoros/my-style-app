using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyStyleApp.Models;

namespace MyStyleApp.Services.Backend.Mocks
{
    public class EstablishmentsServiceMock : IEstablishmentsService
    {
        public async Task<IList<Establishment>> GetEstablishmentsAsync(
            Province province,
            Service service)
        {
            IList<Establishment> listEstablishments = new List<Establishment>();

            listEstablishments.Add(new Establishment()
            {
                Id = 1,
                Name = "Establecimiento1",
                Address = "Dirección1",
                IdEstablishmentType = 1
            }
            );

            listEstablishments.Add(new Establishment()
            {
                Id = 2,
                Name = "Establecimiento2",
                Address = "Dirección2",
                IdEstablishmentType = 2
            }
            );

            listEstablishments.Add(new Establishment()
            {
                Id = 3,
                Name = "Establecimiento3",
                Address = "Dirección3",
                IdEstablishmentType = 3
            }
            );

            return listEstablishments;
        }
    }
}
