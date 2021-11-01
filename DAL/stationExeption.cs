using System;
using System.Runtime.Serialization;

namespace DalObject
{
    [Serializable]
    internal class stationExeption : Exception
    {
        public stationExeption()
        {
        }

        public stationExeption(string message) : base(message)
        {
        }

        public stationExeption(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected stationExeption(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}