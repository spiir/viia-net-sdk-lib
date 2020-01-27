using System;
using System.Net.Http;

namespace Viia.Core.Config.Configurers
{
    public class HttpClientConfigurationBuilder : ConfigurationBuilder
    {
        private readonly IRegistrar _registrar;

        public HttpClientConfigurationBuilder(IRegistrar registrar) : base(registrar)
        {
            _registrar = registrar ?? throw new ArgumentNullException(nameof(registrar));
        }

#if NETSTANDARD
        public void UseHttpFactory(IHttpClientFactory factory)
        {
            _registrar.RegisterInstance(factory);
        }
#endif
    }
}
