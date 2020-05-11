using System.Threading.Tasks;
using Common.Log;
using Lykke.Common.Log;
using Lykke.RabbitMqBroker.Subscriber;
using MAVN.Service.PrivateBlockchainFacade.Contract.Events;
using MAVN.Service.WalletManagement.Domain.Services;

namespace MAVN.Service.WalletManagement.DomainServices.Subscribers
{
    public class P2PTransferDetectedSubscriber : JsonRabbitSubscriber<P2PTransferDetectedEvent>
    {
        private readonly ILog _log;
        private readonly ITransferService _transferService;
        
        public P2PTransferDetectedSubscriber(
            string connectionString, 
            string exchangeName, 
            string queueName, 
            ILogFactory logFactory, 
            ITransferService transferService) : base(connectionString, exchangeName, queueName, logFactory)
        {
            _transferService = transferService;
            _log = logFactory.CreateLog(this);
        }

        public P2PTransferDetectedSubscriber(
            string connectionString, 
            string exchangeName, 
            string queueName, 
            bool isDurable, 
            ILogFactory logFactory, 
            ITransferService transferService) : base(connectionString, exchangeName, queueName, isDurable, logFactory)
        {
            _transferService = transferService;
            _log = logFactory.CreateLog(this);
        }

        protected override async Task ProcessMessageAsync(P2PTransferDetectedEvent message)
        {
            await _transferService.ProcessP2PTransferDetectedEventAsync(
                message.RequestId,
                message.TransactionHash,
                message.SenderCustomerId, 
                message.ReceiverCustomerId,
                message.Amount, 
                message.Timestamp);
            
            _log.Info("Processed p2p transfer detected event", message);
        }
    }
}
