using System;
using System.Text;
using System.Text.RegularExpressions;
using System.IO.Compression;
using System.IO;
using Yahoo.Yui.Compressor;

namespace SilentOrbit.Scatter
{
	public static class Compressor
	{
		public static void CompressDirectoryRecursive(string path)
		{
			string[] files;

			files = Directory.GetFiles(path, "*.html", SearchOption.AllDirectories);
			foreach (string f in files)
				CompressFile(f);
			files = Directory.GetFiles(path, "*.css", SearchOption.AllDirectories);
			foreach (string f in files)
				CompressFile(f);
			files = Directory.GetFiles(path, "*.atom", SearchOption.AllDirectories);
			foreach (string f in files)
				CompressFile(f);
			files = Directory.GetFiles(path, "*.js", SearchOption.AllDirectories);
			foreach (string f in files)
				CompressFile(f);
		}

		static JavaScriptCompressor jc = new JavaScriptCompressor()
		{
			Encoding = Encoding.UTF8,
			ObfuscateJavascript = true
		};
		static CssCompressor cc = new CssCompressor()
		{
			RemoveComments = true
		};

		static void CompressFile(string path)
		{
			DateTime fileDate = File.GetLastWriteTime(path);

			//Minify uncompressed file
			if (path.EndsWith(".js"))
			{
				string original = File.ReadAllText(path, Encoding.UTF8);
				if (string.IsNullOrEmpty(original))
					return;
				string compressed = jc.Compress(original);
				File.WriteAllText(path, compressed, Encoding.UTF8);
				File.SetLastWriteTime(path, fileDate);
			}
			if (path.EndsWith(".css"))
			{
				string original = File.ReadAllText(path, Encoding.UTF8);
				if (string.IsNullOrEmpty(original))
					return;
				string compressed = cc.Compress(original);
				File.WriteAllText(path, compressed, Encoding.UTF8);
				File.SetLastWriteTime(path, fileDate);
			}

			//Compress
			using (GZipStream gz = new GZipStream(new FileStream(path + ".gz", FileMode.Create), CompressionMode.Compress))
			{
				byte[] buffer = File.ReadAllBytes(path);
				gz.Write(buffer, 0, buffer.Length);
			}

			File.SetLastWriteTime(path + ".gz", fileDate);
		}
	}
}

