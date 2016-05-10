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
                return !String.IsNullOrEmpty(valueStr);
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
