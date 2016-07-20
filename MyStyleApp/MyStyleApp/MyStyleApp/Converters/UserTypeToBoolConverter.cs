using MyStyleApp.Enums;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace MyStyleApp.Converters
{
    public class UserTypeToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is UserTypeEnum)
            {
                UserTypeEnum userTypeValue = (UserTypeEnum)value;

                bool negate = false;
                if (parameter is string)
                {
                    string parameterStr = parameter as string;
                    if (parameterStr.StartsWith("!"))
                    {
                        negate = true;
                        parameterStr = parameterStr.Substring(1);
                    }

                    bool result = false;
                    UserTypeEnum userTypeParam = UserTypeEnum.Client;
                    Enum.TryParse(parameterStr, out userTypeParam);
                    if (userTypeValue == userTypeParam)
                    {
                        result = true;
                    }
                    if (negate)
                    {
                        result = !result;
                    }

                    return result;
                }
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
