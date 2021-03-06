﻿using System;
using System.Globalization;
using Xamarin.Forms;

namespace MyStyleApp.Converters
{
    public class NegateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                bool valueBool = (bool)value;

                return !valueBool;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
