using Lykke.Service.WalletManagement.Domain.Models;

namespace Lykke.Service.WalletManagement.Settings.Notifications.EmailNotifications
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
