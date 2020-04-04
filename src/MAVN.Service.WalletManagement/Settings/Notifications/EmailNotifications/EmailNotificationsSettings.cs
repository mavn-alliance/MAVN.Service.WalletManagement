using MAVN.Service.WalletManagement.Domain.Models;

namespace MAVN.Service.WalletManagement.Settings.Notifications.EmailNotifications
{
    public class EmailNotificationsSettings
    {
        public EmailTemplateSettings P2PTransferSucceededForSender { get; set; }
        public EmailTemplateSettings P2PTransferSucceededForReceiver { get; set; }
        public EmailTemplateSettings P2PTransferFailedForSender { get; set; }
        public EmailTemplateSettings PaymentTransferAccepted { get; set; }
        public EmailTemplateSettings PaymentTransferRejected { get; set; }
    }
}
