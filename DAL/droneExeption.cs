using System;
using System.Runtime.Serialization;

namespace DalObject
{
    [Serializable]
    public class DroneExeption : Exception
    {
        public DroneExeption()
        {
        }

        public DroneExeption(string message) : base(message)
        {
        }

        public DroneExeption(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DroneExeption(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}