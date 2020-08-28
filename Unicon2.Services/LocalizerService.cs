using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Unicon2.Infrastructure.Services;
using Unicon2.Services.Properties;
using WPFLocalizeExtension.Engine;
using WPFLocalizeExtension.Extensions;

namespace Unicon2.Services
{
    public class LocalizerService : ILocalizerService
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="culture"></param>
        public LocalizerService(string culture)
        {
            SupportedLanguages = CultureInfo.GetCultures(CultureTypes.AllCultures)
                .Where(c => c.IetfLanguageTag.Equals("ru-RU") || c.IetfLanguageTag.Equals("en-US")).ToList();
            SetLocale(culture);

          //  CultureInfo selectedLanguage = SupportedLanguages.FirstOrDefault(t => t.Name == Settings.Default.Language);

            //if (selectedLanguage != null)
           // {
           //     SelectedLanguage = selectedLanguage;
          //  }
        }

        /// <summary>
        /// Set localization
        /// </summary>
        /// <param name="locale"></param>
        public void SetLocale(string locale)
        {
            LocalizeDictionary.Instance.Culture = CultureInfo.GetCultureInfo(locale);
        }

        /// <summary>
        /// Set localization
        /// </summary>
        /// <param name="culture"></param>
        public void SetLocale(CultureInfo culture)
        {
            LocalizeDictionary.Instance.Culture = culture;
           // Settings.Default.Language = culture.Name;
           // Settings.Default.Save();
        }

        /// <summary>
        /// Get localized string from resource dictionary
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetLocalizedString(string key)
        {
            LocExtension locExtension = new LocExtension(key);
            if (locExtension.ResolveLocalizedValue(out string uiString))
                return uiString;
            return key;
            //var assemblyProp = ResxLocalizationProvider.Instance.FallbackAssembly;
            //var uiString = LocExtension.GetLocalizedValue<string>(assemblyProp + ":Resources:" + key);
            //return uiString;
        }

        public bool TryGetLocalizedString(string key, out string result)
        {
            LocExtension locExtension = new LocExtension(key);
            return locExtension.ResolveLocalizedValue(out result);
        }

        /// <summary>
        /// List with supported languages
        /// </summary>
        public IList<CultureInfo> SupportedLanguages { get; private set; }

        /// <summary>
        /// The current selected language
        /// </summary>
        public CultureInfo SelectedLanguage
        {
            get { return LocalizeDictionary.Instance.Culture; }
            set
            {
                SetLocale(value);

            }
        }
    }
}