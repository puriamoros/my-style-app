using MyStyleApp.Models;
using MyStyleApp.Enums;
using System;
using System.Collections.Generic;

namespace MyStyleApp.Services.Backend
{
    public class ProvincesService
    {
        private const string LOCALIZATION_TOKEN = "province_";
        private LocalizedStringsService _localizedStrings;
        private List<Province> _provinceList;

        public ProvincesService(
            LocalizedStringsService localizedStrings)
        {
            this._localizedStrings = localizedStrings;

            this._provinceList = new List<Province>();
            foreach (ProvinceEnum province in Enum.GetValues(typeof(ProvinceEnum)))
            {
                int id = (int)province;
                this._provinceList.Add(new Province()
                {
                    Id = id,
                    Name = localizedStrings.GetString(LOCALIZATION_TOKEN + id)
                }
                );
            }
        }

        public IList<Province> GetProvinces()
        {
            return this._provinceList;
        }
    }
}
