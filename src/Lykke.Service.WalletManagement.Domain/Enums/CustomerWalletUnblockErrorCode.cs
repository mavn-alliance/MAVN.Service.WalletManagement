namespace Lykke.Service.WalletManagement.Domain.Enums
{
    public enum CustomerWalletUnblockErrorCode
    {
        /// <summary>There is no error</summary>
        None,
    
        /// <summary>Specified Customer was not found</summary>
        CustomerNotFound,
    
        /// <summary>Specified Customer is not blocked</summary>
        CustomerNotBlocked
    }
}