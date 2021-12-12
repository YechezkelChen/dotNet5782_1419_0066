using System;
using System.Runtime.Serialization;

namespace BL
{
    [Serializable]
    public class ChargeSlotsException : Exception
    {
        public ChargeSlotsException()
        {
        }

        public ChargeSlotsException(string message) : base(message)
        {
        }

        public ChargeSlotsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ChargeSlotsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}