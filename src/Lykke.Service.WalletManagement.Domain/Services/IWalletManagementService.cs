using System.Threading.Tasks;
using Lykke.Service.WalletManagement.Domain.Enums;

namespace Lykke.Service.WalletManagement.Domain.Services
{
    public interface IWalletManagementService
    {
        Task<CustomerWalletBlockErrorCode> BlockCustomerWalletAsync(string customerId);

        Task<CustomerWalletUnblockErrorCode> UnblockCustomerWalletAsync(string customerId);

        Task<bool?> IsCustomerWalletBlockedAsync(string customerId);
    }
}