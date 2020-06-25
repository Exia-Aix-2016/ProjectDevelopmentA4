using System;
using System.Runtime.Serialization;

namespace Middleware.Services
{
    [Serializable]
    internal class RemoveUserCancellationSourceException : Exception
    {
        public RemoveUserCancellationSourceException()
        {
        }

        public RemoveUserCancellationSourceException(string message) : base(message)
        {
        }

        public RemoveUserCancellationSourceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RemoveUserCancellationSourceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}