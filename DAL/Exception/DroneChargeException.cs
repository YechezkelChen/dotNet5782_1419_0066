using System;
using System.Runtime.Serialization;

namespace DalObject
{
    [Serializable]
    public class DroneChargeException : Exception
    {
        public DroneChargeException()
        {
        }

        public DroneChargeException(string message) : base(message)
        {
        }

        public DroneChargeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DroneChargeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}