using System.Data.SqlClient;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Common.Log;
using Lykke.Common.MsSql;
using MAVN.Service.WalletManagement.Domain;
using MAVN.Service.WalletManagement.Domain.Models;
using MAVN.Service.WalletManagement.MsSqlRepositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace MAVN.Service.WalletManagement.MsSqlRepositories.Repositories
{
    public class TransfersRepository : ITransfersRepository
    {
        private readonly IDbContextFactory<WalletManagementContext> _contextFactory;
        private readonly ILog _log;

        public TransfersRepository(MsSqlContextFactory<WalletManagementContext> contextFactory, ILogFactory logFactory)
        {
            _contextFactory = contextFactory;
            _log = logFactory.CreateLog(this);
        }

        public async Task AddAsync(ITransfer model)
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                var entity = TransferEventDataEntity.Create(model);

                await context.TransferEventsData.AddAsync(entity);

                try
                {
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateException e)
                {
                    if (e.InnerException is SqlException sqlException
                        && sqlException.Number == MsSqlErrorCodes.PrimaryKeyConstraintViolation)
                    {
                        _log.Warning("Error on transfer event context saving", e);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        public async Task<ITransfer> GetAsync(string id)
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                var result = await context.TransferEventsData.FindAsync(id);

                return result;
            }
        }

        public async Task DeleteAsync(string id)
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                var entity = new TransferEventDataEntity{Id = id};

                context.TransferEventsData.Attach(entity);

                context.Remove(entity);

                await context.SaveChangesAsync();
            }
        }
    }
}
