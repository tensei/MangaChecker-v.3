using System.Collections.ObjectModel;
using MangaCheckerV3.Models;
using MaterialDesignThemes.Wpf;
using PropertyChanged;

namespace MangaCheckerV3.Common {
    [ImplementPropertyChanged]
    public static class GlobalVariables {
        public static readonly SnackbarMessageQueue SnackbarQueue = new SnackbarMessageQueue();
        public static IProviderService ProviderService;
    }
}