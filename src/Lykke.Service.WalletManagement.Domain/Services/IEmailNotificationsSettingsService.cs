using Lykke.Service.WalletManagement.Domain.Models;

namespace Lykke.Service.WalletManagement.Domain.Services
{
    public interface IEmailNotificationsSettingsService
    {
        EmailTemplateSettings P2PSuccessForSenderTemplateSettings { get; }

        EmailTemplateSettings P2PSuccessForReceiverTemplateSettings { get; }

        EmailTemplateSettings P2PFailureForSenderTemplateSettings { get; }

        EmailTemplateSettings PaymentTransferAcceptedTemplateSettings { get; }

        EmailTemplateSettings PaymentTransferRejectedTemplateSettings { get; }
    }
}
