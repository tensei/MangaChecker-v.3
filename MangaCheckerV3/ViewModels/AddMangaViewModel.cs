using System.Collections.ObjectModel;
using System.Linq;
using MangaCheckerV3.Common;
using MangaCheckerV3.Models;
using PropertyChanged;

namespace MangaCheckerV3.ViewModels {
	[ImplementPropertyChanged]
	public class AddMangaViewModel {
		public SiteModel SelectedSite { get; set; }
		private readonly ObservableCollection<SiteModel> _siteAdds = new ObservableCollection<SiteModel>();
		public ReadOnlyObservableCollection<SiteModel> SiteAdds { get; }

		public AddMangaViewModel() {
			SiteAdds = new ReadOnlyObservableCollection<SiteModel>(_siteAdds);
			MangaListViewModel.Instance.Sites.Where(s => s.AddView != null).ToList().ForEach(_siteAdds.Add);
		}
	}
}