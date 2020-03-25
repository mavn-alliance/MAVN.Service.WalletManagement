using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.WalletManagement.Client.Models.Requests
{
    /// <summary>
    /// Used for requesting unblocking Customer's Wallet
    /// </summary>
    public class CustomerWalletUnblockRequest
    {
        /// <summary>
        /// Id of the Customer
        /// </summary>
        [Required]
        public string CustomerId { get; set; }
    }
}