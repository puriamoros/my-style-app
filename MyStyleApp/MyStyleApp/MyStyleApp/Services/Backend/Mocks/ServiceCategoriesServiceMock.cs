using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyStyleApp.Models;

namespace MyStyleApp.Services.Backend.Mocks
{
    public class ServiceCategoriesServiceMock : IServiceCategoriesService
    {
        public async Task<IList<ServiceCategory>> GetServiceCategoriesAsync()
        {
            List<ServiceCategory> listServiceCateogries = new List<ServiceCategory>();

            listServiceCateogries.Add(new ServiceCategory()
            {
                Id = 1,
                Name = "Peluquería",
                IdEstablishmentType = 1
            }
            );
            listServiceCateogries.Add(new ServiceCategory()
            {
                Id = 2,
                Name = "Cortes",
                IdEstablishmentType = 1
            }
            );
            listServiceCateogries.Add(new ServiceCategory()
            {
                Id = 3,
                Name = "Coloración y mechas",
                IdEstablishmentType = 1
            }
            );

            return listServiceCateogries;
        }
    }
}
