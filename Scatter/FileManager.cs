using System;
using System.IO;

namespace SilentOrbit.Scatter
{
	public static class FileManager
	{
        /// <summary>
        /// Clean the web output path.
        /// Expect to find index.html will fail otherwise.
        /// </summary>
		public static void Clean(string path)
		{
			//Safety so we don't clean other places by accident
			if (path == null || path == "")
				throw new InvalidOperationException();
			//if (File.Exists(Path.Combine(path, "index.html")) == false)
			//    throw new InvalidOperationException("Safeguard: no index.html found in " + path);

			CleanUnsafe(path);
		}

		static void CleanUnsafe(string path)
		{
			foreach (string f in Directory.GetFiles(path))
				File.Delete(f);

			foreach (string d in Directory.GetDirectories(path))
			{
				CleanUnsafe(d);
				Directory.Delete(d);
			}
		}

		public static void Clone(string staticPath, string webPath)
		{
			foreach (string f in Directory.GetFiles(staticPath))
			{
				if (f.EndsWith(".page") || f.EndsWith(".post"))
					continue;
				string name = Path.GetFileName(f);
				string destination = Path.GetFullPath(Path.Combine(webPath, name));
				//Get relative symlink path
				File.Copy(f, destination);
				File.SetLastWriteTime(destination, File.GetLastWriteTime(f));
			}

			foreach (string d in Directory.GetDirectories(staticPath))
			{
				string name = Path.GetFileName(d);
				string sourcePath = Path.Combine(staticPath, name);
				string destinationPath = Path.Combine(webPath, name);
				Directory.CreateDirectory(destinationPath);
				Clone(sourcePath, destinationPath);
			}
		}
	}
}

