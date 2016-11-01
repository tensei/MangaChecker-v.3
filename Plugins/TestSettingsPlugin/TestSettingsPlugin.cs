using System.ComponentModel.Composition;
using MangaChecker.Interfaces;

namespace TestSettingsPlugin {
	[Export(typeof(ISettingsPlugin))]
	[ExportMetadata("Title", "SettingsTest")]
	[ExportMetadata("Description", "settings test")]
	[ExportMetadata("Version", "0.1")]
	[ExportMetadata("Author", "Tensei")]
	public class TestSettingsPlugin : ISettingsPlugin
	{
		public object SettingsView() {
			return new TestView();
		}

		public void Initialize() {
			//throw new NotImplementedException();
		}

		public void Dispose() {
			//throw new NotImplementedException();
		}
	}
}
