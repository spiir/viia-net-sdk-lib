using System;
using System.Threading.Tasks;
using Viia.Core.Config;
using Viia.Core.Config.Configurers;
using Viia.Core.Http;
using Viia.Core.Logging;
using Viia.Core.Tokens;

namespace Viia.Core
{
    public class Client : IClient, IDisposable
    {
        private static Logger _logger;

        private readonly ITokenRepository _tokenRepository;
        private readonly IHttpClient _httpClient;

        private bool _disposed;

        internal event Action Disposed = delegate { };

        /// <summary>
        ///     Accesses the options for this client. Mutating the options might/might not have any
        ///     effect if the client has been initialized
        /// </summary>
        public Options Options { get; }

        static Client()
        {
            ViiaLoggerFactory.Changed += f => _logger = f.GetCurrentClassLogger();
        }

        public Client(ITokenRepository tokenRepository, IHttpClient httpClient, Options options)
        {
            _tokenRepository = tokenRepository ?? throw new ArgumentNullException(nameof(tokenRepository));
            Options = options ?? throw new ArgumentNullException(nameof(options));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        ~Client()
        {
            Dispose(false);
        }

        public Task<string> Authenticate()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Task Exchange()
        {
            throw new NotImplementedException();
        }

        public Task GetAccounts(string userId)
        {
            throw new NotImplementedException();
        }

        public Task GetProviders()
        {
            throw new NotImplementedException();
        }

        public Task GetTransactions(string userId, string accountId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Initializes
        /// </summary>
        public Client Initialize()
        {
            return this;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                _logger.Info("Disposing client");
                _httpClient.Dispose();

                _disposed = true;

                Disposed();
            }
        }

        /// <summary>
        ///     Creates a configuration builder that can help you build a fully functional client
        /// </summary>
        public static IOptionalConfiguration<IClient> With()
        {
            return new ClientConfigurationBuilder();
        }
    }
}
