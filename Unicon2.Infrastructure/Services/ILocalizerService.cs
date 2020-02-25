using System.Collections.Generic;
using System.Globalization;

namespace Unicon2.Infrastructure.Services
{
    public interface ILocalizerService
    {
        /// <summary>
        /// Set localization
        /// </summary>
        /// <param name="locale"></param>
        void SetLocale(string locale);

        /// <summary>
        /// Set localization
        /// </summary>
        /// <param name="culture"></param>
        void SetLocale(CultureInfo culture);

        /// <summary>
        /// Get a localized string by key
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns></returns>
        string GetLocalizedString(string key);


        /// <summary>
        /// Get a localized string by key
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns></returns>
        bool TryGetLocalizedString(string key, out string result);


        /// <summary>
        /// Supported languages
        /// </summary>
        IList<CultureInfo> SupportedLanguages { get; }

        /// <summary>
        /// The current selected language
        /// </summary>
        CultureInfo SelectedLanguage { get; set; }
    }
}