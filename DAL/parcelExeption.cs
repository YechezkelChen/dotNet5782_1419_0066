using System;
using System.Runtime.Serialization;

namespace DalObject
{
    [Serializable]
    internal class parcelExeption : Exception
    {
        public parcelExeption()
        {
        }

        public parcelExeption(string message) : base(message)
        {
        }

        public parcelExeption(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected parcelExeption(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}