using System.Threading.Tasks;
using Common.Log;
using Lykke.Common.Log;
using Lykke.RabbitMqBroker.Subscriber;
using Lykke.Service.PrivateBlockchainFacade.Contract.Events;
using MAVN.Service.WalletManagement.Domain.Services;

namespace MAVN.Service.WalletManagement.DomainServices.Subscribers
{
    public class P2PTransferFailedSubscriber : JsonRabbitSubscriber<P2PTransferFailedEvent>
    {
        private readonly ITransferService _transferService;
        private readonly ILog _log;

        public P2PTransferFailedSubscriber(
            ITransferService transferService,
            string connectionString,
            string exchangeName,
            string queueName,
            ILogFactory logFactory)
            : base(connectionString, exchangeName, queueName, logFactory)
        {
            _transferService = transferService;
            _log = logFactory.CreateLog(this);
        }


        protected override async Task ProcessMessageAsync(P2PTransferFailedEvent message)
        {
            await _transferService.ProcessP2PTransferFailed(message.RequestId, message.TransactionHash,
                message.SenderCustomerId, message.ReceiverCustomerId, message.Amount, message.Timestamp);

            _log.Info("Processed P2P transfer failed event", message);
        }
    }
}
