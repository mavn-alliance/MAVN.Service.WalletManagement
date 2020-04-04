using JetBrains.Annotations;
using Lykke.Sdk.Settings;
using Lykke.Service.Campaign.Client;
using Lykke.Service.CustomerProfile.Client;
using Lykke.Service.PaymentTransfers.Client;
using Lykke.Service.PrivateBlockchainFacade.Client;
using MAVN.Service.WalletManagement.Settings.Notifications;

namespace MAVN.Service.WalletManagement.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AppSettings : BaseAppSettings
    {
        public WalletManagementSettings WalletManagementService { get; set; }
        public CustomerProfileServiceClientSettings CustomerProfileService { get; set; }
        public PrivateBlockchainFacadeServiceClientSettings PrivateBlockchainFacadeService { get; set; }
        public PaymentTransfersServiceClientSettings PaymentTransfersService { get; set; }
        public CampaignServiceClientSettings CampaignService { get; set; }
        public Constants Constants { get; set; }
    }
}
