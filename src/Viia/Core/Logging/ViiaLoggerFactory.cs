using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Viia.Core.Logging.Null;

namespace Viia.Core.Logging
{
    /// <summary>
    ///     Abstract logger factory that can be used to install a global logger factory (by setting <see cref="Current" />).
    ///     Classes that want to log stuff should subscribe to the <see cref="Changed" /> event and (possibly re-)set their
    ///     logger instance from the factory passed to the event handler.
    /// </summary>
    public abstract class ViiaLoggerFactory
    {
        private static readonly List<Action<ViiaLoggerFactory>> _changedHandlers = new List<Action<ViiaLoggerFactory>>();
        private static readonly object _changedHandlersLock = new object();

        private static ViiaLoggerFactory _current = new NullLoggerFactory();

        /// <summary>
        ///     Event that is raised whenever the global logger factory is changed. Also immediately raises the event
        ///     for each new subscriber, so that their logger gets initialized.
        /// </summary>
        public static event Action<ViiaLoggerFactory> Changed
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            add
            {
                lock (_changedHandlersLock)
                {
                    _changedHandlers.Add(value);
                    value(_current);
                }
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            remove
            {
                lock (_changedHandlersLock)
                {
                    _changedHandlers.Remove(value);
                }
            }
        }

        /// <summary>
        ///     Gets/sets the global logger factory
        /// </summary>
        public static ViiaLoggerFactory Current
        {
            get => _current;
            set
            {
                _current = value ?? new NullLoggerFactory();

                lock (_changedHandlersLock)
                {
                    foreach (var handler in _changedHandlers)
                    {
                        handler(_current);
                    }
                }
            }
        }

        /// <summary>
        ///     Returns a logger with the given <paramref name="ownerType" /> as its name
        /// </summary>
        public abstract Logger GetLogger(Type ownerType);

        /// <summary>
        ///     Gets a logger with the calling class's name (takes a walk down the call stack)
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public Logger GetCurrentClassLogger()
        {
            var type = new StackFrame(1).GetMethod().DeclaringType;

            return _current.GetLogger(type?.ReflectedType ?? typeof(Logger));
        }
    }
}
