using System;
using System.Runtime.Serialization;

namespace RiseTheBar.Services
{
    [Serializable]
    public class NoSuchPlaceException : Exception
    {
        public NoSuchPlaceException()
        {
        }

        public NoSuchPlaceException(string message) : base(message)
        {
        }

        public NoSuchPlaceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoSuchPlaceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}