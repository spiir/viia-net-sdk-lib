using System;
using System.Linq;
using Viia.Core.Execptions;
using Viia.Core.Tokens;

namespace Viia.Core.Config.Configurers
{
    internal class ClientConfigurationBuilder : IOptionalConfiguration<IClient>
    {
        private readonly ConfigurationContainer _container = new ConfigurationContainer();

        public IClient Create()
        {
            FillInDefaults();

            var resolutionContext = _container.CreateContext();
            var commandProcessor = resolutionContext.Get<IClient>();

            return commandProcessor;
        }

        public IOptionalConfiguration<IClient> HttpClient(Action<HttpClientConfigurationBuilder> configure)
        {
            configure?.Invoke(new HttpClientConfigurationBuilder(_container));
            return this;
        }

        public IOptionalConfiguration<IClient> Logging(Action<LoggingConfigurationBuilder> configure)
        {
            configure?.Invoke(new LoggingConfigurationBuilder(_container));
            return this;
        }

        public IOptionalConfiguration<IClient> Options(Action<OptionsConfigurationBuilder> configure)
        {
            configure?.Invoke(new OptionsConfigurationBuilder(_container));
            return this;
        }

        public IOptionalConfiguration<IClient> TokenRepository(Action<TokenRepositoryConfigurationBuilder> configure)
        {
            configure?.Invoke(new TokenRepositoryConfigurationBuilder(_container));
            return this;
        }

        private void FillInDefaults()
        {
            if (_container.HasService<IClient>(true))
            {
                throw new ConfigurationErrorsException(typeof(IClient),
                                                       "Cannot register the real Client because the configuration container already contains a primary registration for IClient");
            }

            _container.Register<IClient>(context =>
                                         {
                                             var options = new Options();

                                             var tokenRepository = context.Get<ITokenRepository>();

                                             context.GetAll<Action<Options>>()
                                                    .ToList()
                                                    .ForEach(action => action(options));

                                             var client = new Client(tokenRepository, options);

                                             client.Disposed += context.Dispose;

                                             client.Initialize();

                                             return client;
                                         });

            if (!_container.HasService<ITokenRepository>(true))
                _container.Register<ITokenRepository>(context => new DefaultTokenRepository());

            //if (!_container.HasService<IDomainEventSerializer>(true))
            //{
            //    _container.Register<IDomainEventSerializer>(context => new JsonDomainEventSerializer());
            //}

            //if (!_container.HasService<IEventDispatcher>(true))
            //{
            //    _container.Register<IEventDispatcher>(context => new NullEventDispatcher());
            //}
        }
    }
}
