using System;
using System.Runtime.Serialization;

namespace DalObject
{
    [Serializable]
    internal class customerExeption : Exception
    {
        public customerExeption()
        {
        }

        public customerExeption(string message) : base("customer exeption" + message)
        {
        }

        public customerExeption(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected customerExeption(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}