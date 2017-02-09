﻿using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using MangaCheckerV3.Helpers;
using MangaCheckerV3.Properties;
using MaterialDesignColors;
using PropertyChanged;

namespace MangaCheckerV3.ViewModels {
    [ImplementPropertyChanged]
    public class ThemeViewModel {
        private string _theme;
        private readonly ThemeHelper _themeHelper;
        public ThemeViewModel(ThemeHelper themeHelper) {
            Instance = this;
            _themeHelper = themeHelper;
            ApplyPrimaryCommand = new ActionCommand(s => ApplyPrimary((Swatch) s));
            ApplyAccentCommand = new ActionCommand(s => ApplyAccent((Swatch) s));
        }

        /// <summary>
        ///     Initializes a new instance of the ThemeViewModel class.
        /// </summary>
        public static ThemeViewModel Instance { get; private set; }

        public IEnumerable<Swatch> PrimaryColors => _themeHelper.Swatches;
        public IEnumerable<Swatch> AccentColors => _themeHelper.Accents;

        public ICommand ApplyPrimaryCommand { get; }
        public ICommand ApplyAccentCommand { get; }

        public string Theme {
            get { return _theme; }
            set {
                _theme = value;
                _themeHelper.ChangeThemeTo(value == "Dark").ConfigureAwait(false);
                Settings.Default.Theme = value;
                Settings.Default.Save();
            }
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
            var theme = Settings.Default.Theme;
            ApplyPrimary(PrimaryColors.FirstOrDefault(s => s.Name == primary));
            ApplyAccent(AccentColors.FirstOrDefault(s => s.Name == accents));
            Theme = theme;
        }
    }
}