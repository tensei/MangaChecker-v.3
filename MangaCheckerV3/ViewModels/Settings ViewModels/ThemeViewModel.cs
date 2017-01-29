using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using MangaCheckerV3.Helpers;
using MangaCheckerV3.Properties;
using MaterialDesignColors;
using PropertyChanged;

namespace MangaCheckerV3.ViewModels.Settings_ViewModels {
    [ImplementPropertyChanged]
    public class ThemeViewModel {
        private Swatch _accentColor;
        private Swatch _primaryColor;
        private string _theme;

        public ThemeViewModel() {
            Instance = this;
            ApplyPrimaryCommand = new ActionCommand(s => ApplyPrimary((Swatch) s));
            ApplyAccentCommand = new ActionCommand(s => ApplyAccent((Swatch) s));
        }

        /// <summary>
        ///     Initializes a new instance of the ThemeViewModel class.
        /// </summary>
        public static ThemeViewModel Instance { get; private set; }

        public IEnumerable<Swatch> PrimaryColors => ThemeHelper.Swatches;
        public IEnumerable<Swatch> AccentColors => ThemeHelper.Accents;

        public ICommand ApplyPrimaryCommand { get; }
        public ICommand ApplyAccentCommand { get; }

        public string Theme {
            get { return _theme; }
            set {
                _theme = value;
                ThemeHelper.ChangeThemeTo(value == "Dark").ConfigureAwait(false);
                Settings.Default.Theme = value;
                Settings.Default.Save();
            }
        }

        private void ApplyPrimary(Swatch swatch) {
            ThemeHelper.ChangePrimaryColorTo(swatch).ConfigureAwait(false);
            Settings.Default.Primary = swatch.Name;
            Settings.Default.Save();
        }

        private void ApplyAccent(Swatch swatch) {
            ThemeHelper.ChangeAccentColorTo(swatch).ConfigureAwait(false);
            Settings.Default.Accents = swatch.Name;
            Settings.Default.Save();
        }

        public void SetupTheme() {
            var primary = Settings.Default.Primary;
            var accents = Settings.Default.Accents;
            var _theme = Settings.Default.Theme;
            ApplyPrimary(PrimaryColors.FirstOrDefault(s => s.Name == primary));
            ApplyAccent(AccentColors.FirstOrDefault(s => s.Name == accents));
            Theme = _theme;
        }
    }
}