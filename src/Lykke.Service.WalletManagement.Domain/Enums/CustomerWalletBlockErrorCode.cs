namespace Lykke.Service.WalletManagement.Domain.Enums
{
    public enum CustomerWalletBlockErrorCode
    {
        /// <summary>There is no error</summary>
        None,
    
        /// <summary>Specified Customer was not found</summary>
        CustomerNotFound,
    
        /// <summary>Specified Customer's Wallet is already blocked</summary>
        CustomerWalletAlreadyBlocked
    }
}