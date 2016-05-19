using MyStyleApp.Validators;
using System;
using System.Collections.Generic;

namespace MyStyleApp.Services
{
    public class ValidationService
    {
        private LocalizedStringsService _localizedStrings;
        private IList<IValidator> _validators;

        public ValidationService(LocalizedStringsService localizedStrings)
        {
            this._localizedStrings = localizedStrings;
            this._validators = new List<IValidator>();
        }

        public void ClearValidators()
        {
            this._validators.Clear();
        }

        public void AddValidator(IValidator validator)
        {
            this._validators.Add(validator);
        }

        public string GetValidationError()
        {
            string error = null;
            foreach(IValidator validator in this._validators)
            {
                if(!validator.Validate())
                {
                    return validator.GetValidationError(this._localizedStrings);
                }
            }
            return error;
        }
    }
}
