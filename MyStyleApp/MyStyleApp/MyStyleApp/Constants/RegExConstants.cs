namespace MyStyleApp.Constants
{
    public class RegexConstants
    {
        public const string INSECURE_CHARS = "((<+)|(>+))";
        public const string EMAIL = @"^[A-Za-z][_A-Za-z0-9-]+(\.[_A-Za-z0-9-]+)*@([A-Za-z]([a-zA-Z0-9]\.|([a-zA-Z0-9][a-zA-Z0-9\-]*[a-zA-Z0-9]\.))+)[a-zA-Z]{2,4}$";
    }
}
