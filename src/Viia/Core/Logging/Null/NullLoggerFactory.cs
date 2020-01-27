using System;

namespace Viia.Core.Logging.Null
{
    internal class NullLoggerFactory : ViiaLoggerFactory
    {
        private static readonly NullLogger _instance = new NullLogger();

        public override Logger GetLogger(Type ownerType)
        {
            return _instance;
        }

        private class NullLogger : Logger
        {
            public override void Debug(string message, params object[] objs) { }

            public override void Error(string message, params object[] objs) { }

            public override void Error(Exception exception, string message, params object[] objs) { }

            public override void Info(string message, params object[] objs) { }

            public override void Warn(string message, params object[] objs) { }

            public override void Warn(Exception exception, string message, params object[] objs) { }
        }
    }
}
