using System.Collections.Generic;
using System.Linq;
using MangaChecker.Interfaces;

namespace MangaCheckerV3.Common {
	public class AggregateSites : ISite {
		private readonly ISite[] _sites;

		public AggregateSites(IEnumerable<ISite> sites) {
			_sites = sites.ToArray();
		}

		public void Initialize() {
			foreach (var site in _sites) site.Initialize();
		}

		public object View() {
			return null;
		}

		public object SettingsView() {
			return null;
		}

		public object AddView() {
			return null;
		}

		public void Dispose() {
			foreach (var site in _sites) site.Dispose();
		}
	}
}