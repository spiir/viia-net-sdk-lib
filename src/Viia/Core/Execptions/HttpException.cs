using System;
using System.Net;
using System.Runtime.Serialization;

namespace Viia.Core.Execptions
{
    /// <summary>
    ///     Exceptions thrown by the configuration system if a factory method could not be executed
    /// </summary>
    [Serializable]
    public class HttpException : ApplicationException
    {
        /// <summary>
        /// </summary>
        public HttpException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        /// <summary>
        ///     Creates the exception with information about which type it was attempted to resolve, including a message
        /// </summary>
        public HttpException(HttpStatusCode statuscode, string message, params object[] objs)
            : base(CreateMessage(statuscode, message, objs)) { }

        private static string CreateMessage(HttpStatusCode statuscode, string message, object[] objs)
        {
            return $"Error when getting {statuscode}: {string.Format(message, objs)}";
        }
    }
}
