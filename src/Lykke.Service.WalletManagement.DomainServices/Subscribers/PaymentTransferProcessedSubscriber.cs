using System.Threading.Tasks;
using Common.Log;
using Lykke.Common.Log;
using Lykke.RabbitMqBroker.Subscriber;
using Lykke.Service.PaymentTransfers.Contract;
using Lykke.Service.WalletManagement.Domain.Enums;
using Lykke.Service.WalletManagement.Domain.Models;
using Lykke.Service.WalletManagement.Domain.Services;

namespace Lykke.Service.WalletManagement.DomainServices.Subscribers
{
    public class PaymentTransferProcessedSubscriber : JsonRabbitSubscriber<PaymentTransferProcessedEvent>
    {
        private readonly IPaymentTransfersService _paymentTransfersService;
        private readonly ILog _log;
        public PaymentTransferProcessedSubscriber(
            IPaymentTransfersService paymentTransfersService,
            string connectionString,
            string exchangeName,
            string queueName,
            ILogFactory logFactory) 
            : base(connectionString, exchangeName, queueName, logFactory)
        {
            _paymentTransfersService = paymentTransfersService;
            _log = logFactory.CreateLog(this);
        }

        protected override async Task ProcessMessageAsync(PaymentTransferProcessedEvent message)
        {
            await _paymentTransfersService.HandlePaymentTransferProcessed(new PaymentTransferDto
            {
                CustomerId = message.CustomerId,
                Amount = message.Amount,
                CampaignId = message.CampaignId,
                TransferId = message.TransferId,
                Timestamp = message.Timestamp,
                InvoiceId = message.InvoiceId,
                Status = (PaymentTransferStatus) message.Status,
                InstalmentName = message.InstalmentName,
                LocationCode = message.LocationCode,
            });

            _log.Info("Processed PaymentTransferProcessedEvent", message);
        }
    }
}
