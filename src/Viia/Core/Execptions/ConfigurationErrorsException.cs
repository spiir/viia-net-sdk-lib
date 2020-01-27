using System;
using System.Runtime.Serialization;

namespace Viia.Core.Execptions
{
    /// <summary>
    ///     Exceptions thrown by the configuration system
    /// </summary>
    [Serializable]
    public class ConfigurationErrorsException : ApplicationException
    {
        /// <summary>
        /// </summary>
        public ConfigurationErrorsException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        /// <summary>
        ///     Creates the exception with information about which type it was attempted to configure, including a message
        /// </summary>
        public ConfigurationErrorsException(Type resolvedType, string message, params object[] objs)
            : base(CreateMessage(resolvedType, message, objs)) { }

        private static string CreateMessage(Type resolvedType, string message, object[] objs)
        {
            return $"Error when getting {resolvedType}: {string.Format(message, objs)}";
        }
    }
}
