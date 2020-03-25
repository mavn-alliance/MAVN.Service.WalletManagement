using System.Data.SqlClient;
using System.Threading.Tasks;
using Lykke.Common.MsSql;
using Lykke.Service.WalletManagement.Domain.Models;
using Lykke.Service.WalletManagement.Domain.Repositories;
using Lykke.Service.WalletManagement.MsSqlRepositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lykke.Service.WalletManagement.MsSqlRepositories.Repositories
{
    public class WalletFlagsRepository : IWalletFlagsRepository
    {
        private readonly IDbContextFactory<WalletManagementContext> _contextFactory;

        public WalletFlagsRepository(
            MsSqlContextFactory<WalletManagementContext> contextFactory)
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
                    if (e.InnerException is SqlException sqlException &&
                        sqlException.Number == MsSqlErrorCodes.PrimaryKeyConstraintViolation)
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
