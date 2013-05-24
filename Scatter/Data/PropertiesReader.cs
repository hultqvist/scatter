using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SilentOrbit.Scatter.Data
{
    /// <summary>
    /// Read file content from
    /// Site settings, Blog post and pages.
    /// <para>The file starts with key: value pairs followed by an empty line and the page content.</para>
    /// </summary>
	abstract class PropertiesReader
	{
		public string Content { get; set; }
        /// <summary>
        /// name part of .md file, for internal use only
        /// </summary>
		public readonly string Name;
        /// <summary>
        /// Last modified date of source file
        /// </summary>
		public readonly DateTime LastModified;
        /// <summary>
        /// If page/post contain additional files this is where they are stored
        /// </summary>
		public string SourceDir { get; private set; }

		public PropertiesReader(string path)
		{
			LastModified = File.GetLastWriteTime(path);
			Name = System.IO.Path.GetFileNameWithoutExtension(path);
			SourceDir = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(path), Name);

			using (TextReader r = new StreamReader(path, Encoding.UTF8))
			{
				//Headers
				while (true)
				{
					string line = r.ReadLine();
					if (line == null)
						return;
					if (line == "")
						break;

					string[] headerParts = line.Split(new char[] { ':' }, 2);
					if (headerParts.Length != 2)
						throw new InvalidDataException("Expected \"key: value\", got " + line);

					try
					{
						string key = headerParts[0].Trim().ToLowerInvariant();
						string value = headerParts[1].Trim(); 
						if (key.StartsWith("#"))
							continue;
						ParseHeader(key, value);
					}
					catch (DataFormatException ide)
					{
						throw new DataFormatException("Error in file: " + path, ide);
					}
				}

				//Content
				StringBuilder sb = new StringBuilder();
				while (true)
				{
					string line = r.ReadLine();
					if (line == null)
						break;

					sb.AppendLine(line);
				}
				Content = sb.ToString();
			}
		}

		protected abstract void ParseHeader(string key, string value);
	}
}

