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
    public class GlobalVariables {
        public static SnackbarMessageQueue SnackbarQueue = new SnackbarMessageQueue();
        public static ObservableCollection<SiteListItem> _sites = new ObservableCollection<SiteListItem>();
    }
}
