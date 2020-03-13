using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using MahApps.Metro;
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
            // create metro theme color menu items for the demo
            ApplicationThemes = ThemeManager.AppThemes
                .Select(a => new ApplicationTheme
                {
                    Name = a.Name,
                    BorderColorBrush = a.Resources["BlackColorBrush"] as Brush,
                    ColorBrush = a.Resources["WhiteColorBrush"] as Brush
                })
                .ToList();

            // create accent colors list
            AccentColors = ThemeManager.Accents
                .Select(a => new AccentColor { Name = a.Name, ColorBrush = a.Resources["AccentColorBrush"] as Brush })
                .ToList();

            SelectedTheme = ApplicationThemes.FirstOrDefault(t => t.Name.Equals(Settings.Default.BaseTheme));
            SelectedAccentColor = AccentColors.FirstOrDefault(c => c.Name.Equals(Settings.Default.BaseColor));

            if (SelectedTheme == null)
                SelectedTheme = ApplicationThemes.FirstOrDefault();
            if (SelectedAccentColor == null)
                SelectedAccentColor = AccentColors.FirstOrDefault(c => c.Name.Equals("Cyan"));
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

        private IList<ApplicationTheme> applicationsThemes;

        /// <summary>
        /// List with application themes
        /// </summary>
        public IList<ApplicationTheme> ApplicationThemes
        {
            get { return applicationsThemes; }
            set { SetProperty(ref applicationsThemes, value); }
        }

        private IList<AccentColor> accentColors;

        /// <summary>
        /// List with accent colors
        /// </summary>
        public IList<AccentColor> AccentColors
        {
            get { return accentColors; }
            set { SetProperty(ref accentColors, value); }
        }

        private ApplicationTheme selectedTheme;

        /// <summary>
        /// The selected theme
        /// </summary>
        public ApplicationTheme SelectedTheme
        {
            get { return selectedTheme; }
            set
            {
                if (SetProperty(ref selectedTheme, value))
                {
                    var theme = ThemeManager.DetectAppStyle(Application.Current);
                    var appTheme = ThemeManager.GetAppTheme(value.Name);
                    ThemeManager.ChangeAppStyle(Application.Current, theme.Item2, appTheme);
                    Settings.Default.BaseTheme = value.Name;
                    Settings.Default.Save();
                }
            }
        }

        private AccentColor selectedAccentColor;

        /// <summary>
        /// Selected accent color
        /// </summary>
        public AccentColor SelectedAccentColor
        {
            get { return selectedAccentColor; }
            set
            {
                if (SetProperty(ref selectedAccentColor, value))
                {
                    var theme = ThemeManager.DetectAppStyle(Application.Current);
                    var accent = ThemeManager.GetAccent(value.Name);

                    ThemeManager.ChangeAppStyle(Application.Current, accent, theme.Item1);
                    Settings.Default.BaseColor = value.Name;
                    Settings.Default.Save();
                }
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