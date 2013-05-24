using System;
using System.IO;
using SilentOrbit.Scatter.Data;

namespace SilentOrbit.Scatter.Templates
{
	class PostTemplate : Template
	{
		public PostTemplate(Site site) : base(site, "BlogPost.html")
		{

		}

		public Html Generate(Site site, Post post)
		{
			var t = new TemplateInstance(this);
			t.LastModified = post.LastModified;
			t["title"] = post.Title;
			t["author"] = post.Author;
			t["gmdate"] = post.Date.ToUniversalTime().ToString("yyyy-MM-dd\\THH:mm:ss\\Z");
			t["longdate"] = post.Date.ToString("ddd dd MMM yyyy");
			t["monthyear"] = post.Date.ToString("MMM yyyy");
			t["updated"] = post.LastModified.ToString("ddd dd MMM yyyy");
			t["url"] = site.UrlPath + post.Path;
			t["site:path"] = site.UrlPath;
			t["contents"] = Html.Raw(Generator.Markdown.Transform(post.Content));
			return t.ToHtml();
		}
	}
}

