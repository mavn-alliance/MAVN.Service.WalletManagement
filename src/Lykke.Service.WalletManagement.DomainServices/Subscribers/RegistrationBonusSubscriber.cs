using System;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Common;
using Lykke.Common.Log;
using Lykke.RabbitMqBroker;
using Lykke.RabbitMqBroker.Subscriber;
using Lykke.Service.BonusEngine.Contract.Events;
using Lykke.Service.WalletManagement.Domain.Models;
using Lykke.Service.WalletManagement.Domain.Services;

namespace Lykke.Service.WalletManagement.DomainServices.Subscribers
{
    public class RegistrationBonusSubscriber : IStartStop
    {
        private readonly string _connectionString;
        private readonly string _exchangeName;
        private readonly ICashInService _cashInService;
        private readonly ILogFactory _logFactory;
        private readonly ILog _log;

        private RabbitMqSubscriber<BonusIssuedEvent> _subscriber;

        public RegistrationBonusSubscriber(
            string connectionString,
            string exchangeName,
            ICashInService cashInService,
            ILogFactory logFactory)
        {
            _connectionString = connectionString;
            _exchangeName = exchangeName;
            _cashInService = cashInService;
            _logFactory = logFactory;
            _log = logFactory.CreateLog(this);
        }

        public void Start()
        {
            var settings = RabbitMqSubscriptionSettings.ForSubscriber(_connectionString,
                    _exchangeName,
                    "walletmanagement")
                .MakeDurable();

            _subscriber = new RabbitMqSubscriber<BonusIssuedEvent>(
                    _logFactory,
                    settings,
                    new ResilientErrorHandlingStrategy(_logFactory, settings, TimeSpan.FromSeconds(10)))
                .SetMessageDeserializer(new JsonMessageDeserializer<BonusIssuedEvent>())
                .Subscribe(ProcessMessageAsync)
                .CreateDefaultBinding()
                .Start();
        }

        public void Dispose()
        {
            _subscriber.Dispose();
        }

        public void Stop()
        {
            _subscriber.Stop();
        }

        private async Task ProcessMessageAsync(BonusIssuedEvent msg)
        {
            await _cashInService.ProcessBonusIssuedEventAsync(new BonusIssuedDto
            {
                CustomerId = msg.CustomerId,
                OperationId = msg.OperationId,
                Amount = msg.Amount,
                BonusType = msg.BonusOperationType.ToString(),
                CampaignId = msg.CampaignId,
                ConditionId = msg.ConditionId,
                UnitLocationCode = msg.UnitLocationCode,
                PartnerId = msg.PartnerId,
                LocationId = msg.LocationId,
                TimeStamp = msg.TimeStamp,
                ReferralId = msg.ReferralId
            });

            _log.Info($"Processed bonus event", msg);
        }
    }
}
