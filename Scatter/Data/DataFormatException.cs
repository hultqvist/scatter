using System;

namespace SilentOrbit.Scatter.Data
{
	class DataFormatException : Exception
	{
		public DataFormatException(string message) : base(message)
		{
		}

		public DataFormatException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}

