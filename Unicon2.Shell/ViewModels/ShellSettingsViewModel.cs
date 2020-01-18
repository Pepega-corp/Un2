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
            this.ApplicationThemes = ThemeManager.AppThemes
                .Select(a => new ApplicationTheme
                {
                    Name = a.Name,
                    BorderColorBrush = a.Resources["BlackColorBrush"] as Brush,
                    ColorBrush = a.Resources["WhiteColorBrush"] as Brush
                })
                .ToList();

            // create accent colors list
            this.AccentColors = ThemeManager.Accents
                .Select(a => new AccentColor { Name = a.Name, ColorBrush = a.Resources["AccentColorBrush"] as Brush })
                .ToList();

            this.SelectedTheme = this.ApplicationThemes.FirstOrDefault(t => t.Name.Equals(Settings.Default.BaseTheme));
            this.SelectedAccentColor = this.AccentColors.FirstOrDefault(c => c.Name.Equals(Settings.Default.BaseColor));

            if (this.SelectedTheme == null)
                this.SelectedTheme = this.ApplicationThemes.FirstOrDefault();
            if (this.SelectedAccentColor == null)
                this.SelectedAccentColor = this.AccentColors.FirstOrDefault(c => c.Name.Equals("Cyan"));
        }

        public ShellSettingsViewModel(ITypesContainer containerProvider)
        {
            this._containerProvider = containerProvider;
        }

        private ILocalizerService LocalizerService
        {
            get
            {
                this._localizerService = this._localizerService ?? this._containerProvider.Resolve<ILocalizerService>();
                return this._localizerService;
            }
        }

        private IList<ApplicationTheme> applicationsThemes;

        /// <summary>
        /// List with application themes
        /// </summary>
        public IList<ApplicationTheme> ApplicationThemes
        {
            get { return this.applicationsThemes; }
            set { SetProperty(ref this.applicationsThemes, value); }
        }

        private IList<AccentColor> accentColors;

        /// <summary>
        /// List with accent colors
        /// </summary>
        public IList<AccentColor> AccentColors
        {
            get { return this.accentColors; }
            set { SetProperty(ref this.accentColors, value); }
        }

        private ApplicationTheme selectedTheme;

        /// <summary>
        /// The selected theme
        /// </summary>
        public ApplicationTheme SelectedTheme
        {
            get { return this.selectedTheme; }
            set
            {
                if (SetProperty(ref this.selectedTheme, value))
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
            get { return this.selectedAccentColor; }
            set
            {
                if (SetProperty(ref this.selectedAccentColor, value))
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
            get { return this.LocalizerService?.SupportedLanguages; }
        }

        /// <summary>
        /// The selected language
        /// </summary>
        public CultureInfo SelectedLanguage
        {
            get { return this.LocalizerService?.SelectedLanguage; }
            set
            {
                if (value != null && !ReferenceEquals(value, this.LocalizerService.SelectedLanguage))
                {
                    if (this.LocalizerService != null)
                        this.LocalizerService.SelectedLanguage = value;
                }
            }
        }
    }

}