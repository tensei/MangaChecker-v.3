using System;

namespace MangaChecker.DataTypes.Interfaces {
	public interface IPlugin :IDisposable {
		object SettingsView();
		void Initialize();
	}
}