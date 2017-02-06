using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MangaCheckerV3.Models;
using MaterialDesignThemes.Wpf;
using PropertyChanged;

namespace MangaCheckerV3.Common {
    [ImplementPropertyChanged]
    public static class GlobalVariables {
        public static readonly SnackbarMessageQueue SnackbarQueue = new SnackbarMessageQueue();
        public static readonly ObservableCollection<SiteListItem> Sites = new ObservableCollection<SiteListItem>();
    }
}
