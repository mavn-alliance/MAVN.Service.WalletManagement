namespace MAVN.Service.WalletManagement.Domain.Services
{
    public interface IPushNotificationsSettingsService
    {
        string P2PTransferSucceededForSenderTemplateId { get; }

        string P2PTransferSucceededForReceiverTemplateId { get; }

        string P2PTransferFailedForSenderTemplateId { get; }

        string CampaignCompletedTemplateId { get; }

        string CampaignConditionCompletedTemplateId { get; }

        string PartnerPaymentCreatedTemplateId { get; }
    }
}
