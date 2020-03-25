using Lykke.Service.WalletManagement.Settings.Notifications.EmailNotifications;
using Lykke.Service.WalletManagement.Settings.Notifications.PushNotifications;

namespace Lykke.Service.WalletManagement.Settings.Notifications
{
    public class NotificationsSettings
    {
        public PushNotificationsSettings PushNotifications { get; set; }

        public EmailNotificationsSettings EmailNotifications { get; set; }
    }
}
