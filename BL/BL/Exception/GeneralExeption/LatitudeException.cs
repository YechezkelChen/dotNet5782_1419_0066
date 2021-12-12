using System;
using System.Runtime.Serialization;

namespace BL
{
    [Serializable]
    public class LatitudeException : Exception
    {
        public LatitudeException()
        {
        }

        public LatitudeException(string message) : base(message)
        {
        }

        public LatitudeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LatitudeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}