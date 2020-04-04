using System.ComponentModel.DataAnnotations;

namespace MAVN.Service.WalletManagement.Client.Models.Requests
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