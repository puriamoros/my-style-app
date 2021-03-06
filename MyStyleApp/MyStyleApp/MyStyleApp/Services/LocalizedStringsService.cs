﻿using System;
using System.Resources;
using System.Globalization;
using System.Reflection;
using System.Collections.Generic;

namespace MyStyleApp.Services
{
    // This class is an alternative to Xamarin stardard localization.
    //
    // To use it, just add it to your ViewModels. Example
    // public LocalizedStringsService LocalizedStrings { get { return _localizedStringsService; } }
    //
    // To bind some text to your ui component, use a binding. Example:
    // Text="{Binding LocalizedStrings[put_the_string_key_inside_the_brackets]}"
    //
    // To manage long texts that are refused by makepri.exe, it is possible to split the long text in
    // smaller parts in the language files (Resources.resw). To do that, just split the text in some parts
    // and name the keys in the following way: "long_text_key_0", ..., "long_text_key_n". To use this feature
    // you must assure that no key named "long_text_key" exists or it has an empty value.
    public class LocalizedStringsService
    {
        private const string LOCALIZED_STRINGS_RESOURCE_ID = "MyStyleApp.Localization.LocalizedStrings";
        private const string NON_LOCALIZED_STRINGS_RESOURCE_ID = "MyStyleApp.Localization.NonLocalizedStrings";

        private ILocalizationService _localizationService;
        private CultureInfo _ci;
        private static ResourceManager _localizedRm;
        private static ResourceManager _nonLocalizedRm;

        public LocalizedStringsService(ILocalizationService localizationService)
        {
            this._localizationService = localizationService;
            this._ci = this._localizationService.GetCurrentCultureInfo();

            var assembly = typeof(LocalizedStringsService).GetTypeInfo().Assembly;
        }

        public string this[string key]
        {
            get
            {
                return this.GetString(key);
            }
        }

        private string GetRawString(string key)
        {
            // Late initilization needed for WinPhone ResourceManager hack (see WindowsRuntimeResourceManager)
            if (_localizedRm == null)
            {
                var assembly = typeof(LocalizedStringsService).GetTypeInfo().Assembly;
                _localizedRm = new ResourceManager(LOCALIZED_STRINGS_RESOURCE_ID, assembly);
            }
            if(_nonLocalizedRm == null)
            {
                var assembly = typeof(LocalizedStringsService).GetTypeInfo().Assembly;
                _nonLocalizedRm = new ResourceManager(NON_LOCALIZED_STRINGS_RESOURCE_ID, assembly);
            }

            // Try normal string
            string value = _localizedRm.GetString(key, _ci);

            if (value == null)
            {
                // Try multipart string: "key_i" ( 0 <= i < parts)
                int i = 0;
                string part = null;
                do
                {
                    part = _localizedRm.GetString(key + "_" + i, _ci);

                    if(part != null)
                    {
                        value = (value == null) ? part : value + part;
                    }
                    
                    i++;
                } while (part != null);

                if (value == null)
                {
                    // Try NonLocalized string
                    value = _nonLocalizedRm.GetString(key);
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

        public string GetString(string key,
            string token, string value)
        {
            List<KeyValuePair<string, string>> tokenPairs = new List<KeyValuePair<string, string>>();
            tokenPairs.Add(new KeyValuePair<string, string>(token, value));

            return this.GetString(key, tokenPairs);
        }

        public string GetString(string key,
            string token1, string value1,
            string token2, string value2)
        {
            List<KeyValuePair<string, string>> tokenPairs = new List<KeyValuePair<string, string>>();
            tokenPairs.Add(new KeyValuePair<string, string>(token1, value1));
            tokenPairs.Add(new KeyValuePair<string, string>(token2, value2));

            return this.GetString(key, tokenPairs);
        }

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
