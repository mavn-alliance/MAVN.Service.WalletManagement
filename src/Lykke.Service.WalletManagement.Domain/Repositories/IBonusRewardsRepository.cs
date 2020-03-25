using System;
using System.Threading.Tasks;
using Lykke.Service.WalletManagement.Domain.Models;

namespace Lykke.Service.WalletManagement.Domain.Repositories
{
    public interface IBonusRewardsRepository
    {
        Task AddAsync(IBonusIssued model);

        Task<IBonusIssued> GetAsync(Guid operationId);

        Task DeleteAsync(Guid operationId);
    }
}
