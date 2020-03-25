using System.Threading.Tasks;
using Lykke.Service.WalletManagement.Domain.Models;

namespace Lykke.Service.WalletManagement.Domain.Repositories
{
    public interface IWalletFlagsRepository
    {
        Task<IWalletFlags> CreateOrUpdateAsync(string customerId, bool isBlocked);
        
        Task<IWalletFlags> GetByCustomerIdAsync(string customerId);
    }
}