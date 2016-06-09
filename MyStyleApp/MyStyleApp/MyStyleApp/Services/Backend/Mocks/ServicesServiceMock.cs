using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyStyleApp.Models;
using MyStyleApp.Enums;

namespace MyStyleApp.Services.Backend.Mocks
{
    public class ServicesServiceMock : IServicesService
    {
        public async Task<IList<Service>> GetServicesAsync()
        {
            List<Service> listServices = new List<Service>();

            listServices.Add(new Service()
            {
                    Id = 1,
                    Name = "Peinar corto",
                    Type = ServiceCategoryEnum.HairStyles,
                    Duration = 30    
            }
            );
            listServices.Add(new Service()
            {
                Id = 7,
                Name = "Corte señora",
                Type = ServiceCategoryEnum.Haircuts,
                Duration = 30
            }
            );
            listServices.Add(new Service()
            {
                Id = 15,
                Name = "Mechas enteras bicolor",
                Type = ServiceCategoryEnum.ColouringAndStreaks,
                Duration = 60
            }
            );

            return listServices;
        }
    }
}
