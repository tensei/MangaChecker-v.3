using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;

namespace MangaCheckerV3.Helpers {
    public class ThemeHelper {
        public readonly IEnumerable<Swatch> Swatches = new SwatchesProvider().Swatches;

        public readonly IEnumerable<Swatch> Accents =
            new SwatchesProvider().Swatches.Where(a => a.IsAccented);

        public async Task ChangeAccentColorTo(Swatch accentColor) {
            //pallet.SetLightDark(theme);
            await Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                var palletHelper = new PaletteHelper();
                palletHelper.ReplaceAccentColor(accentColor);
            }));
        }

        public async Task ChangePrimaryColorTo(Swatch primaryColor) {
            //pallet.SetLightDark(theme);
            await Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                var palletHelper = new PaletteHelper();
                palletHelper.ReplacePrimaryColor(primaryColor);
            }));
        }

        public async Task ChangeThemeTo(bool isDark) {
            //pallet.SetLightDark(theme);
            await Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                var palletHelper = new PaletteHelper();
                palletHelper.SetLightDark(isDark);
            }));
        }
    }
}