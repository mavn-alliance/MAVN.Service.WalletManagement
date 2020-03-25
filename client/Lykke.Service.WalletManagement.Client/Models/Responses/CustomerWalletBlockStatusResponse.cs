using Lykke.Service.WalletManagement.Client.Enums;

namespace Lykke.Service.WalletManagement.Client.Models.Responses
{
    /// <summary>
    /// Class which holds response for getting block status of Customer's Wallet
    /// </summary>
    public class CustomerWalletBlockStatusResponse
    {
        /// <summary>
        /// Holds information about errors
        /// </summary>
        public CustomerWalletBlockStatusError Error { set; get; }
        
        /// <summary>
        /// Holds status of the Customer's Wallet if there were no errors
        /// </summary>
        public CustomerWalletActivityStatus? Status { set; get; }
    }
}