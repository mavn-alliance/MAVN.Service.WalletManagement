using System.Threading.Tasks;
using Common.Log;
using Lykke.Common.Log;
using Lykke.RabbitMqBroker.Publisher;
using Lykke.Service.WalletManagement.Contract.Events;
using Lykke.Service.WalletManagement.Domain.Enums;
using Lykke.Service.WalletManagement.Domain.Models;
using Lykke.Service.WalletManagement.Domain.Publishers;
using Lykke.Service.WalletManagement.Domain.Services;

namespace Lykke.Service.WalletManagement.DomainServices
{
    public class PartnersPaymentsService : IPartnersPaymentsService
    {
        private readonly IPushNotificationsPublisher _pushNotificationsPublisher;
        private readonly IRabbitPublisher<RefundPartnersPaymentEvent> _refundPartnersPaymentPublisher;
        private readonly IRabbitPublisher<SuccessfulPartnersPaymentEvent> _successfulPartnersPaymentPublisher;
        private readonly ILog _log;

        public PartnersPaymentsService(
            IPushNotificationsPublisher pushNotificationsPublisher,
            IRabbitPublisher<RefundPartnersPaymentEvent> refundPartnersPaymentPublisher,
            IRabbitPublisher<SuccessfulPartnersPaymentEvent> successfulPartnersPaymentPublisher,
            ILogFactory logFactory)
        {
            _pushNotificationsPublisher = pushNotificationsPublisher;
            _refundPartnersPaymentPublisher = refundPartnersPaymentPublisher;
            _successfulPartnersPaymentPublisher = successfulPartnersPaymentPublisher;
            _log = logFactory.CreateLog(this);
        }

        public async Task HandlePartnerPaymentCreatedAsync(string customerId, string paymentRequestId)
        {
            if (string.IsNullOrEmpty(customerId))
            {
                _log.Warning("Got a message with an empty customerId", context: new { paymentRequestId });
                return;
            }

            if (string.IsNullOrEmpty(paymentRequestId))
            {
                _log.Warning("Got a message with an empty payment request id", context: new { customerId });
                return;
            }

            await _pushNotificationsPublisher.PublishPartnerPaymentCreatedAsync(customerId, paymentRequestId);
            _log.Info("Published notification for created PartnerPayment", context: new { customerId, paymentRequestId });
        }

        public async Task HandlePartnersPaymentProcessedAsync(PartnerPaymentDto partnerPayment)
        {
            if (partnerPayment.Status == PartnerPaymentStatus.Accepted)
            {
                await _successfulPartnersPaymentPublisher.PublishAsync(new SuccessfulPartnersPaymentEvent
                {
                    CustomerId = partnerPayment.CustomerId,
                    Amount = partnerPayment.Amount,
                    Timestamp = partnerPayment.Timestamp,
                    PartnerId = partnerPayment.PartnerId,
                    PaymentRequestId = partnerPayment.PaymentRequestId,
                    LocationId = partnerPayment.LocationId,
                });

                _log.Info("Published SuccessfulPartnersPaymentEvent", context: partnerPayment.PaymentRequestId);
            }

            if (partnerPayment.Status == PartnerPaymentStatus.Rejected)
            {
                await _refundPartnersPaymentPublisher.PublishAsync(new RefundPartnersPaymentEvent
                {
                    CustomerId = partnerPayment.CustomerId,
                    Amount = partnerPayment.Amount,
                    Timestamp = partnerPayment.Timestamp,
                    PartnerId = partnerPayment.PartnerId,
                    PaymentRequestId = partnerPayment.PaymentRequestId,
                    LocationId = partnerPayment.LocationId,
                });

                _log.Info("Published RefundPartnersPaymentEvent", context: partnerPayment.PaymentRequestId);
            }
        }
    }
}
