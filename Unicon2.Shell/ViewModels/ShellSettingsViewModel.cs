using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Fluent;
using Unicon2.Infrastructure.Services;
using Unicon2.Shell.Models;
using Unicon2.Shell.Properties;
using Unicon2.Unity.Interfaces;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Shell.ViewModels
{
    public class ShellSettingsViewModel : ViewModelBase
    {
        private ILocalizerService _localizerService;
        private ITypesContainer _containerProvider;

        public void Initialize()
        {
         

        }

        public ShellSettingsViewModel(ITypesContainer containerProvider)
        {
            _containerProvider = containerProvider;
        }

        private ILocalizerService LocalizerService
        {
            get
            {
                _localizerService = _localizerService ?? _containerProvider.Resolve<ILocalizerService>();
                return _localizerService;
            }
        }

    


        /// <summary>
        /// Supported languages
        /// </summary>
        public IList<CultureInfo> SupportedLanguages
        {
            get { return LocalizerService?.SupportedLanguages; }
        }

        /// <summary>
        /// The selected language
        /// </summary>
        public CultureInfo SelectedLanguage
        {
            get { return LocalizerService?.SelectedLanguage; }
            set
            {
                if (value != null && !ReferenceEquals(value, LocalizerService.SelectedLanguage))
                {
                    if (LocalizerService != null)
                        LocalizerService.SelectedLanguage = value;
                }
            }
        }
    }

}