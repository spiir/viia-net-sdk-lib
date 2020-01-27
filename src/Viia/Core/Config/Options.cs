using Viia.Core.Logging;

namespace Viia.Core.Config
{
    /// <summary>
    ///     Represents additional options for how Viia does its thing
    /// </summary>
    public class Options
    {
        /// <summary>
        ///     Default environment to perform request against
        /// </summary>
        public const Environment DefaultEnvironment = Environment.Sandbox;

        /// <summary>
        ///     Default number of retries for command processing
        /// </summary>
        public const int DefaultMaxRetries = 3;

        /// <summary>
        ///     Configures the environment to perform request against.
        /// </summary>
        public Environment Environment { get; set; }

        /// <summary>
        ///     Configures the number of retries to perform when processing commands.
        /// </summary>
        public int MaxRetries { get; set; }

        /// <summary>
        ///     Gets/sets the global logger factory. THIS SETTING AFFECTS HOW Viia LOGS, IMMEDIATELY AND GLOBALLY. The setting is
        ///     here for convenience
        /// </summary>
        public ViiaLoggerFactory GlobalLoggerFactory
        {
            get => ViiaLoggerFactory.Current;
            set => ViiaLoggerFactory.Current = value;
        }

        /// <summary>
        ///     Constructs the default options
        /// </summary>
        public Options()
        {
            MaxRetries = DefaultMaxRetries;
            Environment = DefaultEnvironment;
        }
    }
}
