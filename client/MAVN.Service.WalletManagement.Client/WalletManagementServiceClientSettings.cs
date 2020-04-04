using Lykke.SettingsReader.Attributes;

namespace MAVN.Service.WalletManagement.Client 
{
    /// <summary>
    /// WalletManagement client settings.
    /// </summary>
    public class WalletManagementServiceClientSettings 
    {
        /// <summary>Service url.</summary>
        [HttpCheck("api/isalive")]
        public string ServiceUrl {get; set;}
    }
}
