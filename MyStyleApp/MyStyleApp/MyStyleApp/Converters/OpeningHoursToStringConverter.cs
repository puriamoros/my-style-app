using MyStyleApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;

namespace MyStyleApp.Converters
{
    class OpeningHoursToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                string valueStr = value as string;

                if (!string.IsNullOrEmpty(valueStr))
                {
                    // Split "10:00-14:00" into "10:00" and "14:00"
                    var splitInterval = valueStr.Split(new char[] { '-' });
                    if (splitInterval.Length == 2)
                    {
                        List<OpeningHour> openingHours = new List<OpeningHour>(2);
                        foreach (string hourStr in splitInterval)
                        {
                            // Split "10:00" in "10" and "00"
                            var splitHour = hourStr.Split(new char[] { ':' });
                            if (splitHour.Length == 2)
                            {
                                int hour = 0;
                                int minute = 0;
                                try
                                {
                                    hour = int.Parse(splitHour[0]);
                                    minute = int.Parse(splitHour[1]);
                                }
                                catch (Exception)
                                {
                                    return "";
                                }
                                if (hour < 0 || hour > 23 || (minute != 0 && minute != 30))
                                {
                                    return "";
                                }

                                OpeningHour openingHour = new OpeningHour();
                                openingHour.Hour = hour;
                                openingHour.Minute = minute;
                                openingHours.Add(openingHour);
                            }
                        }

                        DateTime open = new DateTime(
                            DateTime.Today.Year,
                            DateTime.Today.Month,
                            DateTime.Today.Day,
                            openingHours[0].Hour,
                            openingHours[0].Minute,
                            0);
                        DateTime close = new DateTime(
                            DateTime.Today.Year,
                            DateTime.Today.Month,
                            DateTime.Today.Day,
                            openingHours[1].Hour,
                            openingHours[1].Minute,
                            0);
                        return open.ToString("t") + " - " + close.ToString("t");
                    }
                }
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
