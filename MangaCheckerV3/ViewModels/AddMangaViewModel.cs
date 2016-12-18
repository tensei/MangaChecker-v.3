using System.Collections.ObjectModel;
using MangaCheckerV3.Models;
using PropertyChanged;

namespace MangaCheckerV3.ViewModels {
	[ImplementPropertyChanged]
	public class AddMangaViewModel {
		private readonly ObservableCollection<SiteModel> _siteAdds = new ObservableCollection<SiteModel>();

		public AddMangaViewModel() {
			SiteAdds = new ReadOnlyObservableCollection<SiteModel>(_siteAdds);
		}

		public SiteModel SelectedSite { get; set; }
		public ReadOnlyObservableCollection<SiteModel> SiteAdds { get; }
	}
}