using System.ComponentModel.DataAnnotations;

namespace MAVN.Service.WalletManagement.Client.Models.Requests
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