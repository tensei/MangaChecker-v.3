using System.Collections.Generic;
using MangaChecker.Data.Interfaces;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;

namespace MangaCheckerV3.Helpers
{
    public class ThemeHelper : IThemeHelper
    {
        private readonly PaletteHelper _paletteHelper;
        private readonly SwatchesProvider _swatchesProvider;

        public ThemeHelper(SwatchesProvider swatchesProvider, PaletteHelper paletteHelper)
        {
            _swatchesProvider = swatchesProvider;
            _paletteHelper = paletteHelper;
        }

        public void ChangeAccentColorTo(Swatch accentColor)
        {
            //pallet.SetLightDark(theme);
            //            await Application.Current.Dispatcher.BeginInvoke(new Action(() => {
            _paletteHelper.ReplaceAccentColor(accentColor);
            //            }));
        }

        public void ChangePrimaryColorTo(Swatch primaryColor)
        {
            //pallet.SetLightDark(theme);
            _paletteHelper.ReplacePrimaryColor(primaryColor);
        }

        public void ChangeThemeTo(bool isDark)
        {
            //pallet.SetLightDark(theme);
            _paletteHelper.SetLightDark(isDark);
        }

        public IEnumerable<Swatch> Swatches()
        {
            return _swatchesProvider.Swatches;
        }
    }
}