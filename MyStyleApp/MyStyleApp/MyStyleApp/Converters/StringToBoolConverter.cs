using System;
using System.Globalization;
using Xamarin.Forms;

namespace MyStyleApp.Converters
{
    public class StringToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is string)
            {
                string valueStr = value as string;
                bool result = !String.IsNullOrEmpty(valueStr);

                bool negate = false;
                if (parameter is string)
                {
                    string parameterStr = parameter as string;
                    if (parameterStr.StartsWith("!"))
                    {
                        negate = true;
                    }
                }
                if (negate)
                {
                    result = !result;
                }

                return result;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
