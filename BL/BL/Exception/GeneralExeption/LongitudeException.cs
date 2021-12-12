using System;
using System.Runtime.Serialization;

namespace BL
{
    [Serializable]
    public class LongitudeException : Exception
    {
        public LongitudeException()
        {
        }

        public LongitudeException(string message) : base(message)
        {
        }

        public LongitudeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LongitudeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}