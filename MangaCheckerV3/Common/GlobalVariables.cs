using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaterialDesignThemes.Wpf;
using PropertyChanged;

namespace MangaCheckerV3.Common {
    [ImplementPropertyChanged]
    public class GlobalVariables {
        public static SnackbarMessageQueue SnackbarQueue = new SnackbarMessageQueue();
    }
}
