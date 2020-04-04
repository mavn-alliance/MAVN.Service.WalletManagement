using MAVN.Service.WalletManagement.Client.Enums;

namespace MAVN.Service.WalletManagement.Client.Models.Responses
{
    /// <summary>
    /// Class which holds response for attempt to block Customer's Wallet
    /// </summary>
    public class CustomerWalletBlockResponse
    {
        /// <summary>
        /// Holds information about errors
        /// </summary>
        public CustomerWalletBlockError Error { set; get; }
    }
}