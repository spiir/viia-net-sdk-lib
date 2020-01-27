using System.Threading.Tasks;
using Viia.Core.Http;

#if NETFRAMEWORK
namespace Viia.NetFull
{
    public class DefaultHttpClient : IHttpClient
    {
        public Task<TOut> Execute<TOut, TIn>()
        {
            throw new System.NotImplementedException();
        }
    }
}
#endif
