using MyStyleApp.Services;

namespace MyStyleApp.Validators
{
    public class LengthValidator : IValidator
    {
        private const string STRING_ERROR_FIELD_LENGTH = "error_field_length";
        private const string STRING_ERROR_FIELD_LENGTH_MIN = "error_field_length_min";
        private const string STRING_ERROR_FIELD_LENGTH_MAX = "error_field_length_max";
        private const string STRING_ERROR_FIELD_LENGTH_RANGE = "error_field_length_range";
        private const string TOKEN_FIELD_NAME = "${FIELD_NAME}";
        private const string TOKEN_FIELD_LENGTH = "${FIELD_LENGTH}";
        private const string TOKEN_FIELD_LENGTH_MIN = "${FIELD_LENGTH_MIN}";
        private const string TOKEN_FIELD_LENGTH_MAX = "${FIELD_LENGTH_MAX}";
        private string _input;
        private string _fieldName;
        private int _lengthMin;
        private int _lengthMax;

        public LengthValidator(
            string input,
            string fieldName,
            int lengthMin,
            int lengthMax)
        {
            this._input = input;
            this._fieldName = fieldName;
            this._lengthMin = lengthMin;
            this._lengthMax = lengthMax;
        }

        public bool Validate()
        {
            if(this._lengthMin <= 0 && this._lengthMax <= 0)
            {
                return true;
            }

            if (this._lengthMin == this._lengthMax)
            {
                return this._input.Length == this._lengthMin;
            }

            if (this._lengthMin > 0 && this._lengthMax > 0)
            {
                return this._input.Length >= this._lengthMin &&
                    this._input.Length <= this._lengthMax;
            }

            if (this._lengthMin > 0)
            {
                return this._input.Length >= this._lengthMin;
            }

            //if (this._lengthMax > 0)
            return this._input.Length <= this._lengthMax;
        }

        public string GetValidationError(LocalizedStringsService localizedStrings)
        {
            if (this._lengthMin <= 0 && this._lengthMax <= 0)
            {
                return "";
            }

            if (this._lengthMin == this._lengthMax)
            {
                return localizedStrings.GetString(
                    STRING_ERROR_FIELD_LENGTH, TOKEN_FIELD_NAME,
                    localizedStrings.GetString(this._fieldName),
                    TOKEN_FIELD_LENGTH, this._lengthMin.ToString());
            }

            if (this._lengthMin > 0 && this._lengthMax > 0)
            {
                return localizedStrings.GetString(
                    STRING_ERROR_FIELD_LENGTH_RANGE, TOKEN_FIELD_NAME,
                    localizedStrings.GetString(this._fieldName),
                    TOKEN_FIELD_LENGTH_MIN, this._lengthMin.ToString(),
                    TOKEN_FIELD_LENGTH_MAX, this._lengthMax.ToString());
            }

            if (this._lengthMin > 0)
            {
                return localizedStrings.GetString(
                    STRING_ERROR_FIELD_LENGTH_MIN, TOKEN_FIELD_NAME,
                    localizedStrings.GetString(this._fieldName),
                    TOKEN_FIELD_LENGTH_MIN, this._lengthMin.ToString());
            }

            //if (this._lengthMax > 0)
            return localizedStrings.GetString(
                    STRING_ERROR_FIELD_LENGTH_MAX, TOKEN_FIELD_NAME,
                    localizedStrings.GetString(this._fieldName),
                    TOKEN_FIELD_LENGTH_MAX, this._lengthMax.ToString());

        }
    }
}
