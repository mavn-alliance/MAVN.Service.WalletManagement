using Lykke.Service.WalletManagement.Client.Enums;

namespace Lykke.Service.WalletManagement.Client.Models.Responses
{
    /// <summary>
    /// Class which holds response for attempt to unblock Customer's Wallet
    /// </summary>
    public class CustomerWalletUnblockResponse
    {
        /// <summary>
        /// Holds information about errors
        /// </summary>
        public CustomerWalletUnblockError Error { set; get; }
    }
}