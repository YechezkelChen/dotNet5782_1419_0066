using System;
using System.Runtime.Serialization;

namespace BO
{
    [Serializable]
    public class BatteryDroneException : Exception
    {
        public BatteryDroneException()
        {
        }

        public BatteryDroneException(string message) : base(message)
        {
        }

        public BatteryDroneException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BatteryDroneException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}