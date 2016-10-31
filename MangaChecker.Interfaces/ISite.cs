using System;

namespace MangaChecker.Interfaces {
	public interface ISite : IDisposable{
		object View();
		object SettingsView();
		object AddView();
		void Stop();
		string Name();
		string Version();
		void Initialize();
	}
}
