using System;
using System.Globalization;
using Xamarin.Forms;

namespace MyStyleApp.Converters
{
    public class IntToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int)
            {
                int valueInt = (int)value;
                bool result = (valueInt > 0);

                bool reverse = false;
                if (parameter is string)
                {
                    reverse = bool.Parse(parameter as string);
                }
                if(reverse)
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
