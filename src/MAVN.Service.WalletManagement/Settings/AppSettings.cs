using JetBrains.Annotations;
using Lykke.Sdk.Settings;
using MAVN.Service.Campaign.Client;
using MAVN.Service.CustomerProfile.Client;
using MAVN.Service.PrivateBlockchainFacade.Client;
using MAVN.Service.WalletManagement.Settings.Notifications;

namespace MAVN.Service.WalletManagement.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AppSettings : BaseAppSettings
    {
        public WalletManagementSettings WalletManagementService { get; set; }
        public CustomerProfileServiceClientSettings CustomerProfileService { get; set; }
        public PrivateBlockchainFacadeServiceClientSettings PrivateBlockchainFacadeService { get; set; }
        public CampaignServiceClientSettings CampaignService { get; set; }
        public Constants Constants { get; set; }
    }
}
