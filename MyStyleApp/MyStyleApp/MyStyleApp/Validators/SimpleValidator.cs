using MyStyleApp.Services;
using System;

namespace MyStyleApp.Validators
{
    public class SimpleValidator : IValidator
    {
        private string _input;
        private Func<string, bool> _isValid;
        private string _error;

        public SimpleValidator(
            string input,
            Func<string, bool> isValid,
            string error)
        {
            this._input = input;
            this._isValid = isValid;
            this._error = error;
        }

        public bool Validate()
        {
            return this._isValid(this._input);
        }

        public string GetValidationError(LocalizedStringsService localizedStrings)
        {
            return localizedStrings.GetString( this._error);
        }
    }
}
