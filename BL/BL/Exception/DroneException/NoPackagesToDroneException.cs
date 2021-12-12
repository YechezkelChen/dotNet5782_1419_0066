using System;
using System.Runtime.Serialization;

namespace BL
{
    [Serializable]
    public class NoPackagesToDroneException : Exception
    {
        public NoPackagesToDroneException()
        {
        }

        public NoPackagesToDroneException(string message) : base(message)
        {
        }

        public NoPackagesToDroneException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoPackagesToDroneException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}