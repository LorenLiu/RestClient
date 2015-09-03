using System;
using System.Runtime.Serialization;

namespace RestUtil
{
    public class RestException : Exception
    {
        public RestException() : base()
        {

        }

        public RestException(string message) : base(message)
        {

        }

        public RestException(string message, Exception innerException) : base(message, innerException)
        {

        }
        
        public RestException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}
