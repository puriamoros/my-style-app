using System;
using System.Globalization;
using Xamarin.Forms;

namespace MyStyleApp.Converters
{
    public class NumberToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double numberValue = System.Convert.ToDouble(value);

            if(parameter != null)
            {
                return (numberValue == 0) ? "" : numberValue.ToString(parameter as string);
            }
            else
            {
                return (numberValue == 0) ? "" : value.ToString();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
