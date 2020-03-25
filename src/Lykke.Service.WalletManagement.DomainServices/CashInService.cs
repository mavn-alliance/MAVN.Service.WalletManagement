using System;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Lykke.Common.ApiLibrary.Exceptions;
using Lykke.Common.Log;
using Falcon.Numerics;
using Lykke.RabbitMqBroker.Publisher;
using Lykke.Service.Campaign.Client;
using Lykke.Service.Campaign.Client.Models.Enums;
using Lykke.Service.PrivateBlockchainFacade.Client;
using Lykke.Service.PrivateBlockchainFacade.Client.Models;
using Lykke.Service.WalletManagement.Contract.Events;
using Lykke.Service.WalletManagement.Domain.Models;
using Lykke.Service.WalletManagement.Domain.Publishers;
using Lykke.Service.WalletManagement.Domain.Repositories;
using Lykke.Service.WalletManagement.Domain.Services;

namespace Lykke.Service.WalletManagement.DomainServices
{
    public class CashInService : ICashInService
    {
        private readonly IBonusRewardsRepository _bonusRewardsRepository;
        private readonly IRabbitPublisher<BonusReceivedEvent> _bonusEventPublisher;
        private readonly IPrivateBlockchainFacadeClient _blockchainFacadeClient;
        private readonly IPushNotificationsPublisher _pushNotificationsPublisher;
        private readonly ICampaignClient _campaignClient;
        private readonly ILog _log;
        private readonly string _tokenSymbol;

        public CashInService(
            ILogFactory logFactory,
            IBonusRewardsRepository bonusRewardsRepository,
            IRabbitPublisher<BonusReceivedEvent> bonusEventPublisher,
            IPrivateBlockchainFacadeClient blockchainFacadeClient,
            IPushNotificationsPublisher pushNotificationsPublisher,
            ICampaignClient campaignClient,
            string tokenSymbol)
        {
            _bonusRewardsRepository = bonusRewardsRepository;
            _bonusEventPublisher = bonusEventPublisher ?? throw new ArgumentNullException(nameof(bonusEventPublisher));
            _blockchainFacadeClient = blockchainFacadeClient;
            _pushNotificationsPublisher = pushNotificationsPublisher;
            _campaignClient = campaignClient;
            _tokenSymbol = tokenSymbol ?? throw new ArgumentNullException(nameof(tokenSymbol));
            _log = logFactory.CreateLog(this);
        }

        public async Task ProcessBonusIssuedEventAsync(IBonusIssued bonusIssued)
        {
            if (string.IsNullOrEmpty(bonusIssued.CustomerId) || bonusIssued.OperationId == Guid.Empty)
            {
                _log.Warning("Bonus Issued event would not be processed because it contains invalid data",
                    context: bonusIssued);
                return;
            }

            BonusRewardResponseModel rewardRequestResult;

            var request = new BonusRewardRequestModel
            {
                CustomerId = Guid.Parse(bonusIssued.CustomerId),
                Amount = bonusIssued.Amount,
                RewardRequestId = bonusIssued.OperationId.ToString(),
                BonusReason = bonusIssued.BonusType,
                CampaignId = bonusIssued.CampaignId,
                ConditionId = bonusIssued.ConditionId
            };

            try
            {
                rewardRequestResult = await _blockchainFacadeClient.BonusesApi.RewardAsync(request);
            }
            catch (ClientApiException e)
            {
                _log.Error(e, "Couldn't make reward request to PBF",
                    new
                    {
                        e.HttpStatusCode,
                        e.ErrorResponse?.ErrorMessage,
                        ModelErrors = e.ErrorResponse?.ModelErrors?.ToJson(),
                        request = request.ToJson()
                    });

                throw;
            }

            if (rewardRequestResult.Error != BonusRewardError.None)
            {
                _log.Error(message: "Error when trying to create reward operation in BC",
                    context: bonusIssued);
            }

            await _bonusRewardsRepository.AddAsync(bonusIssued);
        }

        public async Task ProcessBonusRewardDetectedEventAsync(
            string customerId,
            Money18 amount,
            string requestId,
            string bonusReason,
            DateTime timespan)
        {
            var operationIdIsGuid = Guid.TryParse(requestId, out var guidRequestId);

            if (!operationIdIsGuid)
            {
                _log.Error(message: "Invalid operation Id", context: requestId);
            }

            var eventInfo = await _bonusRewardsRepository.GetAsync(guidRequestId);

            if (eventInfo == null)
            {
                const string msg = "BonusRewardDetectedEvent context was not found";

                _log.Error(message: msg, context: new {customerId, requestId, amount});

                throw new InvalidOperationException(msg);
            }

            await _bonusEventPublisher.PublishAsync(new BonusReceivedEvent
            {
                CustomerId = customerId,
                Amount = amount,
                AssetSymbol = _tokenSymbol,
                TransactionId = requestId,
                ExternalOperationId = eventInfo.OperationId.ToString(),
                PartnerId = eventInfo.PartnerId,
                LocationId = eventInfo.LocationId,
                LocationCode = eventInfo.UnitLocationCode,
                CampaignId = eventInfo.CampaignId,
                BonusType = eventInfo.BonusType,
                Timestamp = DateTime.UtcNow,
                ConditionId = eventInfo.ConditionId,
                ReferralId = eventInfo.ReferralId
            });

            await SendNotificationAsync(customerId, amount, eventInfo.CampaignId, eventInfo.ConditionId, requestId);

            await _bonusRewardsRepository.DeleteAsync(guidRequestId);
        }

        private async Task SendNotificationAsync(string customerId, Money18 amount, Guid campaignId, Guid conditionId,
            string requestId)
        {
            var campaign = await _campaignClient.History.GetEarnRuleByIdAsync(campaignId);

            if (campaign.ErrorCode != CampaignServiceErrorCodes.None)
            {
                _log.Error(message: "An error occurred while getting campaign",
                    context:
                    $"customerId: {customerId}; requestId: {requestId}; campaignId: {campaignId}; error: {campaign.ErrorCode}");
                return;
            }

            if (conditionId == Guid.Empty)
            {
                await _pushNotificationsPublisher.PublishCampaignCompletedAsync(customerId, amount, campaign.Name);
            }
            else
            {
                var condition = campaign.Conditions.FirstOrDefault(o => o.Id == conditionId.ToString());

                if (condition == null)
                {
                    _log.Error(message: "Campaign does not contains condition",
                        context:
                        $"customerId: {customerId}; requestId: {requestId}; campaignId: {campaignId}; conditionId: {conditionId}");
                    return;
                }

                await _pushNotificationsPublisher.PublishCampaignConditionCompletedAsync(customerId, amount,
                    campaign.Name, condition.TypeDisplayName);
            }
        }
    }
}
