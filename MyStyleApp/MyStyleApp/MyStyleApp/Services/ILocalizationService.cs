using System;
using System.Globalization;

namespace MyStyleApp.Services
{
    public interface ILocalizationService
    {
        CultureInfo GetCurrentCultureInfo();

        void SetLocale();
    }
}
