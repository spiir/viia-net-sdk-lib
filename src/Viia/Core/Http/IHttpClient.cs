using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Viia.Core.Http
{
    public interface IHttpClient
    {
        Task<TOut> Execute<TOut, TIn>(string relativeUrl,
                                      HttpMethod method,
                                      TIn body,
                                      CancellationToken cancellationToken,
                                      TimeSpan? timeout = null,
                                      string accessToken = null);
    }
}
