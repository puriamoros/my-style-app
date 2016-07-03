using MyStyleApp.Enums;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace MyStyleApp.Converters
{
    public class AppointmentStatusToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is AppointmentStatusEnum)
            {
                AppointmentStatusEnum appointmentStatusValue = (AppointmentStatusEnum)value;

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
                    AppointmentStatusEnum appointmentStatusParam = AppointmentStatusEnum.Pending;
                    Enum.TryParse(parameterStr, out appointmentStatusParam);
                    if (appointmentStatusValue == appointmentStatusParam)
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
