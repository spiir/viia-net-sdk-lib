#if NETSTANDARD
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Polly;
using Viia.Core.Http;

namespace Viia.NetStandard
{
    public class DefaultHttpClient : IHttpClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IAsyncPolicy _policy;

        public DefaultHttpClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public Task<TOut> Execute<TOut, TIn>(string relativeUrl,
                                             HttpMethod method,
                                             TIn body,
                                             CancellationToken cancellationToken,
                                             TimeSpan? timeout = null,
                                             string accessToken = null)
        {
            throw new NotImplementedException();
        }
    }
}
#endif
