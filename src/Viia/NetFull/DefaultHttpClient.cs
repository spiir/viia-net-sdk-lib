#if NETFRAMEWORK
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Polly;
using Viia.Core.Http;

namespace Viia.NetFull
{
    public class DefaultHttpClient : IHttpClient
    {
        private readonly IAsyncPolicy _policy;

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
