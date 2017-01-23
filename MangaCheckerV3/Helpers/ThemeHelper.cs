using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;

namespace MangaCheckerV3.Helpers {
	public static class ThemeHelper {
	    public static readonly IEnumerable<Swatch> Swatches = new SwatchesProvider().Swatches;

	    public static readonly IEnumerable<Swatch> Accents =
	        new SwatchesProvider().Swatches.Where(a => a.IsAccented);

		public static async Task ChangeAccentColorTo(Swatch accentColor) {
            //pallet.SetLightDark(theme);
            await Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                var palletHelper = new PaletteHelper();
                palletHelper.ReplaceAccentColor(accentColor);
            }));
		}

		public static async Task ChangePrimaryColorTo(Swatch primaryColor) {
            //pallet.SetLightDark(theme);
            await Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                var palletHelper = new PaletteHelper();
                palletHelper.ReplacePrimaryColor(primaryColor);
            }));
		}

		public static async Task ChangeThemeTo(bool isDark) {
            //pallet.SetLightDark(theme);
            await Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                var palletHelper = new PaletteHelper();
                palletHelper.SetLightDark(isDark);
            }));
		}
	}
}