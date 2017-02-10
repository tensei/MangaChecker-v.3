using System.Collections.Generic;
using System.Threading.Tasks;
using MaterialDesignColors;

namespace MangaChecker.Data.Interfaces {
    public interface IThemeHelper {
        IEnumerable<Swatch> Swatches();
        Task ChangeAccentColorTo(Swatch accentColor);
        Task ChangePrimaryColorTo(Swatch primaryColor);
        Task ChangeThemeTo(bool isDark);
    }
}