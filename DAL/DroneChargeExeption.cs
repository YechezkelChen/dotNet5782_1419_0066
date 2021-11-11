using System;
using System.Runtime.Serialization;

namespace DalObject
{
    [Serializable]
    public class DroneChargeExeption : Exception
    {
        public DroneChargeExeption()
        {
        }

        public DroneChargeExeption(string message) : base(message)
        {
        }

        public DroneChargeExeption(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DroneChargeExeption(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}