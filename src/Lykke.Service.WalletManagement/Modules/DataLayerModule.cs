using System;
using Autofac;
using JetBrains.Annotations;
using Lykke.Common.MsSql;
using Lykke.Service.WalletManagement.Domain;
using Lykke.Service.WalletManagement.Domain.Repositories;
using Lykke.Service.WalletManagement.MsSqlRepositories;
using Lykke.Service.WalletManagement.MsSqlRepositories.Repositories;
using Lykke.Service.WalletManagement.Settings;
using Lykke.SettingsReader;

namespace Lykke.Service.WalletManagement.Modules
{
    [UsedImplicitly]
    public class DataLayerModule : Module
    {
        private readonly WalletManagementSettings _appSettings;

        public DataLayerModule(IReloadingManager<AppSettings> appSettings)
        {
            _appSettings = appSettings?.CurrentValue.WalletManagementService ?? throw new ArgumentNullException(nameof(appSettings));
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterMsSql(
                _appSettings.Db.MsSqlConnString,
                connString => new WalletManagementContext(connString, false),
                dbConn => new WalletManagementContext(dbConn));

            builder.RegisterType<BonusRewardsRepository>()
                .As<IBonusRewardsRepository>()
                .SingleInstance();

            builder.RegisterType<TransfersRepository>()
                .As<ITransfersRepository>()
                .SingleInstance();
            
            builder.RegisterType<WalletFlagsRepository>()
                .As<IWalletFlagsRepository>()
                .SingleInstance();
        }
    }
}
