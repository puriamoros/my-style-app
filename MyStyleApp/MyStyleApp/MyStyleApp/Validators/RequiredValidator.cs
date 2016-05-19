using MyStyleApp.Services;

namespace MyStyleApp.Validators
{
    public class RequiredValidator : IValidator
    {
        private const string STRING_ERROR_REQUIRED_FIELD = "error_required_field";
        private const string TOKEN_FIELD_NAME = "${FIELD_NAME}";
        private string _input;
        private string _fieldName;

        public RequiredValidator(
            string input,
            string fieldName)
        {
            this._input = input;
            this._fieldName = fieldName;
        }

        public bool Validate()
        {
            return !string.IsNullOrEmpty(this._input);
        }

        public string GetValidationError(LocalizedStringsService localizedStrings)
        {
            return localizedStrings.GetString(
                    STRING_ERROR_REQUIRED_FIELD, TOKEN_FIELD_NAME,
                    localizedStrings.GetString(this._fieldName));
        }
    }
}
