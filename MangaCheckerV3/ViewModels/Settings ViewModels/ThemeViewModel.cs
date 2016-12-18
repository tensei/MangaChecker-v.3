using System.Collections.Generic;
using System.Threading.Tasks;
using MangaCheckerV3.Helpers;
using MangaCheckerV3.SQLite;
using PropertyChanged;

namespace MangaCheckerV3.ViewModels.Settings_ViewModels {
	[ImplementPropertyChanged]
	public class ThemeViewModel {
		private string _accentColor;
		private string _primaryColor;
		private string _theme;

		public ThemeViewModel() {
			Instance = this;
		}

		/// <summary>
		///     Initializes a new instance of the ThemeViewModel class.
		/// </summary>
		public static ThemeViewModel Instance { get; set; }

		public IEnumerable<string> PrimaryColors => ThemeHelper.Swatches;
		public IEnumerable<string> AccentColors => ThemeHelper.Accents;

		public string PrimaryColor {
			get { return _primaryColor; }
			set {
				_primaryColor = value;
				ThemeHelper.ChangePrimaryColorTo(value);
				new Database().UpdateTheme("Primary", value);
			}
		}

		public string AccentColor {
			get { return _accentColor; }
			set {
				_accentColor = value;
				ThemeHelper.ChangeAccentColorTo(value);
				new Database().UpdateTheme("Accents", value);
			}
		}

		public string Theme {
			get { return _theme; }
			set {
				_theme = value;
				ThemeHelper.ChangeThemeTo(value == "Dark");
				new Database().UpdateTheme("Theme", value);
			}
		}

		public async Task SetupTheme() {
			var themes = await new Database().GetThemes();
			foreach (var theme in themes) {
				if (theme.Name == "Primary") PrimaryColor = theme.Color;
				if (theme.Name == "Accents") AccentColor = theme.Color;
				if (theme.Name == "Theme") Theme = theme.Color;
			}
		}
	}
}