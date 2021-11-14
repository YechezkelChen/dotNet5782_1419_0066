using System;
using System.Runtime.Serialization;

namespace DalObject
{
    [Serializable]
    public class stationException : Exception
    {
        public stationException()
        {
        }

        public stationException(string message) : base(message)
        {
        }

        public stationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected stationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}