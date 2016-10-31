using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaChecker.Interfaces {
	public interface IPluginMetadata {
		
		string Title { get; }
		
		string Description { get; }

		string Version { get; }
		
		string Author { get; }
	}
}
