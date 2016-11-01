﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using MangaChecker.Interfaces;

namespace MangaCheckerV3.Models {
	public class SiteModel {
		private readonly ISite _site;

		private readonly IPluginMetadata _metadata;
		
		public object View => _site.View();
		public object SettingsView => _site.SettingsView();
		public object AddView => _site.AddView();
		
		public string Name => _metadata.Title;
		public string Author => _metadata.Author;
		public string Version => _metadata.Version;
		public string Description => _metadata.Description;

		public SiteModel(Lazy<ISite, IPluginMetadata> site) {
			_site = site.Value;
			_metadata = site.Metadata;
		}
	}
}
