using System;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Common.ApiLibrary.Exceptions;
using Lykke.Common.Log;
using Falcon.Numerics;
using Lykke.RabbitMqBroker.Publisher;
using Lykke.Service.CustomerProfile.Client;
using Lykke.Service.CustomerProfile.Client.Models.Enums;
using Lykke.Service.PrivateBlockchainFacade.Client;
using Lykke.Service.PrivateBlockchainFacade.Client.Models;
using MAVN.Service.WalletManagement.Contract.Events;
using MAVN.Service.WalletManagement.Domain;
using MAVN.Service.WalletManagement.Domain.Enums;
using MAVN.Service.WalletManagement.Domain.Models;
using MAVN.Service.WalletManagement.Domain.Publishers;
using MAVN.Service.WalletManagement.Domain.Services;

namespace MAVN.Service.WalletManagement.DomainServices
{
    public class TransferService : ITransferService
    {
        private readonly ICustomerProfileClient _customerProfileClient;
        private readonly IPrivateBlockchainFacadeClient _blockchainFacadeClient;
        private readonly ITransfersRepository _transfersRepository;
        private readonly IRabbitPublisher<P2PTransferEvent> _p2PTransferEventPublisher;
        private readonly IWalletManagementService _walletManagementService;
        private readonly string _tokenSymbol;
        private readonly IEmailNotificationsPublisher _emailsPublisher;
        private readonly IPushNotificationsPublisher _pushNotificationsPublisher;
        private readonly ILog _log;

        private const string ContextNotFoundMsg = "P2PTransferDetectedEvent context was not found";

        public TransferService(
            string tokenSymbol,
            ICustomerProfileClient customerProfileClient,
            IPrivateBlockchainFacadeClient blockchainFacadeClient,
            IWalletManagementService walletManagementService,
            ITransfersRepository transfersRepository,
            IRabbitPublisher<P2PTransferEvent> p2PTransferEventPublisher,
            IEmailNotificationsPublisher emailsPublisher,
            IPushNotificationsPublisher pushNotificationsPublisher,
            ILogFactory logFactory)
        {
            _customerProfileClient = customerProfileClient;
            _blockchainFacadeClient = blockchainFacadeClient;
            _walletManagementService = walletManagementService;
            _transfersRepository = transfersRepository;
            _p2PTransferEventPublisher = p2PTransferEventPublisher;
            _tokenSymbol = tokenSymbol;
            _emailsPublisher = emailsPublisher;
            _pushNotificationsPublisher = pushNotificationsPublisher;
            _log = logFactory.CreateLog(this);
        }
        public async Task<TransferResultModel> TransferBalanceAsync(
            string externalOperationId,
            string senderCustomerId,
            string receiverCustomerId,
            Money18 amount)
        {
            var isSenderIdValidGuid = Guid.TryParse(senderCustomerId, out var senderId);
            var isReceiverIdValidGuid = Guid.TryParse(receiverCustomerId, out var receiverId);

            if (!isSenderIdValidGuid)
                return new TransferResultModel { ErrorCode = TransferErrorCodes.InvalidSenderId };

            if (!isReceiverIdValidGuid)
                return new TransferResultModel { ErrorCode = TransferErrorCodes.InvalidRecipientId };

            if (await ValidateIfCustomerExists(senderCustomerId))
            {
                _log.Warning($"Attempt for Transfer from Sender with ${senderCustomerId} failed as such Customer does not exist");
                return new TransferResultModel { ErrorCode = TransferErrorCodes.SourceCustomerNotFound };
            }

            if (await ValidateIfCustomerExists(receiverCustomerId))
            {
                _log.Warning($"Attempt for Transfer from Sender with ${senderCustomerId} failed as Receiver Customer with ${receiverCustomerId} does not exist");
                return new TransferResultModel { ErrorCode = TransferErrorCodes.TargetCustomerNotFound };
            }

            if (senderCustomerId == receiverCustomerId)
                return new TransferResultModel { ErrorCode = TransferErrorCodes.TransferSourceAndTargetMustBeDifferent };

            if (amount <= 0)
                return new TransferResultModel { ErrorCode = TransferErrorCodes.InvalidAmount };

            if (await ValidateIfCustomerWalletIsBlocked(senderCustomerId))
            {
                _log.Warning($"Attempt for Transfer from Sender with ${senderCustomerId} failed as such Customer Wallet is blocked");
                return new TransferResultModel { ErrorCode = TransferErrorCodes.SourceCustomerWalletBlocked };
            }

            if (await ValidateIfCustomerWalletIsBlocked(receiverCustomerId))
            {
                _log.Warning($"Attempt for Transfer from Sender with ${senderCustomerId} failed as Receiver Customer Wallet is blocked");
                return new TransferResultModel { ErrorCode = TransferErrorCodes.TargetCustomerWalletBlocked };
            }

            var transactionId = Guid.NewGuid().ToString();
            var contextId = externalOperationId ?? transactionId;

            var pbfTransferResponse = await _blockchainFacadeClient.TransfersApi.TransferAsync(
                new TransferRequestModel
                {
                    TransferId = contextId,
                    SenderCustomerId = senderId,
                    RecipientCustomerId = receiverId,
                    Amount = amount
                });

            if (pbfTransferResponse.Error != TransferError.None)
            {
                return new TransferResultModel { ErrorCode = (TransferErrorCodes)pbfTransferResponse.Error };
            }

            var now = DateTime.UtcNow;
            // saving context
            await _transfersRepository.AddAsync(new TransferDto
            {
                Id = contextId,
                OperationId = transactionId,
                ExternalOperationId = externalOperationId,
                SenderCustomerId = senderCustomerId,
                RecipientCustomerId = receiverCustomerId,
                Amount = amount,
                AssetSymbol = _tokenSymbol,
                TimeStamp = now
            });

            return new TransferResultModel
            {
                ExternalOperationId = externalOperationId,
                TransactionId = transactionId,
                Timestamp = now
            };
        }

        public async Task ProcessP2PTransferDetectedEventAsync(
            string contextId,
            string transactionHash,
            string senderCustomerId,
            string receiverCustomerId,
            Money18 amount,
            DateTime timestamp)
        {
            var transferContext = await _transfersRepository.GetAsync(contextId);

            if (transferContext == null)
            {
                _log.Error(message: ContextNotFoundMsg,
                    context: new { requestId = contextId, transactionHash, senderCustomerId, receiverCustomerId });
                throw new InvalidOperationException(ContextNotFoundMsg);
            }

            await _p2PTransferEventPublisher.PublishAsync(new P2PTransferEvent
            {
                Amount = amount,
                Timestamp = timestamp,
                SenderCustomerId = senderCustomerId,
                ReceiverCustomerId = receiverCustomerId,
                AssetSymbol = transferContext.AssetSymbol,
                TransactionId = transferContext.OperationId,
                ExternalOperationId = transferContext.ExternalOperationId
            });

            var senderCurrentBalance = await GetCustomerCurrentBalance(senderCustomerId);

            var receiverCurrentBalance = await GetCustomerCurrentBalance(receiverCustomerId);

            var senderCustomerEmail = await GetCustomerEmail(senderCustomerId);

            var receiverCustomerEmail = await GetCustomerEmail(receiverCustomerId);

            var senderEmailNotificationTask = _emailsPublisher.SendP2PSucceededForSenderAsync(senderCustomerId,
                contextId, amount, timestamp, senderCurrentBalance,
                receiverCustomerEmail);

            var receiverEmailNotificationTask = _emailsPublisher.SendP2PSucceededForReceiverAsync(receiverCustomerId,
                contextId, amount, timestamp, receiverCurrentBalance,
                senderCustomerEmail);

            var senderPushNotificationTask =
                _pushNotificationsPublisher.PublishP2PSucceededForSenderAsync(senderCustomerId);

            var receiverPushNotificationTask =
                _pushNotificationsPublisher.PublishP2PSucceededForReceiverAsync(receiverCustomerId, amount,
                    senderCustomerEmail);

            await Task.WhenAll(
                senderEmailNotificationTask,
                receiverEmailNotificationTask,
                senderPushNotificationTask,
                receiverPushNotificationTask);

            await _transfersRepository.DeleteAsync(contextId);
        }

        public async Task ProcessP2PTransferFailed(
            string contextId,
            string transactionHash,
            string senderCustomerId,
            string receiverCustomerId,
            Money18 amount,
            DateTime timestamp)
        {
            var transferContext = await _transfersRepository.GetAsync(contextId);

            if (transferContext == null)
            {
                _log.Error(message: "Context not found for p2p failed transfer",
                    context: new { requestId = contextId, transactionHash, senderCustomerId, receiverCustomerId });
                return;
            }

            var senderCurrentBalance = await GetCustomerCurrentBalance(senderCustomerId);

            var receiverEmail = await GetCustomerEmail(receiverCustomerId);

            var emailNotificationTask = _emailsPublisher.SendP2PFailedForSenderAsync(
                senderCustomerId,
                contextId,
                amount,
                timestamp,
                senderCurrentBalance,
                receiverEmail);

            var pushNotificationTask = _pushNotificationsPublisher.PublishP2PFailedForSenderAsync(senderCustomerId);

            await Task.WhenAll(emailNotificationTask, pushNotificationTask);

            await _transfersRepository.DeleteAsync(contextId);

        }

        private async Task<string> GetCustomerEmail(string customerId)
        {
            string customerEmail;
            try
            {
                var customerProfileResponse = await _customerProfileClient.CustomerProfiles.GetByCustomerIdAsync(customerId);

                if (customerProfileResponse.ErrorCode == CustomerProfileErrorCodes.None)
                    return customerProfileResponse.Profile.Email;

                _log.Error(
                    message:
                    "Cannot process P2P failed transfer notifications cause a customer does not have a profile",
                    context: customerId);
                customerEmail = "Unknown";

            }
            catch (ClientApiException e)
            {
                _log.Error(e, "Cannot get customer profile when processing p2p failed event", customerId);
                customerEmail = "Unknown";
            }

            return customerEmail;
        }

        private async Task<Money18> GetCustomerCurrentBalance(string customerId)
        {
            Money18 amount = 0;

            try
            {
                var currentBalanceResponse =
                    await _blockchainFacadeClient.CustomersApi.GetBalanceAsync(Guid.Parse(customerId));

                if (currentBalanceResponse.Error == CustomerBalanceError.None)
                    amount = currentBalanceResponse.Total;

                else
                {
                    _log.Error(
                        message:
                        "Cannot process P2P failed transfer notifications cause a customer does not have a wallet",
                        context: customerId);
                }
            }
            catch (ClientApiException e)
            {
                _log.Error(e, "Cannot get customer balance when processing p2p failed event", customerId);
            }

            return amount;
        }

        private async Task<bool> ValidateIfCustomerExists(string customerId)
        {
            var customer = await _customerProfileClient.CustomerProfiles.GetByCustomerIdAsync(customerId);

            return customer?.Profile == null;
        }

        private async Task<bool> ValidateIfCustomerWalletIsBlocked(string customerId)
        {
            var isBlocked = await _walletManagementService.IsCustomerWalletBlockedAsync(customerId);

            return isBlocked.Value;
        }
    }
}
