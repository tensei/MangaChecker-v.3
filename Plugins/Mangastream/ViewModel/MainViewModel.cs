
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using MangaCheckerV3.SQLite;
using MangaCheckerV3.SQLite.Tables;

namespace Mangastream.ViewModel {
	public class MainViewModel :ViewModelBase {
		/// <summary>
		/// Initializes a new instance of the MainViewModel class.
		/// </summary>
		private readonly ObservableCollection<Manga> _mangas = new ObservableCollection<Manga>();

		private string _error;
		private Manga _selectedManga;
		private string _teststring;
		public ReadOnlyObservableCollection<Manga> Mangas { get; }

		public static MainViewModel Instance;
		

		public string Error {
			get { return _error; }
			set {
				_error = value; 
				OnPropertyChanged();
			}
		}

		public Manga SelectedManga {
			get { return _selectedManga; }
			set {
				_selectedManga = value;
				OnPropertyChanged();
			}
		}

		public MainViewModel() {
			Instance = this;
			Mangas = new ReadOnlyObservableCollection<Manga>(_mangas);
			fill();
		}

		private async void fill() {
			var x = await new Database().GetAllMangas();
			x.ForEach(_mangas.Add);
			Error = "lul";
		}
	}
}