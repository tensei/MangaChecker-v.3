using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using MangaChecker.Data.Interfaces;
using MangaChecker.ViewModels.Properties;
using MaterialDesignColors;
using PropertyChanged;

namespace MangaChecker.ViewModels.ViewModels {
    [ImplementPropertyChanged]
    public class ThemeViewModel {
        private readonly IThemeHelper _themeHelper;

        public ThemeViewModel(IThemeHelper themeHelper) {
            _themeHelper = themeHelper;
            ApplyPrimaryCommand = new ActionCommand(s => ApplyPrimary((Swatch) s));
            ApplyAccentCommand = new ActionCommand(s => ApplyAccent((Swatch) s));
            ToggleBaseCommand = new ActionCommand(s => ApplyTheme((bool) s));
            SetupTheme();
        }
        
        public IEnumerable<Swatch> PrimaryColors => _themeHelper.Swatches();
        private IEnumerable<Swatch> AccentColors => _themeHelper.Swatches().Where(s=> s.IsAccented);

        public ICommand ApplyPrimaryCommand { get; }
        public ICommand ApplyAccentCommand { get; }

        public bool Theme { get; set; }

        public ICommand ToggleBaseCommand { get; }

        private void ApplyTheme(bool isDark) {
            _themeHelper.ChangeThemeTo(isDark).ConfigureAwait(false);
            Settings.Default.Theme = isDark;
            Settings.Default.Save();
        }
        private void ApplyPrimary(Swatch swatch) {
            _themeHelper.ChangePrimaryColorTo(swatch).ConfigureAwait(false);
            Settings.Default.Primary = swatch.Name;
            Settings.Default.Save();
        }

        private void ApplyAccent(Swatch swatch) {
            _themeHelper.ChangeAccentColorTo(swatch).ConfigureAwait(false);
            Settings.Default.Accents = swatch.Name;
            Settings.Default.Save();
        }

        public void SetupTheme() {
            var primary = Settings.Default.Primary;
            var accents = Settings.Default.Accents;
            ApplyPrimary(PrimaryColors.FirstOrDefault(s => s.Name == primary));
            ApplyAccent(AccentColors.FirstOrDefault(s => s.Name == accents));
            Theme = Settings.Default.Theme;
            ApplyTheme(Theme);
        }
    }
}