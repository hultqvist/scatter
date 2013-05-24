using System;
using System.IO;
using SilentOrbit.Scatter.Data;

namespace SilentOrbit.Scatter
{
	class MainClass
	{
		public static int Main(string[] args)
		{
			try
			{
				string path = Environment.CurrentDirectory;
				if (args.Length > 0)
					path = Path.GetFullPath(Path.Combine(path, args[0]));
				Site site = new Site(path);
				Console.WriteLine("Generating site: " + site.Title);
				Generator.Generate(site);
				Console.WriteLine("All done");
				return 0;
			}
			catch (Exception e)
			{
				Console.Error.WriteLine("Error:");
				Console.Error.WriteLine(e.Message);
				Console.Error.WriteLine(e.StackTrace);
				if (e.InnerException != null)
					Console.Error.WriteLine(e.InnerException.Message);
				return -1;
			}
		}
	}
}
