using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MangaChecker.Database;
using MangaCheckerV3.Helpers;
using MangaCheckerV3.Properties;
using MangaCheckerV3.SQLite;
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
		}

		/// <summary>
		///     Initializes a new instance of the ThemeViewModel class.
		/// </summary>
		public static ThemeViewModel Instance { get; private set; }

		public IEnumerable<Swatch> PrimaryColors => ThemeHelper.Swatches;
		public IEnumerable<Swatch> AccentColors => ThemeHelper.Accents;

		public Swatch PrimaryColor {
			get { return _primaryColor; }
			set {
				_primaryColor = value;
				ThemeHelper.ChangePrimaryColorTo(value).ConfigureAwait(false);
			    Settings.Default.Primary = value.Name;
                Settings.Default.Save();
            }
		}

		public Swatch AccentColor {
			get { return _accentColor; }
			set {
				_accentColor = value;
                ThemeHelper.ChangeAccentColorTo(value).ConfigureAwait(false);
                Settings.Default.Accents = value.Name;
                Settings.Default.Save();
            }
		}

		public string Theme {
			get { return _theme; }
			set {
				_theme = value;
				ThemeHelper.ChangeThemeTo(value == "Dark").ConfigureAwait(false);
                Settings.Default.Theme = value;
                Settings.Default.Save();
            }
		}

        public void SetupTheme() {
            var _primary = Settings.Default.Primary;
            var _accents = Settings.Default.Accents;
            var _theme  = Settings.Default.Theme;
            PrimaryColor = PrimaryColors.FirstOrDefault(s => s.Name == _primary);
            AccentColor = AccentColors.FirstOrDefault(s => s.Name == _accents);
            Theme = _theme;
        }
    }
}