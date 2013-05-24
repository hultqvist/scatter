using System;
using System.Collections.Generic;
using SilentOrbit.Scatter.Data;
using System.Text;
using System.IO;

namespace SilentOrbit.Scatter.Templates
{
	public class TemplateInstance : Variables
	{
		Template template;

		internal TemplateInstance(Template template) : base(template.LastModified)
		{
			this.template = template;
		}

		public Html ToHtml()
		{
			StringBuilder s = new StringBuilder();
			foreach (TemplateItem i in template.Items)
			{
				if (i.Variable == null)
				{
					s.Append(i.Content);
				}
				else
				{
					Html html;
					if (vars.ContainsKey(i.Variable))
						html = vars[i.Variable];
					else if (template.vars.ContainsKey(i.Variable))
						html = template.vars[i.Variable];
					else
						throw new TemplateFormatException("Unknown variable: " + i.Variable + "\nIn template: " + template);

					s.Append(html);
					if (LastModified < html.LastModified)
						LastModified = html.LastModified;
				}
			}
			return Html.Raw(s.ToString(), LastModified);
		}
        /// <summary>
        /// Write content to path and set correct last modified timestamp.
        /// </summary>
        /// <param name='path'>
        /// Path.
        /// </param>
		public void Write(string path)
		{
			Html html = ToHtml();
			using (TextWriter w = new StreamWriter(path, false, Encoding.UTF8))
			{
				w.Write(ToHtml());
			}
			File.SetLastWriteTime(path, html.LastModified);

			//Only report new/updated pages
			if (html.LastModified > DateTime.Now.AddDays(-1))
				Console.WriteLine(html.LastModified.ToString("yyyy-MM-dd HH:mm") + "\t" + path);
		}
	}
}

