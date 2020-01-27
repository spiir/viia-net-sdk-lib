using System;

namespace Viia.Core.Config.Configurers
{
    /// <summary>
    ///     Configuration builder that allows for configuring other things
    /// </summary>
    public interface IOptionalConfiguration<T>
    {
        /// <summary>
        ///     Finishes off the configuration (returns an implementation of <see cref="ICommandProcessor" /> if you're configuring
        ///     Cirqus 4real, or the TestContext if you're going to do some testing)
        /// </summary>
        T Create();

        /// <summary>
        ///     Begins the aggregate root repository configuration, which can be completed by supplying an action that makes an
        ///     additional call to the passed-in <see cref="TokenRepositoryConfigurationBuilder" />
        /// </summary>
        IOptionalConfiguration<T> HttpClient(Action<HttpClientConfigurationBuilder> configure);

        /// <summary>
        ///     Begins the additional options configuration, which can be completed by supplying an action that makes an additional
        ///     call to the passed-in <see cref="OptionsConfigurationBuilder" />
        /// </summary>
        IOptionalConfiguration<T> Options(Action<OptionsConfigurationBuilder> configure);

        /// <summary>
        ///     Begins the aggregate root repository configuration, which can be completed by supplying an action that makes an
        ///     additional call to the passed-in <see cref="TokenRepositoryConfigurationBuilder" />
        /// </summary>
        IOptionalConfiguration<T> TokenRepository(Action<TokenRepositoryConfigurationBuilder> configure);

        /// <summary>
        /// Begins the logging configuration, which can be completed by supplying an action that makes an additional call to the passed-in <see cref="LoggingConfigurationBuilder"/>
        /// </summary>
        IOptionalConfiguration<T> Logging(Action<LoggingConfigurationBuilder> configure);
    }
}
