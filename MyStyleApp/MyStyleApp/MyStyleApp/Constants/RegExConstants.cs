namespace MyStyleApp.Constants
{
    public class RegexConstants
    {
        public const string NOT_INSECURE_CHARS = "^((?![<>]).)*$"; // '<' and '>' are insecure
        public const string EMAIL = @"^[A-Za-z][_A-Za-z0-9-]+(\.[_A-Za-z0-9-]+)*@([A-Za-z]([a-zA-Z0-9]\.|([a-zA-Z0-9][a-zA-Z0-9\-]*[a-zA-Z0-9]\.))+)[a-zA-Z]{2,4}$";
        public const string PHONE = @"^\d{9}$";
    }
}
