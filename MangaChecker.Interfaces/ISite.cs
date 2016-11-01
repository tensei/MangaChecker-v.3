using System;

namespace MangaChecker.Interfaces {
	public interface ISite : ISettingsPlugin{
		object View();
		object AddView();
	}
}
