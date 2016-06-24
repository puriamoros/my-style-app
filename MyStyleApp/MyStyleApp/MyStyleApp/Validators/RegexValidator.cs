using MyStyleApp.Services;
using System.Text.RegularExpressions;

namespace MyStyleApp.Validators
{
    class RegexValidator : IValidator
    {
        private const string TOKEN_FIELD_NAME = "${FIELD_NAME}";
        private string _input;
        private string _regex;
        private string _errorTokenWithFieldName;
        private string _fieldName;

        public RegexValidator(
            string input,
            string regex,
            string errorTokenWithFieldName,
            string fieldName)
        {
            this._input = input;
            this._regex = regex;
            this._errorTokenWithFieldName = errorTokenWithFieldName;
            this._fieldName = fieldName;
        }

        public bool Validate()
        {
            return Regex.IsMatch(this._input, this._regex);
        }

        public string GetValidationError(LocalizedStringsService localizedStrings)
        {
            return localizedStrings.GetString(
                this._errorTokenWithFieldName, TOKEN_FIELD_NAME,
                localizedStrings.GetString(this._fieldName));
        }
    }
}
