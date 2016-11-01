using System;

namespace MangaChecker.Interfaces {
	public interface ISettingsPlugin :IDisposable {
		object SettingsView();
		void Initialize();
	}
}