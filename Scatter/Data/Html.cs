using System;
using System.Web;
using System.Text;

namespace SilentOrbit.Scatter.Data
{
	public class Html
	{
		readonly string html;
		public readonly DateTime LastModified;

		public Html()
		{
			this.html = "";
		}

		Html(string rawHtml, DateTime modified)
		{
			this.html = rawHtml;
			this.LastModified = modified;
		}

		public static Html Escape(string text)
		{
			return new Html(HttpUtility.HtmlEncode(text), DateTime.MinValue);
		}

		public static Html Escape(string text, DateTime modified)
		{
			return new Html(HttpUtility.HtmlEncode(text), modified);
		}
        /// <summary>
        /// From raw html, unescaped
        /// </summary>
		public static Html Raw(string html)
		{
			return new Html(html, DateTime.MinValue);
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name='html'>
        /// Raw HTML
        /// </param>
        /// <param name='modified'>
        /// Timestamp of when the html was last modified
        /// </param>
		public static Html Raw(string html, DateTime modified)
		{
			return new Html(html, modified);
		}

		static DateTime Last(Html a, Html b)
		{
			if (a.LastModified > b.LastModified)
				return a.LastModified;
			else
				return b.LastModified;
		}

		public static Html operator +(Html a, Html b)
		{
			return new Html(a.html + b.html, Last(a, b));
		}

		public static implicit operator Html(string text)
		{
			return new Html(HttpUtility.HtmlEncode(text), DateTime.MinValue);
		}

		public override string ToString()
		{
			return html;
		}

		public object Length
		{
			get { return html.Length; }
		}
	}
}

