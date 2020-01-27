namespace Viia.Core.Config.Configurers
{
    /// <summary>
    ///     Configuration builder for configuring logging
    /// </summary>
    public class LoggingConfigurationBuilder : ConfigurationBuilder
    {
        /// <summary>
        ///     Constructs the builder
        /// </summary>
        public LoggingConfigurationBuilder(IRegistrar registrar) : base(registrar) { }

        /// <summary>
        ///     Configures Viia to not log anything at all.
        /// </summary>
        public void None()
        {
            Use(new NullLoggerFactory());
        }

        /// <summary>
        ///     Configures Viia get its logger using specified factory.
        /// </summary>
        public void Use(ViiaLoggerFactory factory)
        {
            ViiaLoggerFactory.Current = factory;
        }

        /// <summary>
        ///     Configures Viia to log using the console.
        /// </summary>
        public void UseConsole(Logger.Level minLevel = Logger.Level.Info)
        {
            Use(new ConsoleLoggerFactory(minLevel: minLevel));
        }

        /// <summary>
        ///     Configures Viia to log using <see cref="Debug.WriteLine(string)" />.
        /// </summary>
        public void UseDebug(Logger.Level minLevel = Logger.Level.Info)
        {
            Use(new DebugLoggerFactory(minLevel: minLevel));
        }
    }
}
