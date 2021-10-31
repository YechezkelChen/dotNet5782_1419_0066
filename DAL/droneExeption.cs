using System;
using System.Runtime.Serialization;

namespace DalObject
{
    [Serializable]
    internal class droneExeption : Exception
    {
        public droneExeption()
        {
        }

        public droneExeption(string message) : base("drone exeption" +message)
        {
        }

        public droneExeption(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected droneExeption(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}