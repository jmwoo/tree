using System.Runtime.Serialization;

namespace lib.Services
{
	[Serializable]
	internal class InvalidTreeException : Exception
	{
		public InvalidTreeException()
		{
		}

		public InvalidTreeException(string? message) : base(message)
		{
		}

		public InvalidTreeException(string? message, Exception? innerException) : base(message, innerException)
		{
		}

		protected InvalidTreeException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}