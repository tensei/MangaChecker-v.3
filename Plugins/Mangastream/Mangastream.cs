using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using MangaChecker.Interfaces;
using Mangastream.Common;
using Mangastream.ViewModel;
using Mangastream.Views;

namespace Mangastream {
	[Export(typeof(ISite))]
	[ExportMetadata("Title", "Mangastream")]
	[ExportMetadata("Description", "Weow")]
	[ExportMetadata("Version", "0.1")]
	[ExportMetadata("Author", "Tensei")]
	public class Mangastream : ISite {
		private UserControl _settingsView;


		private UserControl _view;

		public static Mangastream Instance;

		public object View() {
			return _view;
		}

		public object SettingsView() {
			return _settingsView;
		}

		public object AddView() {
			return null;
		}

		public void Stop() {
			//throw new NotImplementedException();
		}

		public string Name() {
			return "MangaStream";
		}

		public string Version() {
			return "0.0.1.1";
		}

		public void Initialize() {
			_settingsView = new SettingsView {DataContext = new SettingsViewModel()};
			_view = new MainView {DataContext = new MainViewModel()};
			Instance = this;
			//new CheckForNewTask().Run();
		}

		public void Dispose() {
			_view = null;
			_settingsView = null;
		}
	}
}
