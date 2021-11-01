using System;
using System.Runtime.Serialization;

namespace DalObject
{
    [Serializable]
    internal class ParcelExeption : Exception
    {
        public ParcelExeption()
        {
        }

        public ParcelExeption(string message) : base(message)
        {
        }

        public ParcelExeption(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ParcelExeption(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}