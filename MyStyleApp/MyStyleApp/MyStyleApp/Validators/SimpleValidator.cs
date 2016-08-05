using MyStyleApp.Services;
using System;

namespace MyStyleApp.Validators
{
    public class SimpleValidator<T> : IValidator
    {
        private T _input;
        private Func<T, bool> _isValid;
        private string _error;

        public SimpleValidator(
            T input,
            Func<T, bool> isValid,
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
            return this._error;
        }
    }
}
