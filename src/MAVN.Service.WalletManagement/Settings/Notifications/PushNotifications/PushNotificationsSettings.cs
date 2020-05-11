namespace MAVN.Service.WalletManagement.Settings.Notifications.PushNotifications
{
    public class PushNotificationsSettings
    {
        public string P2PTransferSucceededForSenderTemplateId { get; set; }
        public string P2PTransferSucceededForReceiverTemplateId { get; set; }
        public string P2PTransferFailedForSenderTemplateId { get; set; }
        public string CampaignCompletedTemplateId { get; set; }
        public string CampaignConditionCompletedTemplateId { get; set; }
        public string PartnerPaymentCreatedTemplateId { get; set; }
    }
}
