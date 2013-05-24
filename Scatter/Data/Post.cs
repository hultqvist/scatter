using System;
using System.IO;
using System.Web;

namespace SilentOrbit.Scatter.Data
{
	class Post : PropertiesReader
	{
        /// <summary>
        /// Date from post header
        /// </summary>
		public DateTime Date { get; set; }
        /// <summary>
        /// From post header
        /// </summary>
		public string Author { get; set; }
        /// <summary>
        /// From post header
        /// </summary>
		public string Title { get; set; }
        /// <summary>
        /// Relative to site.URL
        /// </summary>
		public string Path { get; set; }
        /// <summary>
        /// First lines of post, stop at first empty line
        /// </summary>
		public string ExcerptMarkdown { get; set; }

		public Post(string path) : base(path)
		{
			Path = Date.ToString("yyyy/MM/") + HttpUtility.UrlEncode(Name) + "/";
			Title = Title ?? Name;

			//Include all to first empty line
			ExcerptMarkdown = "";
			string[] lines = Content.Split('\n');
			foreach (string line in lines)
			{
				if (line.Trim() == "" && ExcerptMarkdown != "")
					break;
				ExcerptMarkdown += "\n" + line;
			}
		}

		protected override void ParseHeader(string key, string value)
		{
			switch (key)
			{
				case "date":
					Date = DateTime.Parse(value);
					return;
				case "author":
					Author = value;
					return;
				case "title":
					Title = value;
					return;
			}
			throw new DataFormatException("Unkown key: " + key);
		}

		public static int ComparerLatestFirst(Post a, Post b)
		{
			return -a.Date.CompareTo(b.Date);
		}

		public override string ToString()
		{
			return string.Format("[Post: {0}, {1}, {2} @ {3}]", Date.ToString("yyyy-MM-dd"), Author, Title, Path);
		}
	}
}

