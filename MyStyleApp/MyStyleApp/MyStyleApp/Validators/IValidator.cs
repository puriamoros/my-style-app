using MyStyleApp.Services;
using System;
namespace MyStyleApp.Validators
{
    public interface IValidator
    {
        bool Validate();
        string GetValidationError(LocalizedStringsService localizedStrings);
    }
}
