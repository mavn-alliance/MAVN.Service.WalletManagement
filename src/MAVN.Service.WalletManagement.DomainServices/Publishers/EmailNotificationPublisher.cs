using Falcon.Numerics;
using Lykke.Common;
using Lykke.Common.Log;
using Lykke.RabbitMqBroker.Publisher;
using Lykke.Service.NotificationSystem.SubscriberContract;
using MAVN.Service.WalletManagement.Domain.Publishers;
using MAVN.Service.WalletManagement.Domain.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace MAVN.Service.WalletManagement.DomainServices.Publishers
{
    public class EmailNotificationPublisher : JsonRabbitPublisher<EmailMessageEvent>, IEmailNotificationsPublisher
    {
        private readonly IEmailNotificationsSettingsService _emailNotificationsSettingsService;
        private readonly string _customerSupportNumber;
        private readonly IMoneyFormatter _moneyFormatter;

        public EmailNotificationPublisher(
            string connectionString,
            string exchangeName,
            ILogFactory logFactory,
            IEmailNotificationsSettingsService emailNotificationsSettingsService,
            string customerSupportNumber,
            IMoneyFormatter moneyFormatter)
            : base(logFactory, connectionString, exchangeName)
        {
            _emailNotificationsSettingsService = emailNotificationsSettingsService;
            _customerSupportNumber = customerSupportNumber
                                     ?? throw new ArgumentNullException(nameof(customerSupportNumber));
            _moneyFormatter = moneyFormatter;
        }

        public Task SendP2PSucceededForSenderAsync(string customerId,
            string transactionId,
            Money18 amount,
            DateTime timestamp,
            Money18 currentBalance,
            string receiverEmail)
        {
            return PublishAsync(new EmailMessageEvent
            {
                CustomerId = customerId,
                MessageTemplateId = _emailNotificationsSettingsService.P2PSuccessForSenderTemplateSettings.MessageTemplateId,
                SubjectTemplateId = _emailNotificationsSettingsService.P2PSuccessForSenderTemplateSettings.SubjectTemplateId,
                Source = $"{AppEnvironment.Name} - {AppEnvironment.Version}",
                TemplateParameters = new Dictionary<string, string>
                {
                    {"ReceiverEmail", receiverEmail },
                    {"TransactionId", transactionId },
                    {"Amount", _moneyFormatter.FormatAmountToDisplayString(amount) },
                    {"Timestamp", timestamp.ToString(CultureInfo.InvariantCulture) },
                    {"CurrentBalance", _moneyFormatter.FormatAmountToDisplayString(currentBalance) }
                }
            });
        }

        public Task SendP2PSucceededForReceiverAsync(string customerId,
            string transactionId,
            Money18 amount,
            DateTime timestamp,
            Money18 currentBalance,
            string senderEmail)
        {
            return PublishAsync(new EmailMessageEvent
            {
                CustomerId = customerId,
                MessageTemplateId = _emailNotificationsSettingsService.P2PSuccessForReceiverTemplateSettings.MessageTemplateId,
                SubjectTemplateId = _emailNotificationsSettingsService.P2PSuccessForReceiverTemplateSettings.SubjectTemplateId,
                Source = $"{AppEnvironment.Name} - {AppEnvironment.Version}",
                TemplateParameters = new Dictionary<string, string>
                {
                    {"SenderEmail", senderEmail },
                    {"TransactionId", transactionId },
                    {"Amount", _moneyFormatter.FormatAmountToDisplayString(amount) },
                    {"Timestamp", timestamp.ToString(CultureInfo.InvariantCulture) },
                    {"CurrentBalance", _moneyFormatter.FormatAmountToDisplayString(currentBalance) }
                }
            });
        }

        public Task SendP2PFailedForSenderAsync(string customerId,
            string transactionId,
            Money18 amount,
            DateTime timestamp,
            Money18 currentBalance,
            string receiverEmail)
        {
            return PublishAsync(new EmailMessageEvent
            {
                CustomerId = customerId,
                MessageTemplateId = _emailNotificationsSettingsService.P2PFailureForSenderTemplateSettings.MessageTemplateId,
                SubjectTemplateId = _emailNotificationsSettingsService.P2PFailureForSenderTemplateSettings.SubjectTemplateId,
                Source = $"{AppEnvironment.Name} - {AppEnvironment.Version}",
                TemplateParameters = new Dictionary<string, string>
                {
                    {"ReceiverEmail", receiverEmail },
                    {"TransactionId", transactionId },
                    {"Amount", _moneyFormatter.FormatAmountToDisplayString(amount) },
                    {"Timestamp", timestamp.ToString(CultureInfo.InvariantCulture) },
                    {"CurrentBalance", _moneyFormatter.FormatAmountToDisplayString(currentBalance)}
                }
            });
        }

        public Task SendPaymentTransferAcceptedAsync(string customerId, string invoiceId, Money18 amount)
        {
            return PublishAsync(new EmailMessageEvent
            {
                CustomerId = customerId,
                MessageTemplateId = _emailNotificationsSettingsService.PaymentTransferAcceptedTemplateSettings.MessageTemplateId,
                SubjectTemplateId = _emailNotificationsSettingsService.PaymentTransferAcceptedTemplateSettings.SubjectTemplateId,
                Source = $"{AppEnvironment.Name} - {AppEnvironment.Version}",
                TemplateParameters = new Dictionary<string, string>
                {
                    {"InvoiceId", invoiceId},
                    {"TokensBalance", _moneyFormatter.FormatAmountToDisplayString(amount)}
                }
            });
        }

        public Task SendPaymentTransferRejectedAsync(string customerId, string invoiceId)
        {
            return PublishAsync(new EmailMessageEvent
            {
                CustomerId = customerId,
                MessageTemplateId = _emailNotificationsSettingsService.PaymentTransferRejectedTemplateSettings.MessageTemplateId,
                SubjectTemplateId = _emailNotificationsSettingsService.PaymentTransferRejectedTemplateSettings.SubjectTemplateId,
                Source = $"{AppEnvironment.Name} - {AppEnvironment.Version}",
                TemplateParameters = new Dictionary<string, string>
                {
                    {"InvoiceId", invoiceId},
                    {"SupportPhoneNumber", _customerSupportNumber}
                }
            });
        }
    }
}
