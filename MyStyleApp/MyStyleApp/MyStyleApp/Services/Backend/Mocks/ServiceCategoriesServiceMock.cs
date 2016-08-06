using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyStyleApp.Models;
using MyStyleApp.Enums;

namespace MyStyleApp.Services.Backend.Mocks
{
    public class ServiceCategoriesServiceMock : IServiceCategoriesService
    {
        public async Task<List<ServiceCategory>> GetServiceCategoriesAsync()
        {
            List<ServiceCategory> listServiceCateogries = new List<ServiceCategory>();

            listServiceCateogries.Add(new ServiceCategory()
            {
                Id = 1,
                Name = "Peluquería",
                EstablishmentType = EstablishmentTypeEnum.Hairdresser
            }
            );
            listServiceCateogries.Add(new ServiceCategory()
            {
                Id = 2,
                Name = "Cortes",
                EstablishmentType = EstablishmentTypeEnum.Hairdresser
            }
            );
            listServiceCateogries.Add(new ServiceCategory()
            {
                Id = 3,
                Name = "Coloración y mechas",
                EstablishmentType = EstablishmentTypeEnum.Hairdresser
            }
            );

            return listServiceCateogries;
        }
    }
}
