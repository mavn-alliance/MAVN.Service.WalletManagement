using System.Threading.Tasks;
using Common.Log;
using Lykke.Common.Log;
using Lykke.RabbitMqBroker.Subscriber;
using Lykke.Service.PartnersPayments.Contract;
using MAVN.Service.WalletManagement.Domain.Services;

namespace MAVN.Service.WalletManagement.DomainServices.Subscribers
{
    public class PartnerPaymentRequestCreatedSubscriber : JsonRabbitSubscriber<PartnerPaymentRequestCreatedEvent>
    {
        private readonly IPartnersPaymentsService _partnersPaymentsService;
        private readonly ILog _log;

        public PartnerPaymentRequestCreatedSubscriber(
            string connectionString, 
            string exchangeName, 
            string queueName, 
            ILogFactory logFactory, 
            IPartnersPaymentsService partnersPaymentsService) : base(connectionString, exchangeName, queueName, logFactory)
        {
            _partnersPaymentsService = partnersPaymentsService;
            _log = logFactory.CreateLog(this);
        }
        
        protected override async Task ProcessMessageAsync(PartnerPaymentRequestCreatedEvent message)
        {
            await _partnersPaymentsService.HandlePartnerPaymentCreatedAsync(
                message.CustomerId,
                message.PaymentRequestId);
            
            _log.Info("Processed PartnerPaymentRequestCreatedEvent", message);
        }
    }
}
