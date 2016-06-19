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

                bool reverse = false;
                if (parameter is string)
                {
                    reverse = bool.Parse(parameter as string);
                }

                bool result = (valueInt > 0);
                if(reverse)
                {
                    return !result;
                }
                else
                {
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
