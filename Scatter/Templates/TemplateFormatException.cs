using System;

namespace SilentOrbit.Scatter.Templates
{
	class TemplateFormatException : Exception
	{
		public TemplateFormatException(string message) : base(message)
		{
		}

		public TemplateFormatException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}

