using MyStyleApp.Enums;
using MyStyleApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStyleApp.Services.Backend
{
    public class ServiceCategoriesService
    {
        private const string LOCALIZATION_TOKEN = "service_category_";
        private LocalizedStringsService _localizedStrings;
        private List<ServiceCategory> _serviceCategoryList;

        public ServiceCategoriesService(
            LocalizedStringsService localizedStrings)
        {
            this._localizedStrings = localizedStrings;

            this._serviceCategoryList = new List<ServiceCategory>();
            foreach (ServiceCategoryEnum serviceCategory in Enum.GetValues(typeof(ServiceCategoryEnum)))
            {
                int id = (int)serviceCategory;
                this._serviceCategoryList.Add(new ServiceCategory()
                {
                    Id = id,
                    Name = localizedStrings.GetString(LOCALIZATION_TOKEN + id)
                }
                );
            }
        }

        public IList<ServiceCategory> GetServiceCategories()
        {
            return this._serviceCategoryList;
        }
    }
}
