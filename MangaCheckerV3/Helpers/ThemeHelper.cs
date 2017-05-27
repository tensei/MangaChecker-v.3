using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using MangaChecker.Data.Interfaces;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;

namespace MangaCheckerV3.Helpers {
    public class ThemeHelper : IThemeHelper {
        private readonly SwatchesProvider _swatchesProvider;

        public ThemeHelper() {
            _swatchesProvider = new SwatchesProvider();
        }

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

        public IEnumerable<Swatch> Swatches() {
            return _swatchesProvider.Swatches;
        }
    }
}