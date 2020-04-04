using System;
using System.Threading.Tasks;
using MAVN.Service.WalletManagement.Domain.Models;

namespace MAVN.Service.WalletManagement.Domain.Repositories
{
    public interface IBonusRewardsRepository
    {
        Task AddAsync(IBonusIssued model);

        Task<IBonusIssued> GetAsync(Guid operationId);

        Task DeleteAsync(Guid operationId);
    }
}
