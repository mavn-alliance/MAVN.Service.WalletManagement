using System.Data.Common;
using Lykke.Common.MsSql;
using MAVN.Service.WalletManagement.MsSqlRepositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace MAVN.Service.WalletManagement.MsSqlRepositories
{
    public class WalletManagementContext : MsSqlContext
    {
        private const string Schema = "wallet_management";

        internal DbSet<BonusIssuedEventDataEntity> BonusIssuedEventsData { get; set; }
        internal DbSet<TransferEventDataEntity> TransferEventsData { get; set; }
        internal DbSet<WalletFlagsEntity> WalletFlags { get; set; }

        public WalletManagementContext(DbContextOptions contextOptions)
            : base(Schema, contextOptions)
        {
        }

        public WalletManagementContext()
            : base(Schema)
        {
        }

        public WalletManagementContext(string connectionString, bool isTraceEnabled)
            : base(Schema, connectionString, isTraceEnabled)
        {
        }

        public WalletManagementContext(DbContextOptions options, bool isForMocks = false)
            : base(Schema, options, isForMocks)
        {
        }

        public WalletManagementContext(DbConnection dbConnection)
            : base(Schema, dbConnection)
        {
        }

        protected override void OnLykkeModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
