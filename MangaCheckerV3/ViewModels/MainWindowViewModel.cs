using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MangaChecker.Database;
using MangaChecker;
using MangaChecker.Parser;
using MaterialDesignThemes.Wpf;
using PropertyChanged;

namespace MangaCheckerV3.ViewModels {
	[ImplementPropertyChanged]
	public class MainWindowViewModel {

		private PackIcon _drawerSelectedItem;

		/// <summary>
		///     Initializes a new instance of the MainWindowViewModel class.
		/// </summary>
		public MainWindowViewModel() {
			Instance = this;
			SnackbarQueue = new SnackbarMessageQueue();
			SnackbarQueue.Enqueue("Test message", true);
		    //var xml = new Client().GetRssFeedAsync("https://yomanga.co/reader/feeds/rss").ConfigureAwait(false);
      //      new Client().GetRssFeedAsync("https://gameofscanlation.moe/projects/trinity-wonder/index.rss").ConfigureAwait(false);
      //      new Client().GetRssFeedAsync("http://www.webtoons.com/en/fantasy/tower-of-god/rss?title_no=95").ConfigureAwait(false);
	    }

		public static MainWindowViewModel Instance { get; private set; }

		public ICommand DoubleClickCommand { get; }
        
		public SnackbarMessageQueue SnackbarQueue { get; }

		public int DrawerIndex { get; set; }

		public int TransitionerIndex { get; set; }

		public bool MenuToggleButton { get; set; } = true;
	}
}