using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Viia.Core.Http
{
    public class DefaultHttpClient : IHttpClient
    {
        private const double DEFAULT_TIMEOUT = 30;

        public void Dispose() { }

        public async Task<TOut> Execute<TOut, TIn>(string relativeUrl,
                                                   HttpMethod method,
                                                   TIn body,
                                                   CancellationToken cancellationToken,
                                                   string accessToken = null)
        {
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(DEFAULT_TIMEOUT);
                client.BaseAddress = new Uri("");
                client.DefaultRequestHeaders.Clear();
                var httpRequestMessage = new HttpRequestMessage(method, relativeUrl)
                                         {
                                             Content = new StringContent(JsonConvert.SerializeObject(body),
                                                                         Encoding.UTF8,
                                                                         "application/json")
                                         };

                if (string.IsNullOrWhiteSpace(accessToken))
                    httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await client.SendAsync(httpRequestMessage, cancellationToken);

                if (!result?.IsSuccessStatusCode ?? true)
                    throw new Exception();

                var responseContent = await result.Content.ReadAsStringAsync()
                                                  .ConfigureAwait(false);
                var response = JsonConvert.DeserializeObject<TOut>(responseContent);

                return response;
            }
        }
    }
}
