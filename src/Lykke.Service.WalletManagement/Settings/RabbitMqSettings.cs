using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.WalletManagement.Settings
{
    public class RabbitMqSettings
    {
        [AmqpCheck]
        public string RabbitMqConnectionString { get; set; }

        [AmqpCheck]
        public string NotificationRabbitMqConnectionString { get; set; }
    }
}
