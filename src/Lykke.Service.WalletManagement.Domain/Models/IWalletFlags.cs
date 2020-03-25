namespace Lykke.Service.WalletManagement.Domain.Models
{
    public interface IWalletFlags
    {
        string CustomerId { set; get; }
                
        bool IsBlocked { set; get; }
    }
}