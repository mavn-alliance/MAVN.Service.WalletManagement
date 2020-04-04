using JetBrains.Annotations;

namespace MAVN.Service.WalletManagement.Client
{
    /// <summary>
    /// WalletManagement client interface.
    /// </summary>
    [PublicAPI]
    public interface IWalletManagementClient
    {
        // Make your app's controller interfaces visible by adding corresponding properties here.
        // NO actual methods should be placed here (these go to controller interfaces, for example - IWalletManagementApi).
        // ONLY properties for accessing controller interfaces are allowed.

        /// <summary>Wallets Api interface</summary>
        IWalletManagementApi Api { get; }
    }
}
