using JetBrains.Annotations;
using Lykke.SettingsReader.Attributes;

namespace MAVN.Service.WalletManagement.Settings
{
    [UsedImplicitly]
    public class DbSettings
    {
        [AzureTableCheck]
        public string LogsConnString { get; set; }

        [SqlCheck]
        public string MsSqlConnString { get; set; }
    }
}
