using System;
using System.IO;
using System.Collections.Generic;

namespace SilentOrbit.Scatter.Data
{
	class Site : PropertiesReader
	{
        //Public site settings
		public string Email { get; set; }

		public string URL { get; set; }

		public string UrlPath { get; set; }

		public string Title { get; set; }
        /// <summary>Local filesystem location</summary>
		public string DataPath { get; set; }
        /// <summary>Local filesystem location</summary>
		public string StaticPath { get; set; }
        /// <summary>Local filesystem location</summary>
		public string TemplatePath { get; set; }
        /// <summary>Local filesystem location</summary>
		public string WebPath { get; set; }

		public List<Page> Pages { get; set; }

		public List<Post> Posts { get; set; }

		public Site(string path) : base(Path.Combine(path, "config.txt"))
		{
			if (DataPath == null)
				DataPath = "";
			DataPath = Path.GetFullPath(Path.Combine(path, DataPath));
			if (StaticPath == null)
				StaticPath = "static";
			StaticPath = Path.GetFullPath(Path.Combine(path, StaticPath));
			if (TemplatePath == null)
				TemplatePath = "template";
			TemplatePath = Path.GetFullPath(Path.Combine(path, TemplatePath));
			if (WebPath == null)
				WebPath = "htdocs";
			WebPath = Path.GetFullPath(Path.Combine(path, WebPath));

			if (UrlPath.EndsWith("/") == false)
				UrlPath += "/";

			Pages = new List<Page>();
			Posts = new List<Post>();
		}

		protected override void ParseHeader(string key, string value)
		{
			switch (key)
			{
				case "email":
					Email = value;
					return;
				case "url":
					URL = value;
					return;
				case "path":
					UrlPath = value;
					return;
				case "title":
					Title = value;
					return;
				case "data_path":
					DataPath = value;
					return;
				case "static_path":
					StaticPath = value;
					return;
				case "template_path":
					TemplatePath = value;
					return;
				case "web_path":
					WebPath = value;
					return;
			}
			throw new DataFormatException("Unkown key: " + key);
		}

		public void Add(Page p)
		{
			Pages.Add(p);
		}

		public void Add(Post p)
		{
			Posts.Add(p);
		}

		public Page GetPage(string name)
		{
			foreach (Page p in Pages)
			{
				if (p.Name == name)
					return p;
			}
			return null;
		}
	}
}

