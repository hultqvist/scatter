using System;
using System.IO;
using SilentOrbit.Scatter.Data;
using System.Text;
using System.Web;
using MarkdownSharp;
using SilentOrbit.Scatter.Templates;

namespace SilentOrbit.Scatter
{
	static class Generator
	{
		public static Markdown Markdown = new Markdown(new MarkdownOptions()
		{
			AutoHyperlink = true, 
			LinkEmails = true
		});

		public static void Generate(Site site)
		{
			LoadData(site);

			//Read templates
			var indexTemplate = new IndexTemplate(site);

			//Clean web target
			FileManager.Clean(site.WebPath);
			//Copy static: Done in calling bash script
			FileManager.Clone(site.StaticPath, site.WebPath);

			//Generate news
			indexTemplate["site:news"] = GenerateNews(site);

			var pageTemplate = new PageTemplate(site);
			var postTemplate = new PostTemplate(site);
            
			//Generate all files
			GenerateIndex(indexTemplate, site);
			GenerateFeed(site);
			foreach (Page p in site.Pages)
				GeneratePage(site, p, pageTemplate, indexTemplate);
			foreach (Post p in site.Posts)
				GeneratePost(site, p, postTemplate, indexTemplate);
			GenerateNewPostPageTemplate(site);

			Compressor.CompressDirectoryRecursive(site.WebPath);
		}

		static void LoadData(Site site)
		{
			//Scan for pages
			string[] pageFiles = Directory.GetFiles(site.DataPath, "*.page", SearchOption.AllDirectories);
			foreach (string pageFile in pageFiles)
			{
				Page p = new Page(site.DataPath, pageFile);
				//Ignore news pages
				if (p.HasDate)
					continue;

				site.Add(p);
				//Console.WriteLine("Page: " + p);
			}
			site.Pages.Sort(Page.ComparerIndex);

			//Scan for posts
			string[] postFiles = Directory.GetFiles(site.DataPath, "*.post", SearchOption.AllDirectories);
			foreach (string postFile in postFiles)
			{
				Post p = new Post(postFile);
				//Ignore non news pages
				if (p.Date == DateTime.MinValue)
					continue;

				site.Add(p);
				//Console.WriteLine("News: " + p);
			}
			site.Posts.Sort(Post.ComparerLatestFirst);
		}
        /// <summary>
        /// Helper for generating html list item <li></li>
        /// </summary>
		static Html LiTag(string url, string title, string text, bool active, DateTime modified)
		{
			Html titleHtml = new Html();
			if (title != "")
				titleHtml = Html.Raw(" title=\"") + title + Html.Raw("\"");
			if (active)
				return Html.Raw("<li><strong") + titleHtml + Html.Raw(">") + text + Html.Raw("</strong></li>", modified);
			else
				return Html.Raw("<li><a href=\"") + url + Html.Raw("\"") + titleHtml + Html.Raw(">") + text + Html.Raw("</a></li>", modified);
		}

		static Html GenerateTabs(Site site, Page page, Post post)
		{
			//Find out if we have an index page
			Page indexPage = null;
			foreach (Page p in site.Pages)
			{
				if (p.Name == "index")
				{
					indexPage = p;
					break;
				}
			}
			Html tabs = new Html();
			if (indexPage == null)
				tabs += LiTag(site.UrlPath, "", "News", page == null && post == null, DateTime.MinValue);
			else
				tabs += LiTag(site.UrlPath, indexPage.Title, indexPage.LinkTitle, page == indexPage, indexPage.LastModified);
			foreach (Page p in site.Pages)
			{
				if (p.LinkTitle == "")
					continue;
				if (p == indexPage)
					continue;
				tabs += LiTag(p.LinkUrl ?? site.UrlPath + p.Path, p.Title, p.LinkTitle, p == page, p.LastModified);
			}
			return tabs;
		}

		static Html GenerateNews(Site site)
		{
			Html news = Html.Raw("<ul>");
			string lastMonth = "";
			foreach (Post p in site.Posts)
			{
				string month = p.Date.ToString("MMM yyyy");
				if (month != lastMonth)
				{
					lastMonth = month;
					news += Html.Raw("</ul><h3>") + month + Html.Raw("</h3><ul>");
				}
				news += LiTag(site.UrlPath + p.Path, "", p.Title, false, p.LastModified);
			}
			news += Html.Raw("</ul>");
			return news;
		}

		static void GenerateIndex(IndexTemplate template, Site site)
		{
			var indexInstance = template.Create(site);
			if (site.GetPage("index") != null)
				return;

			var compactPostTemplate = new CompactPostTemplate(site);
			indexInstance["title"] = site.Title;
			indexInstance["tabs"] = GenerateTabs(site, null, null);
			indexInstance["contents"] = new Html();
			foreach (Post p in site.Posts)
			{
				indexInstance["contents"] += compactPostTemplate.Generate(site, p);
			}
			indexInstance.Write(Path.Combine(site.WebPath, "index.html"));
		}

		static void GenerateFeed(Site site)
		{
			string feedDir = Path.Combine(site.WebPath, "feed");
			Directory.CreateDirectory(feedDir);
			using (TextWriter w = new StreamWriter(Path.Combine(feedDir, ".htaccess"), false, Encoding.ASCII))
			{
				w.WriteLine("DirectoryIndex index.atom");
				w.WriteLine("ForceType application/atom+xml");
				w.WriteLine("AddType application/atom+xml .atom");
			}
			var feedTemplate = new FeedTemplate(site);
			var entryTemplate = new FeedEntryTemplate(site);

			//At least 5 posts and one month
			Html entries = new Html();
			int posts = 0;
			DateTime expire = DateTime.Now.AddMonths(-1);
			foreach (Post p in site.Posts)
			{
				if (posts > 5 && p.Date < expire && p.LastModified < expire)
					continue;

				entries += entryTemplate.Generate(site, p);

				posts += 1;
			}
			feedTemplate.Generate(entries, Path.Combine(feedDir, "index.atom"));
		}

		static void GeneratePage(Site site, Page p, PageTemplate pageTemplate, IndexTemplate indexTemplate)
		{
			string dirPath = Path.Combine(site.WebPath, p.Path);
			if (p.Path == "index/")
				dirPath = site.WebPath;
			Directory.CreateDirectory(dirPath);
			if (Directory.Exists(p.SourceDir))
				FileManager.Clone(p.SourceDir, dirPath);

			var indexInstance = indexTemplate.Create(site);
			indexInstance["title"] = p.Title;
			indexInstance["tabs"] = GenerateTabs(site, p, null);
			indexInstance["contents"] = pageTemplate.Generate(site, p);
			indexInstance.Write(Path.Combine(dirPath, "index.html"));
		}

		static void GeneratePost(Site site, Post p, PostTemplate postTemplate, IndexTemplate indexTemplate)
		{
			string dirPath = Path.Combine(site.WebPath, p.Path);
			string htmlPath = Path.Combine(dirPath, "index.html");
			Directory.CreateDirectory(dirPath);
			if (Directory.Exists(p.SourceDir))
				FileManager.Clone(p.SourceDir, dirPath);

			var inst = indexTemplate.Create(site);
			inst["title"] = p.Title;
			inst["tabs"] = GenerateTabs(site, null, p);
			inst["contents"] = postTemplate.Generate(site, p);
			inst.Write(htmlPath);
		}

		static void GenerateNewPostPageTemplate(Site site)
		{
			string postTemplatePath = Path.Combine(site.DataPath, "new.post-dist");
			using (TextWriter w = new StreamWriter(postTemplatePath, false, Encoding.UTF8))
			{
				w.WriteLine("Date: " + DateTime.Now.ToString("yyyy-MM-dd"));
				w.WriteLine("Author: ");
				w.WriteLine("Title: ");
				w.WriteLine();
				w.WriteLine("# ");
				w.WriteLine();
				w.WriteLine();
			}
			string pageTemplatePath = Path.Combine(site.DataPath, "new.page-dist");
			using (TextWriter w = new StreamWriter(pageTemplatePath, false, Encoding.UTF8))
			{
				w.WriteLine("Title: ");
				w.WriteLine("#LinkTitle: ");
				w.WriteLine("#LinkUrl: ");
				w.WriteLine("Index: ");
				w.WriteLine();
				w.WriteLine("# ");
				w.WriteLine();
				w.WriteLine();
			}
		}

		static DateTime Latest(params DateTime[] dates)
		{
			DateTime latest = DateTime.MinValue;
			foreach (DateTime d in dates)
			{
				if (d > latest)
					latest = d;
			}
			return latest;
		}
	}
}

