using MAVN.Service.WalletManagement.Settings.Notifications.EmailNotifications;
using MAVN.Service.WalletManagement.Settings.Notifications.PushNotifications;

namespace MAVN.Service.WalletManagement.Settings.Notifications
{
    public class NotificationsSettings
    {
        public PushNotificationsSettings PushNotifications { get; set; }

        public EmailNotificationsSettings EmailNotifications { get; set; }
    }
}
