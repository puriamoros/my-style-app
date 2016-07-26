using System;
using System.Globalization;
using Xamarin.Forms;

namespace MyStyleApp.Converters
{
    class ObjectToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = value != null;

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

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
