using System;
using SilentOrbit.Scatter.Data;

namespace SilentOrbit.Scatter.Templates
{
    /// <summary>
    /// For internal use by Template
    /// </summary>
	class TemplateItem
	{
		public Html Content { get; set; }

		public string Variable { get; set; }

		public TemplateItem(Html content)
		{
			this.Content = content;
			this.Variable = null;
		}

		public TemplateItem(string variable)
		{
			this.Content = null;
			this.Variable = variable;
		}

		public override string ToString()
		{
			if (Content == null)
				return string.Format("[Item: Variable={0}]", Variable);
			else
				return string.Format("[Item: Content={0}]", Content.Length);
		}
	}
}

