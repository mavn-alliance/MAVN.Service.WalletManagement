using System.Threading.Tasks;
using MAVN.Service.WalletManagement.Domain.Models;

namespace MAVN.Service.WalletManagement.Domain.Repositories
{
    public interface IWalletFlagsRepository
    {
        Task<IWalletFlags> CreateOrUpdateAsync(string customerId, bool isBlocked);
        
        Task<IWalletFlags> GetByCustomerIdAsync(string customerId);
    }
}