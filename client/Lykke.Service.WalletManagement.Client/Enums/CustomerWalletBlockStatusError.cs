namespace Lykke.Service.WalletManagement.Client.Enums
{
    /// <summary>
    /// Holds information about the Errors involving getting Customer Wallet's block status
    /// </summary>
    public enum CustomerWalletBlockStatusError
    {
        /// <summary>
        /// There is no error
        /// </summary>
        None,
        
        /// <summary>
        /// Specified Customer was not found
        /// </summary>
        CustomerNotFound
    }
}