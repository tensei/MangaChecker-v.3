using System;

namespace MangaChecker.Interfaces {
	public interface IPlugin :IDisposable {
		object SettingsView();
		void Initialize();
	}
}