using Common.Log;
using Lykke.Common.Log;
using Lykke.RabbitMqBroker.Subscriber;
using MAVN.Service.PartnersPayments.Contract;
using MAVN.Service.WalletManagement.Domain.Models;
using MAVN.Service.WalletManagement.Domain.Services;
using System.Threading.Tasks;

namespace MAVN.Service.WalletManagement.DomainServices.Subscribers
{
    public class PartnersPaymentProcessedSubscriber : JsonRabbitSubscriber<PartnersPaymentProcessedEvent>
    {
        private readonly IPartnersPaymentsService _partnersPaymentsService;
        private readonly ILog _log;

        public PartnersPaymentProcessedSubscriber(
            string connectionString,
            string exchangeName,
            string queueName,
            ILogFactory logFactory,
            IPartnersPaymentsService partnersPaymentsService) : base(connectionString, exchangeName, queueName, logFactory)
        {
            _partnersPaymentsService = partnersPaymentsService;
            _log = logFactory.CreateLog(this);
        }

        protected override async Task ProcessMessageAsync(PartnersPaymentProcessedEvent message)
        {
            await _partnersPaymentsService.HandlePartnersPaymentProcessedAsync(new PartnerPaymentDto
            {
                CustomerId = message.CustomerId,
                Amount = message.Amount,
                Timestamp = message.Timestamp,
                PartnerId = message.PartnerId,
                LocationId = message.LocationId,
                PaymentRequestId = message.PaymentRequestId,
                Status = (Domain.Enums.PartnerPaymentStatus) message.Status
            });

            _log.Info("Processed PartnersPaymentProcessedEvent", message);
        }
    }
}
