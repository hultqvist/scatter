using System;
using System.Collections.Generic;
using SilentOrbit.Scatter.Data;

namespace SilentOrbit.Scatter.Templates
{
	public class Variables
	{
		public readonly Dictionary<string, Html> vars = new Dictionary<string, Html>();
		DateTime lastModified;

		public DateTime LastModified
		{
			get{ return lastModified;}
			set
			{
				if (lastModified < value)
					lastModified = value;
			}
		}

		public Variables()
		{
			this.LastModified = DateTime.MinValue;
		}

		public Variables(DateTime modified)
		{
			this.LastModified = modified;
		}

		public Html this [string key]
		{
			get
			{
				string name = key.ToLowerInvariant();
				if (vars.ContainsKey(name))
					return vars[name];
				return new Html();
			}
			set
			{
				string name = key.ToLowerInvariant();
				if (vars.ContainsKey(name))
					vars[name] = value;
				else
					vars.Add(name, value);
			}
		}

		public Html this [string key, DateTime modified]
		{
			set
			{
				this[key] = value;
				if (this.LastModified < modified)
					this.LastModified = modified;
			}
		}
	}
}

