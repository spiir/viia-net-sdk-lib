﻿using System;
using System.Runtime.Serialization;

namespace Viia.Core.Execptions
{
    /// <summary>
    ///     Exceptions thrown by the configuration system if a factory method could not be executed
    /// </summary>
    [Serializable]
    public class ResolutionException : ApplicationException
    {
        /// <summary>
        /// </summary>
        public ResolutionException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        /// <summary>
        ///     Creates the exception with information about which type it was attempted to resolve, including a message
        /// </summary>
        public ResolutionException(Type resolvedType, string message, params object[] objs)
            : base(CreateMessage(resolvedType, message, objs)) { }

        private static string CreateMessage(Type resolvedType, string message, object[] objs)
        {
            return $"Error when getting {resolvedType}: {string.Format(message, objs)}";
        }
    }
}
