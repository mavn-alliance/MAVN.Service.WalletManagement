namespace Lykke.Service.WalletManagement.Client.Enums
{
    /// <summary>
    /// Holds information about the Errors involving the Customer blocking procedure
    /// </summary>
    public enum CustomerWalletBlockError
    {
        /// <summary>
        /// There is no error
        /// </summary>
        None,
        
        /// <summary>
        /// Specified Customer was not found
        /// </summary>
        CustomerNotFound,
        
        /// <summary>
        /// Specified Customer's Wallet is already blocked
        /// </summary>
        CustomerWalletAlreadyBlocked
    }
}