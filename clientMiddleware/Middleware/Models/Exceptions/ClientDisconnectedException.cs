using System;
using System.Runtime.Serialization;

namespace Middleware
{
    [Serializable]
    internal class ClientDisconnectedException : Exception
    {
        public ClientDisconnectedException() : base("Client Disconnected")
        {          
        }

        public ClientDisconnectedException(string message) : base(message)
        {
        }

        public ClientDisconnectedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ClientDisconnectedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}