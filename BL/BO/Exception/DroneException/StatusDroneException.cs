using System;
using System.Runtime.Serialization;

namespace BO
{
    [Serializable]
    public class StatusDroneException : Exception
    {
        public StatusDroneException()
        {
        }

        public StatusDroneException(string message) : base(message)
        {
        }

        public StatusDroneException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected StatusDroneException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}