namespace Lykke.Service.WalletManagement.Client.Enums
{
    /// <summary>
    /// Holds information about the Errors involving the Customer unblocking procedure
    /// </summary>
    public enum CustomerWalletUnblockError
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
        /// Specified Customer is not blocked
        /// </summary>
        CustomerWalletNotBlocked
    }
}