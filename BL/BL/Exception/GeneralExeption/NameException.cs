using System;
using System.Runtime.Serialization;

namespace BL
{
    [Serializable]
    public class NameException : Exception
    {
        public NameException()
        {
        }

        public NameException(string message) : base(message)
        {
        }

        public NameException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NameException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}