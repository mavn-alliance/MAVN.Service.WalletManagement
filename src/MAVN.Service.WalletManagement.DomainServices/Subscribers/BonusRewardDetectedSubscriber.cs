using System.Threading.Tasks;
using Common.Log;
using Lykke.Common.Log;
using Lykke.RabbitMqBroker.Subscriber;
using Lykke.Service.PrivateBlockchainFacade.Contract.Events;
using MAVN.Service.WalletManagement.Domain.Services;

namespace MAVN.Service.WalletManagement.DomainServices.Subscribers
{
    public class BonusRewardDetectedSubscriber : JsonRabbitSubscriber<BonusRewardDetectedEvent>
    {
        private readonly ICashInService _cashInService;
        private readonly ILog _log;

        public BonusRewardDetectedSubscriber(string connectionString,
            string exchangeName,
            string queueName,
            ILogFactory logFactory,
            ICashInService cashInService)
            : base(connectionString, exchangeName, queueName, logFactory)
        {
            _cashInService = cashInService;
            _log = logFactory.CreateLog(this);
        }

        public BonusRewardDetectedSubscriber(string connectionString,
            string exchangeName,
            string queueName,
            bool isDurable,
            ILogFactory logFactory,
            ICashInService cashInService)
            : base(connectionString, exchangeName, queueName, isDurable, logFactory)
        {
            _cashInService = cashInService;
            _log = logFactory.CreateLog(this);
        }

        protected override async Task ProcessMessageAsync(BonusRewardDetectedEvent message)
        {
            await _cashInService.ProcessBonusRewardDetectedEventAsync
                (message.CustomerId, message.Amount, message.RequestId, message.BonusReason, message.Timestamp);

            _log.Info("Processed bonus reward detected event", message);
        }

    }
}
