using Lykke.HttpClientGenerator;

namespace Lykke.Service.WalletManagement.Client
{
    /// <summary>
    /// WalletManagement API aggregating interface.
    /// </summary>
    public class WalletManagementClient : IWalletManagementClient
    {
        // Note: Add similar Api properties for each new service controller

        /// <summary>Inerface to WalletManagement Api.</summary>
        public IWalletManagementApi Api { get; private set; }

        /// <summary>C-tor</summary>
        public WalletManagementClient(IHttpClientGenerator httpClientGenerator)
        {
            Api = httpClientGenerator.Generate<IWalletManagementApi>();
        }
    }
}
