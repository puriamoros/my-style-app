using MyStyleApp.Services;
using System.Text.RegularExpressions;

namespace MyStyleApp.Validators
{
    class EqualValidator
    {
        private const string STRING_ERROR_INVALID_FIELD = "error_fields_not_equal";
        private const string TOKEN_FIELD_NAME_1 = "${FIELD_NAME_1}";
        private const string TOKEN_FIELD_NAME_2 = "${FIELD_NAME_2}";
        private string _input1;
        private string _input2;
        private string _fieldName1;
        private string _fieldName2;

        public EqualValidator(
            string input1,
            string input2,
            string fieldName1,
            string fieldName2)
        {
            this._input1 = input1;
            this._input2 = input2;
            this._fieldName1 = fieldName1;
            this._fieldName2 = fieldName2;
        }

        public bool Validate()
        {
            return this._input1.Equals(this._input2);
        }

        public string GetValidationError(LocalizedStringsService localizedStrings)
        {
            return localizedStrings.GetString(
                STRING_ERROR_INVALID_FIELD,
                TOKEN_FIELD_NAME_1, localizedStrings.GetString(this._fieldName1),
                TOKEN_FIELD_NAME_2, localizedStrings.GetString(this._fieldName2));
        }
    }
}
