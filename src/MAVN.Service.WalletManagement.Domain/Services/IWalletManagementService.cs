using System.Threading.Tasks;
using MAVN.Service.WalletManagement.Domain.Enums;

namespace MAVN.Service.WalletManagement.Domain.Services
{
    public interface IWalletManagementService
    {
        Task<CustomerWalletBlockErrorCode> BlockCustomerWalletAsync(string customerId);

        Task<CustomerWalletUnblockErrorCode> UnblockCustomerWalletAsync(string customerId);

        Task<bool?> IsCustomerWalletBlockedAsync(string customerId);
    }
}