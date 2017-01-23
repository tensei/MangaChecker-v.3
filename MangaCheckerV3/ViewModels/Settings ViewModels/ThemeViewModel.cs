using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MangaChecker.Database;
using MangaCheckerV3.Helpers;
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
				new Database().UpdateTheme("Primary", value.Name);
			}
		}

		public Swatch AccentColor {
			get { return _accentColor; }
			set {
				_accentColor = value;
                ThemeHelper.ChangeAccentColorTo(value).ConfigureAwait(false);
				new Database().UpdateTheme("Accents", value.Name);
			}
		}

		public string Theme {
			get { return _theme; }
			set {
				_theme = value;
				ThemeHelper.ChangeThemeTo(value == "Dark").ConfigureAwait(false);
				new Database().UpdateTheme("Theme", value);
			}
		}

		public async Task SetupTheme() {
			var themes = await new Database().GetThemes();
			foreach (var theme in themes) {
				if (theme.Name == "Primary") PrimaryColor = PrimaryColors.FirstOrDefault(s=>s.Name ==theme.Color);
				if (theme.Name == "Accents") AccentColor = AccentColors.FirstOrDefault(s => s.Name == theme.Color);
				if (theme.Name == "Theme") Theme = theme.Color;
			}
		}
	}
}