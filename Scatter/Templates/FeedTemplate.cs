using System;
using System.IO;
using SilentOrbit.Scatter.Data;

namespace SilentOrbit.Scatter.Templates
{
	class FeedTemplate : Template
	{
		public FeedTemplate(Site site) : base(site, "feed.xml")
		{
		}

		public void Generate(Html entries, string path)
		{
			var t = new TemplateInstance(this);
			t.LastModified = site.LastModified;
			t["title"] = site.Title;
			t["url"] = site.URL;
			t["updated"] = t.LastModified.ToUniversalTime().ToString(@"yyyy-MM-dd\THH:mm:ss\Z");
			t["entries"] = entries;
			t.Write(path);
		}
	}
}

