using System;
using System.Globalization;
using Xamarin.Forms;

namespace MyStyleApp.Converters
{    
    public class PlatformToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
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
                if(Device.OS.ToString().Contains(parameterStr))
                {
                    result = true;
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
