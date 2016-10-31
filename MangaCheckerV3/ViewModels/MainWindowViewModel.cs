using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MahApps.Metro.Controls;
using MangaCheckerV3.Common;
using MangaCheckerV3.Helpers;
using MangaCheckerV3.SQLite;
using MangaCheckerV3.SQLite.Tables;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using PropertyChanged;

namespace MangaCheckerV3.ViewModels {
	[ImplementPropertyChanged]
	public class MainWindowViewModel {
		private readonly Dictionary<string, int> _transitionerIndexSelector = new Dictionary<string, int> {
			{"Home", 0},
			{"Add Manga", 1},
			{"Settings", 4},
			{"History", 2},
			{"Plugins", 3},
			{"Theme", 5}
		};

		private ListBoxItem _drawerSelectedItem;

		/// <summary>
		///     Initializes a new instance of the MainWindowViewModel class.
		/// </summary>
		public MainWindowViewModel() {
			Instance = this;
			SnackbarQueue = new SnackbarMessageQueue();
			SnackbarQueue.Enqueue("Test message", true);
			if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "mcv3.sqlite"))) {
				var updated = new Database().CheckDbVersion();
				if (updated!= null) {
					SnackbarQueue.Enqueue(updated);
				}
			} else {
				new Database().CreateDatabase();
				SnackbarQueue.Enqueue("Created new Database");
			}
			//Task.Run(async () => {
			//	for (int i = 0; i < 20; i++) {
			//		await new Database().InsertManga(new Manga {
			//			Added = DateTime.Now,
			//			Chapter = 1,
			//			Name = "trest",
			//			Updated = DateTime.Now,
			//			Site = "lul",
			//			Link = "jj"
			//		});
			//	}
			//});
			//MangaListViewModel.Instance.Fill();
		}

		public static MainWindowViewModel Instance { get; private set; }

		public ICommand DoubleClickCommand { get; }

		public ListBoxItem DrawerSelectedItem {
			get { return _drawerSelectedItem; }
			set {
				_drawerSelectedItem = value;
				TransitionerIndex = _transitionerIndexSelector[value.ToolTip.ToString()];
			}
		}

		public SnackbarMessageQueue SnackbarQueue { get; }

		public int DrawerIndex { get; set; }

		public int TransitionerIndex { get; set; }
		
		public bool MenuToggleButton { get; set; } = true;

	}
}