using MyStyleApp.Models;
using MyStyleApp.Enums;
using System;
using System.Collections.Generic;

namespace MyStyleApp.Services.Backend
{
    public class EstablishmentTypesService
    {
        private const string LOCALIZATION_TOKEN = "establishment_type_";
        private LocalizedStringsService _localizedStrings;
        private List<EstablishmentType> _establismentTypeList;

        public EstablishmentTypesService(
            LocalizedStringsService localizedStrings)
        {
            this._localizedStrings = localizedStrings;

            this._establismentTypeList = new List<EstablishmentType>();
            foreach (EstablishmentTypeEnum establishmentType in Enum.GetValues(typeof(EstablishmentTypeEnum)))
            {
                int id = (int)establishmentType;
                this._establismentTypeList.Add(new EstablishmentType()
                {
                    Id =id,
                    Name = localizedStrings.GetString(LOCALIZATION_TOKEN + id)
                }
                );
            }
        }

        public IList<EstablishmentType> GetEstablishmentTypes()
        {
            return this._establismentTypeList;
        }
    }
}
