using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Common.Log;
using Lykke.Common.MsSql;
using Lykke.Service.WalletManagement.Domain.Models;
using Lykke.Service.WalletManagement.Domain.Repositories;
using Lykke.Service.WalletManagement.MsSqlRepositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lykke.Service.WalletManagement.MsSqlRepositories.Repositories
{
    public class BonusRewardsRepository : IBonusRewardsRepository
    {
        private readonly IDbContextFactory<WalletManagementContext> _contextFactory;
        private readonly ILog _log;

        public BonusRewardsRepository(MsSqlContextFactory<WalletManagementContext> contextFactory, ILogFactory logFactory)
        {
            _contextFactory = contextFactory;
            _log = logFactory.CreateLog(this);
        }

        public async Task AddAsync(IBonusIssued model)
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                var entity = BonusIssuedEventDataEntity.Create(model);

                await context.BonusIssuedEventsData.AddAsync(entity);

                try
                {
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateException e)
                {
                    if (e.InnerException is SqlException sqlException
                        && sqlException.Number == MsSqlErrorCodes.PrimaryKeyConstraintViolation)
                    {
                        _log.Warning("Error on bonus issued event context saving", e);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        public async Task<IBonusIssued> GetAsync(Guid operationId)
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                var result = await context.BonusIssuedEventsData.FindAsync(operationId);

                return result;
            }
        }

        public async Task DeleteAsync(Guid operationId)
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                var entity = new BonusIssuedEventDataEntity{OperationId = operationId};

                context.BonusIssuedEventsData.Attach(entity);

                context.Remove(entity);

                await context.SaveChangesAsync();
            }
        }
    }
}
