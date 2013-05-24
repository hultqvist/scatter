using System;
using System.IO;
using SilentOrbit.Scatter.Data;
using MarkdownSharp;

namespace SilentOrbit.Scatter.Templates
{
	class FeedEntryTemplate : Template
	{
		public FeedEntryTemplate(Site site) : base(site, "feed.entry.xml")
		{
		}

		public Html Generate(Site site, Post post)
		{
			var t = new TemplateInstance(this);
			t.LastModified = post.LastModified;
			t["title"] = post.Title;
			t["author"] = post.Author;
			t["url"] = site.URL + post.Path;
			t["updated"] = post.LastModified.ToUniversalTime().ToString("yyyy-MM-dd\\THH:mm:ss\\Z");
			t["excerpt"] = Generator.Markdown.Transform(post.ExcerptMarkdown).Trim();
			t["content"] = Generator.Markdown.Transform(post.Content).Trim();
			return t.ToHtml();
		}
	}
}

