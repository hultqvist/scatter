using System;
using System.IO;
using SilentOrbit.Scatter.Data;

namespace SilentOrbit.Scatter.Templates
{
	class PageTemplate : Template
	{
		public PageTemplate(Site site) : base(site, "Page.html")
		{

		}

		public Html Generate(Site site, Page page)
		{
			var t = new TemplateInstance(this);
			t.LastModified = page.LastModified;
			t["title"] = page.Title;
			t["contents"] = Html.Raw(Generator.Markdown.Transform(page.Content));
			return t.ToHtml();
		}
	}
}

