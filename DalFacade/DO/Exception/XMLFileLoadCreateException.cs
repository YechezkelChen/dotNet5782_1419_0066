using System;
using System.Runtime.Serialization;

namespace DO
{
    [Serializable]
    public class XMLFileLoadCreateException : Exception
    {
        public XMLFileLoadCreateException()
        {
        }

        public XMLFileLoadCreateException(string message) : base(message)
        {
        }

        public XMLFileLoadCreateException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected XMLFileLoadCreateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
        public XMLFileLoadCreateException(string filePath, string s, Exception exception)
        {
        }
    }
}