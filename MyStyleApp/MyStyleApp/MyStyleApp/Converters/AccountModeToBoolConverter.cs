using MyStyleApp.Enums;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace MyStyleApp.Converters
{
    public class AccountModeToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is AccountModeEnum)
            {
                AccountModeEnum accountModeValue = (AccountModeEnum)value;

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
                    AccountModeEnum accountModeParam = AccountModeEnum.View;
                    Enum.TryParse(parameterStr, out accountModeParam);
                    if (accountModeValue == accountModeParam)
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
