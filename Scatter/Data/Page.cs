using System;
using System.IO;

namespace SilentOrbit.Scatter.Data
{
	class Page : PropertiesReader
	{
        /// <summary>
        /// Text used in page link, if empty no link will be generated
        /// From post header
        /// </summary>
		public string Title { get; set; }
        /// <summary>
        /// Text used in page link, if empty no link will be generated
        /// From post header
        /// </summary>
		public string LinkTitle { get; set; }
        /// <summary>
        /// If set points to an external page the menu button links to
        /// </summary>
		public string LinkUrl { get; set; }

		public readonly string Path;

		public bool HasDate { get; set; }
        /// <summary>
        /// Order in which to be displayed
        /// </summary>
		public string Index { get; set; }

		public Page(string basePath, string path) : base(path)
		{
			if (path.EndsWith(".page") == false)
				throw new DataFormatException("file must end with .page");

			Title = Title ?? Name;
			LinkTitle = LinkTitle ?? Title;
			Index = Index ?? Name;
			Path = path.Substring(basePath.Length, path.Length - basePath.Length - ".page".Length) + "/";
		}

		protected override void ParseHeader(string key, string value)
		{
			switch (key)
			{
				case "date":
					HasDate = true;
					return;
				case "title":
					Title = value;
					return;
				case "linktitle":
					LinkTitle = value;
					return;
				case "linkurl":
					LinkUrl = value;
					return;
				case "index":
					Index = value;
					return;
			}
			throw new DataFormatException("Unkown key: " + key);
		}

		public static int ComparerIndex(Page a, Page b)
		{
			int r = a.Index.CompareTo(b.Index);
			if (r != 0)
				return r;
			return a.Name.CompareTo(b.Name);
		}

		public override string ToString()
		{
			return string.Format("[Page: Title={0}, LinkTitle={1}, URL={2}]", Title, LinkTitle, Name);
		}
	}
}

