using System;
using System.Runtime.Serialization;

namespace BO
{
    [Serializable]
    public class PhoneException : Exception
    {
        public PhoneException()
        {
        }

        public PhoneException(string message) : base(message)
        {
        }

        public PhoneException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PhoneException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}