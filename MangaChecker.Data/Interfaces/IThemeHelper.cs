using System.Collections.Generic;
using MaterialDesignColors;

namespace MangaChecker.Data.Interfaces
{
    public interface IThemeHelper
    {
        IEnumerable<Swatch> Swatches();
        void ChangeAccentColorTo(Swatch accentColor);
        void ChangePrimaryColorTo(Swatch primaryColor);
        void ChangeThemeTo(bool isDark);
    }
}