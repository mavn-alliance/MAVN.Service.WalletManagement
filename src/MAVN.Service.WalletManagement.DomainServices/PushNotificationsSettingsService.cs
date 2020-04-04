using MAVN.Service.WalletManagement.Domain.Services;

namespace MAVN.Service.WalletManagement.DomainServices
{
    public class PushNotificationsSettingsService : IPushNotificationsSettingsService
    {
        public PushNotificationsSettingsService(string p2PTransferSucceededForSenderTemplateId,
            string p2PTransferSucceededForReceiverTemplateId, 
            string p2PTransferFailedForSenderTemplateId,
            string campaignCompletedTemplateId, 
            string campaignConditionCompletedTemplateId,
            string partnerPaymentCreatedTemplateId, 
            string paymentTransferAcceptedTemplateId,
            string paymentTransferRejectedTemplateId)
        {
            P2PTransferSucceededForSenderTemplateId = p2PTransferSucceededForSenderTemplateId;
            P2PTransferSucceededForReceiverTemplateId = p2PTransferSucceededForReceiverTemplateId;
            P2PTransferFailedForSenderTemplateId = p2PTransferFailedForSenderTemplateId;
            CampaignCompletedTemplateId = campaignCompletedTemplateId;
            CampaignConditionCompletedTemplateId = campaignConditionCompletedTemplateId;
            PartnerPaymentCreatedTemplateId = partnerPaymentCreatedTemplateId;
            PaymentTransferAcceptedTemplateId = paymentTransferAcceptedTemplateId;
            PaymentTransferRejectedTemplateId = paymentTransferRejectedTemplateId;
        }

        public string P2PTransferSucceededForSenderTemplateId { get; }
        public string P2PTransferSucceededForReceiverTemplateId { get; }
        public string P2PTransferFailedForSenderTemplateId { get; }
        public string CampaignCompletedTemplateId { get; }
        public string CampaignConditionCompletedTemplateId { get; }
        public string PartnerPaymentCreatedTemplateId { get; }
        public string PaymentTransferAcceptedTemplateId { get; }
        public string PaymentTransferRejectedTemplateId { get; }
    }
}
