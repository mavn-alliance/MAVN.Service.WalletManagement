using System.Threading.Tasks;
using MAVN.Persistence.PostgreSQL.Legacy;
using MAVN.Service.WalletManagement.Domain.Models;
using MAVN.Service.WalletManagement.Domain.Repositories;
using MAVN.Service.WalletManagement.MsSqlRepositories.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace MAVN.Service.WalletManagement.MsSqlRepositories.Repositories
{
    public class WalletFlagsRepository : IWalletFlagsRepository
    {
        private readonly IDbContextFactory<WalletManagementContext> _contextFactory;

        public WalletFlagsRepository(
            PostgreSQLContextFactory<WalletManagementContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        
        public async Task<IWalletFlags> CreateOrUpdateAsync(string customerId, bool isBlocked)
        {
            var entity = WalletFlagsEntity.Create(customerId, isBlocked);
            
            using (var context = _contextFactory.CreateDataContext())
            {
                await context.WalletFlags.AddAsync(entity);

                try
                {
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateException e)
                {
                    if (e.InnerException is PostgresException sqlException &&
                        sqlException.SqlState == PostgresErrorCodes.UniqueViolation)
                    {
                        context.WalletFlags.Update(entity);

                        await context.SaveChangesAsync();
                    }
                    else throw;
                }
            }

            return entity;
        }

        public async Task<IWalletFlags> GetByCustomerIdAsync(string customerId)
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                var entity = await context.WalletFlags.FindAsync(customerId);

                return entity;
            }
        }
    }
}
