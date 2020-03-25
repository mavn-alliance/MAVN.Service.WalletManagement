using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.WalletManagement.Client.Models.Requests
{
    /// <summary>
    /// Used for requesting blocking Customer's Wallet
    /// </summary>
    public class CustomerWalletBlockRequest
    {
        /// <summary>
        /// Id of the Customer
        /// </summary>
        [Required]
        public string CustomerId { get; set; }
    }
}