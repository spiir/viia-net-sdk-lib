using System.Threading.Tasks;

namespace Viia.Core.Tokens
{
    /// <summary>
    ///     Repository of tokens.
    /// </summary>
    public interface ITokenRepository
    {
        /// <summary>
        ///     Checks whether a token of the specified ID exists.
        /// </summary>
        Task<bool> Exists(string consentId);

        /// <summary>
        /// </summary>
        Task<Token> Get(string consentId);

        /// <summary>
        /// </summary>
        Task Set(string consentId, Token token);
    }
}
