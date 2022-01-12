using System;
using System.Runtime.Serialization;

namespace BO
{
    [Serializable]
    public class NoParcelsToDroneException : Exception
    {
        public NoParcelsToDroneException()
        {
        }

        public NoParcelsToDroneException(string message) : base(message)
        {
        }

        public NoParcelsToDroneException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoParcelsToDroneException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}