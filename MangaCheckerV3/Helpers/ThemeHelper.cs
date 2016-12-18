using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;

namespace MangaCheckerV3.Helpers {
	public class ThemeHelper {
		public static readonly List<string> Swatches = new SwatchesProvider().Swatches.Select(p => p.Name).ToList();

		public static readonly List<string> Accents =
			new SwatchesProvider().Swatches.Where(a => a.IsAccented).Select(a => a.Name).ToList();

		public static void ChangeAccentColorTo(string accentColor) {
			//pallet.SetLightDark(theme);
			Application.Current.Dispatcher.BeginInvoke(new Action(() => {
				var palletHelper = new PaletteHelper();
				palletHelper.ReplaceAccentColor(accentColor);
			}));
		}

		public static void ChangePrimaryColorTo(string primaryColor) {
			//pallet.SetLightDark(theme);
			Application.Current.Dispatcher.BeginInvoke(new Action(() => {
				var palletHelper = new PaletteHelper();
				palletHelper.ReplacePrimaryColor(primaryColor);
			}));
		}

		public static void ChangeThemeTo(bool isDark) {
			//pallet.SetLightDark(theme);
			Application.Current.Dispatcher.BeginInvoke(new Action(() => {
				var palletHelper = new PaletteHelper();
				palletHelper.SetLightDark(isDark);
			}));
		}
	}
}