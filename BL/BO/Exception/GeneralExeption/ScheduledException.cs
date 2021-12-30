using System;
using System.Runtime.Serialization;

namespace BO
{
    [Serializable]
    public class ScheduledException : Exception
    {
        public ScheduledException()
        {
        }

        public ScheduledException(string message) : base(message)
        {
        }

        public ScheduledException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ScheduledException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}