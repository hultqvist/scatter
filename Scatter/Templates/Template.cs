using System;
using System.IO;
using System.Text;
using System.Web;
using System.Collections.Generic;
using SilentOrbit.Scatter.Data;

namespace SilentOrbit.Scatter.Templates
{
	class Template : Variables
	{
		public readonly List<TemplateItem> Items = new List<TemplateItem>();
		protected Site site;
		readonly string filename;

		public Template(Site site, string filename)
		{
			this.site = site;
			this.filename = filename;
			string path = Path.Combine(site.TemplatePath, filename);
			string raw = File.ReadAllText(path, Encoding.UTF8);
			LastModified = File.GetLastWriteTime(path);

			int index = 0;
			while (true)
			{
				int tag = raw.IndexOf("<:", index);

				//Last content
				if (tag < 0)
				{
					if (index < raw.Length)
						Items.Add(new TemplateItem(Html.Raw(raw.Substring(index))));
					//Console.WriteLine("Template Last: " + index + ", " + tag);
					break;                    
				}

				//Content before tag
				if (tag > index)
				{
					Items.Add(new TemplateItem(Html.Raw(raw.Substring(index, tag - index))));
					//Console.WriteLine("Template cont: " + index + ", " + tag);
					index = tag;
				}

				//Tag
				tag = raw.IndexOf(">", index);
				if (tag < 0)
					throw new TemplateFormatException("Expected matching >");

				//Console.WriteLine("Template Tag: " + index + ", " + tag);
				string tagVal = raw.Substring(index + 2, tag - index - 2).Trim().Trim(':').ToLowerInvariant();

				//include .md tag contents
				if (tagVal.EndsWith(".md"))
				{
					string mdpath = Path.Combine(site.DataPath, tagVal);
					string text = File.ReadAllText(mdpath, Encoding.UTF8);
					Items.Add(new TemplateItem(Html.Raw(Generator.Markdown.Transform(text))));
                    
					DateTime fileTime = File.GetLastWriteTime(mdpath);
					if (LastModified < fileTime)
						LastModified = fileTime;
				}
				else
				{
					//Add tag
					Items.Add(new TemplateItem(tagVal));
				}

				index = tag + 1;
			}
		}

		public override string ToString()
		{
			return filename;
		}
	}
}

