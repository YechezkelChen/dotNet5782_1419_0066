using System;
using System.Runtime.Serialization;

namespace BL
{
    [Serializable]
    internal class LocationException : Exception
    {
        public LocationException()
        {
        }

        public LocationException(string message) : base(message)
        {
        }

        public LocationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LocationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}