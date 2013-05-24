using System;
using System.IO;
using SilentOrbit.Scatter.Data;

namespace SilentOrbit.Scatter.Templates
{
	class IndexTemplate : Template
	{
		public IndexTemplate(Site site) : base(site, "index.html")
		{
		}

		public TemplateInstance Create(Site site)
		{
			var t = new TemplateInstance(this);
			t.LastModified = site.LastModified;
			t["site:path"] = site.UrlPath;
			t["site:title"] = site.Title;
			return t;
		}
	}
}

