using System;
using Xamarin.Forms;
using System.Resources;
using System.Globalization;
using System.Reflection;
using System.Collections.Generic;

namespace MyStyleApp.Services
{
    /// <summary>
    /// This class is an alternative to x:Uid based localization.
    ///
    /// To use it, just add a StaticResource in your App.xaml pointing to to this class. Example:
    /// <LocalizedStringsServiceNS:LocalizedStringsService x:Key="LocalizedStrings" />
    ///
    /// To bind some text to your ui component, use a binding. Example:
    /// Text="{Binding [put_the_string_key_inside_the_brackets], Source={StaticResource StringResources}}"
    ///
    /// To manage long texts that are refused by makepri.exe, it is possible to split the long text in
    /// smaller parts in the language files (Resources.resw). To do that, just split the text in some parts
    /// and name the keys in the following way: "long_text_key_0", ..., "long_text_key_n". To use this feature
    /// you must assure that no key named "long_text_key" exists or it has an empty value.
    /// </summary>
    public class LocalizedStringsService
    {
        private const string LOCALIZED_STRINGS_RESOURCE_ID = "MyStyleApp.Localization.LocalizedStrings";
        private const string NON_LOCALIZED_STRINGS_RESOURCE_ID = "MyStyleApp.Localization.NonLocalizedStrings";

        private ILocalizationService _localizationService;
        private readonly CultureInfo _ci;
        private ResourceManager _rm;
        private ResourceManager _nonLocalizedRm;

        public LocalizedStringsService(/*ILocalizationService localizationService*/)
        {
            // Can not load ILocalizationService from dependency injection container because this class
            // Is referenced from App.xaml and App.xaml is loaded before App.xaml.cs calls Bootstrapper, so
            // dependency injection container has not been initialized
            this._localizationService = DependencyService.Get<ILocalizationService>();//localizationService;
            this._ci = this._localizationService.GetCurrentCultureInfo();

            var assembly = typeof(LocalizedStringsService).GetTypeInfo().Assembly;

            //foreach (var res in assembly.GetManifestResourceNames())
            //    System.Diagnostics.Debug.WriteLine("found resource: " + res);

            this._rm = new ResourceManager(LOCALIZED_STRINGS_RESOURCE_ID, assembly);
            this._nonLocalizedRm = new ResourceManager(NON_LOCALIZED_STRINGS_RESOURCE_ID, assembly);
        }

        /// <summary>
        /// Gets a string resource from the current language file as it is.
        /// </summary>
        /// <param name="key">The key of the string resource</param>
        /// <returns>
        /// The string resource or an empty string if the string resource does not exist
        /// </returns>
        public string this[string key]
        {
            get
            {
                return this.GetString(key);
            }
        }

        /// <summary>
        /// Gets a string resource from the current language file as it is.
        /// </summary>
        /// <param name="key">The key of the string resource</param>
        /// <returns>
        /// The string resource or an empty string if the string resource does not exist
        /// </returns>
        private string GetRawString(string key)
        {
            // Try normal string
            string value = this._nonLocalizedRm.GetString(key, _ci);

            if (value == null)
            {
                // Try multipart string: "key_i" ( 0 <= i < parts)
                int i = 0;
                string part = null;
                do
                {
                    part = this._rm.GetString(key + "_" + i, _ci);

                    if(part != null)
                    {
                        value = (value == null) ? part : value + part;
                    }
                    
                    i++;
                } while (part != null);

                if (value == null)
                {
                    // Try NonLocalized string
                    value = this._nonLocalizedRm.GetString(key);
                }
            }

            if (value == null)
            {
                // This should not happen, review resource files
#if DEBUG
                throw new ArgumentException(
                    String.Format("Key '{0}' was not found in resources for culture '{2}'.", key, _ci.Name),
                    "Text");
#else
                value = key; // HACK: returns the key, which GETS DISPLAYED TO THE USER
#endif
            }

            return value;
        }

        /// <summary>
        /// Gets a list of token pairs found in a string.
        /// It will look for tokens (format: ${another_string_resource_key})
        /// </summary>
        /// <param name="value">The base string</param>
        /// <returns>
        /// The list of token pairs
        /// </returns>
        private List<KeyValuePair<string, string>> GetTokenPairs(string value)
        {
            // Look for tokens (Format: ${...})
            List<string> tokens = this.GetTokens(value);

            List<KeyValuePair<string, string>> tokenPairs = new List<KeyValuePair<string, string>>();
            foreach (string token in tokens)
            {
                // Get token value
                string tokenKey = token.Substring(2, token.Length - 3);
                string tokenValue = this.GetString(tokenKey);

                // Add token and token value to list
                tokenPairs.Add(new KeyValuePair<string, string>(token, tokenValue));
            }

            return tokenPairs;
        }

        /// <summary>
        /// Compose a string with some given tokens.
        /// It will look for tokens (format: ${another_string_resource_key}) and replace the them
        /// </summary>
        /// <param name="value">The base string</param>
        /// <param name="tokenPairs">A list of string key-value pairs (key: token, value: value to replace the token with)</param>
        /// <returns>
        /// The composed string
        /// </returns>
        private string ComposeStringWithTokenPairs(string value, List<KeyValuePair<string, string>> tokenPairs)
        {
            string composed = value;
            foreach (KeyValuePair<string, string> tokenPair in tokenPairs)
            {
                // Replace token with token value
                composed = composed.Replace(tokenPair.Key, tokenPair.Value);
            }

            composed = composed.Replace("\\n", "\n");

            return composed;
        }

        /// <summary>
        /// Gets a string resource from the current language file composed with other string resources.
        /// It will look for tokens (format: ${another_string_resource_key}) and replace the them
        /// </summary>
        /// <param name="key">The key of the string resource</param>
        /// <returns>
        /// The composed string resource or an empty string if the string resource does not exist
        /// </returns>
        public string GetString(string key)
        {
            // Get value
            string value = this.GetRawString(key);

            // Get token pairs from language files
            List<KeyValuePair<string, string>> tokenPairs = GetTokenPairs(value);

            // Compose final value
            value = this.ComposeStringWithTokenPairs(value, tokenPairs);

            return value;
        }

        /// <summary>
        /// Gets a string resource from the current language file composed with some given tokens.
        /// It will look for tokens (format: ${another_string_resource_key}) and replace the them
        /// </summary>
        /// <param name="key">The key of the string resource</param>
        /// <param name="tokenPairs">A list of string key-value pairs (key: token, value: value to replace the token with)</param>
        /// <returns>
        /// The composed string resource or an empty string if the string resource does not exist
        /// </returns>
        public string GetString(string key, List<KeyValuePair<string, string>> tokenPairs)
        {
            string value = this.GetRawString(key);

            // Compose value with token pairs passed by argument
            value = ComposeStringWithTokenPairs(value, tokenPairs);

            // Get token pairs from language files
            tokenPairs = GetTokenPairs(value);

            // Compose final value
            value = this.ComposeStringWithTokenPairs(value, tokenPairs);

            return value;
        }

        /// <summary>
        /// Gets a string resource from the current language file composed with some given tokens.
        /// It will look for tokens (format: ${another_string_resource_key}) and replace the them
        /// </summary>
        /// <param name="key">The key of the string resource</param>
        /// <param name="token">A token</param>
        /// <param name="value">A value to replace the token with</param>
        /// <returns>
        /// The composed string resource or an empty string if the string resource does not exist
        /// </returns>
        public string GetString(string key,
            string token, string value)
        {
            List<KeyValuePair<string, string>> tokenPairs = new List<KeyValuePair<string, string>>();
            tokenPairs.Add(new KeyValuePair<string, string>(token, value));

            return this.GetString(key, tokenPairs);
        }

        /// <summary>
        /// Gets a string resource from the current language file composed with some given tokens.
        /// It will look for tokens (format: ${another_string_resource_key}) and replace the them
        /// </summary>
        /// <param name="key">The key of the string resource</param>
        /// <param name="token1">The first token</param>
        /// <param name="value1">The value to replace the first token with</param>
        /// <param name="token2">The second token</param>
        /// <param name="value2">The value to replace the second token with</param>
        /// <returns>
        /// The composed string resource or an empty string if the string resource does not exist
        /// </returns>
        public string GetString(string key,
            string token1, string value1,
            string token2, string value2)
        {
            List<KeyValuePair<string, string>> tokenPairs = new List<KeyValuePair<string, string>>();
            tokenPairs.Add(new KeyValuePair<string, string>(token1, value1));
            tokenPairs.Add(new KeyValuePair<string, string>(token2, value2));

            return this.GetString(key, tokenPairs);
        }

        /// <summary>
        /// Gets a string resource from the current language file composed with some given tokens.
        /// It will look for tokens (format: ${another_string_resource_key}) and replace the them
        /// </summary>
        /// <param name="key">The key of the string resource</param>
        /// <param name="token1">The first token</param>
        /// <param name="value1">The value to replace the first token with</param>
        /// <param name="token2">The second token</param>
        /// <param name="value2">The value to replace the second token with</param>
        /// <param name="token3">The third token</param>
        /// <param name="value3">The value to replace the third token with</param>
        /// <returns>
        /// The composed string resource or an empty string if the string resource does not exist
        /// </returns>
        public string GetString(string key,
            string token1, string value1,
            string token2, string value2,
            string token3, string value3)
        {
            List<KeyValuePair<string, string>> tokenPairs = new List<KeyValuePair<string, string>>();
            tokenPairs.Add(new KeyValuePair<string, string>(token1, value1));
            tokenPairs.Add(new KeyValuePair<string, string>(token2, value2));
            tokenPairs.Add(new KeyValuePair<string, string>(token3, value3));

            return this.GetString(key, tokenPairs);
        }

        private List<string> GetTokens(string value)
        {
            List<string> tokens = new List<string>();

            int start = 0;
            while (start >= 0)
            {
                start = value.IndexOf("${", start);
                if (start >= 0)
                {
                    int end = value.IndexOf("}", start + 2);
                    if (end >= 0)
                    {
                        string token = value.Substring(start, end - (start - 1));
                        if (token.Length >= 4 && !tokens.Contains(token))
                        {
                            tokens.Add(token);
                        }

                        start = end + 1;
                    }
                    else
                    {
                        start = -1;
                    }
                }
            }

            return tokens;
        }
    }
}
