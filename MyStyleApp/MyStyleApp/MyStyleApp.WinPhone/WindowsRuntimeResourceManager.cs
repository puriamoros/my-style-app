using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace MyStyleApp.WinPhone
{
    // Class to workarond the MissingManifestResourceException in WinPhone
    // Reference: https://blogs.msdn.microsoft.com/philliphoff/2014/11/19/missingmanifestresourceexception-when-using-portable-class-libraries-within-winrt/
    public class WindowsRuntimeResourceManager : ResourceManager
    {

        private readonly ResourceLoader _resourceLoader;

        private WindowsRuntimeResourceManager(string baseName, Assembly assembly) : base(baseName, assembly)
        {
            System.Diagnostics.Debug.WriteLine(baseName);
            this._resourceLoader = ResourceLoader.GetForViewIndependentUse(baseName);
        }

        public static void InjectInto(Type typeWithResourceManager, string resourceManagerFieldName, string resourceName)
        {
            typeWithResourceManager.GetRuntimeFields()
              .First(m => m.Name == resourceManagerFieldName)
              .SetValue(null, new WindowsRuntimeResourceManager(resourceName, typeWithResourceManager.GetTypeInfo().Assembly));
        }

        public override string GetString(string name, CultureInfo culture)
        {
            string value = this._resourceLoader.GetString(name);

            // Need to return null in case of "" because PCL ResourceManager expect that for non existing name
            return (value.Length > 0) ? value : null;
        }
    }
}
