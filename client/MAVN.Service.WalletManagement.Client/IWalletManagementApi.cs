using System.Threading.Tasks;
using JetBrains.Annotations;
using MAVN.Service.WalletManagement.Client.Models.Requests;
using MAVN.Service.WalletManagement.Client.Models.Responses;
using Refit;

namespace MAVN.Service.WalletManagement.Client
{
    /// <summary>
    /// WalletManagement client API interface.
    /// </summary>
    [PublicAPI]
    public interface IWalletManagementApi
    {
        /// <summary>
        /// Transfers a Balance from One Customer Wallet to Another
        /// </summary>
        /// <returns><see cref="TransferBalanceResponse"/></returns>
        [Post("/api/wallets/transfer-balance")]
        Task<TransferBalanceResponse> TransferBalanceAsync([Body] TransferBalanceRequestModel request);
        
        /// <summary>
        /// Blocks Customer's Wallet
        /// </summary>
        /// <param name="request" cref="CustomerWalletBlockRequest"></param>
        /// <returns><see cref="CustomerWalletBlockResponse"/></returns>
        [Post("/api/wallets/block")]
        Task<CustomerWalletBlockResponse> CustomerWalletBlockAsync(CustomerWalletBlockRequest request);

        /// <summary>
        /// Unblocks Customer's Wallet
        /// </summary>
        /// <param name="request" cref="CustomerWalletUnblockRequest"></param>
        /// <returns><see cref="CustomerWalletUnblockResponse"/></returns>
        [Post("/api/wallets/unblock")]
        Task<CustomerWalletUnblockResponse> CustomerWalletUnblockAsync(CustomerWalletUnblockRequest request);

        /// <summary>
        /// Gets block status of the Customer's Wallet
        /// </summary>
        /// <param name="customerId">Id of the Customer</param>
        /// <returns><see cref="CustomerWalletBlockStatusResponse"/></returns>
        [Get("/api/wallets/blockStatus/{customerId}")]
        Task<CustomerWalletBlockStatusResponse> GetCustomerWalletBlockStateAsync(string customerId);
    }
}
