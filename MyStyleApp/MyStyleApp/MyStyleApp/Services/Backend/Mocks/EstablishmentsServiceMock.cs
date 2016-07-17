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
            List<ShortenService> services = new List<ShortenService>();
            services.Add(new ShortenService()
            {
                Id = 1,
                Price = 10.5f
            });

            IList<Establishment> listEstablishments = new List<Establishment>();

            listEstablishments.Add(new Establishment()
            {
                Id = 1,
                Name = "Establecimiento1",
                Address = "Dirección1",
                Phone = "951456587",
                IdEstablishmentType = 1,
                IdFavourite = 1,
                ShortenServices = services
            }
            );

            listEstablishments.Add(new Establishment()
            {
                Id = 2,
                Name = "Establecimiento2",
                Address = "Dirección2",
                Phone = "951456588",
                IdEstablishmentType = 2,
                IdFavourite = 0,
                ShortenServices = services
            }
            );

            listEstablishments.Add(new Establishment()
            {
                Id = 3,
                Name = "Establecimiento3",
                Address = "Dirección3",
                Phone = "951456589",
                IdEstablishmentType = 3,
                IdFavourite = 2,
                ShortenServices = services
            }
            );

            return listEstablishments;
        }

        public async Task<Establishment> GetEstablishmentAsync(int id)
        {
            List<ShortenService> services = new List<ShortenService>();
            services.Add(new ShortenService()
                {
                    Id = 1,
                    Price = 10.5f
                });

            Establishment establishment = new Establishment()
            {
                Id = 1,
                Name = "Establecimiento1",
                Address = "Dirección1",
                Phone = "951456587",
                IdEstablishmentType = 1,
                IdFavourite = 1,
                ShortenServices = services

            };

            return establishment;
        }

        public async Task<IList<Establishment>> GetMyEstablishmentsAsync()
        {
            IList<Establishment> listEstablishments = new List<Establishment>();

            listEstablishments.Add(new Establishment()
            {
                Id = 1,
                Name = "Establecimiento1",
                Address = "Dirección1",
                Phone = "951456587",
                IdEstablishmentType = 1
            }
            );

            listEstablishments.Add(new Establishment()
            {
                Id = 2,
                Name = "Establecimiento2",
                Address = "Dirección2",
                Phone = "951456588",
                IdEstablishmentType = 2
            }
            );

            listEstablishments.Add(new Establishment()
            {
                Id = 3,
                Name = "Establecimiento3",
                Address = "Dirección3",
                Phone = "951456589",
                IdEstablishmentType = 3
            }
            );

            return listEstablishments;
        }
    }
}
