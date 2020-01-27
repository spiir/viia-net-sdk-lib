using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Viia.Core.Tokens
{
    public class DefaultTokenRepository : ITokenRepository
    {
        private readonly ConcurrentDictionary<string, Token> _tokens = new ConcurrentDictionary<string, Token>();

        /// <summary>
        /// </summary>
        public Task<bool> Exists(string consentId)
        {
            return Task.FromResult(_tokens.ContainsKey(consentId));
        }

        /// <summary>
        /// </summary>
        public Task<Token> Get(string consentId)
        {
            _tokens.TryGetValue(consentId, out var token);
            return Task.FromResult(token);
        }

        /// <summary>
        /// </summary>
        public Task Set(string consentId, Token token)
        {
            _tokens.AddOrUpdate(consentId, token, (key, _) => token);
            return Task.FromResult(false);
        }
    }
}
