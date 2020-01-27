using System.Threading.Tasks;

namespace Viia.Core
{
    /// <summary>
    ///     Command processor API - basically just processes commands :)
    /// </summary>
    public interface IClient
    {
        /// <summary>
        ///     Processes the specified command by invoking the generic eventDispatcher method
        /// </summary>
        /// <exception cref=""></exception>
        Task<string> Authenticate();

        /// <summary>
        ///     Processes the specified command by invoking the generic eventDispatcher method
        /// </summary>
        /// <exception cref=""></exception>
        Task Exchange();

        /// <summary>
        ///     Processes the specified command by invoking the generic eventDispatcher method
        /// </summary>
        /// <exception cref=""></exception>
        Task GetAccounts(string userId);

        /// <summary>
        ///     Processes the specified command by invoking the generic eventDispatcher method
        /// </summary>
        /// <exception cref=""></exception>
        Task GetProviders();

        /// <summary>
        ///     Processes the specified command by invoking the generic eventDispatcher method
        /// </summary>
        /// <exception cref=""></exception>
        Task GetTransactions(string userId, string accountId);
    }
}
