using System;
using System.Threading.Tasks;
using Lykke.RabbitMqBroker.Publisher;
using Lykke.Service.CustomerProfile.Client;
using Lykke.Service.CustomerProfile.Client.Models.Enums;
using Lykke.Service.WalletManagement.Contract.Events;
using Lykke.Service.WalletManagement.Domain.Enums;
using Lykke.Service.WalletManagement.Domain.Repositories;
using Lykke.Service.WalletManagement.Domain.Services;

namespace Lykke.Service.WalletManagement.DomainServices
{
    public class WalletManagementService : IWalletManagementService
    {
        private readonly ICustomerProfileClient _customerProfileClient;
        private readonly IWalletFlagsRepository _walletFlagsRepository;
        private readonly IRabbitPublisher<CustomerWalletStatusUpdatedEvent> _walletStatusUpdatePublisher;

        public WalletManagementService(
            ICustomerProfileClient customerProfileClient,
            IWalletFlagsRepository walletFlagsRepository,
            IRabbitPublisher<CustomerWalletStatusUpdatedEvent> walletStatusUpdatePublisher)
        {
            _customerProfileClient = customerProfileClient;
            _walletFlagsRepository = walletFlagsRepository;
            _walletStatusUpdatePublisher = walletStatusUpdatePublisher;
        }
        
        public async Task<CustomerWalletBlockErrorCode> BlockCustomerWalletAsync(string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
                throw new ArgumentNullException(nameof(customerId));
            
            var customerResponse = await _customerProfileClient.CustomerProfiles.GetByCustomerIdAsync(customerId, true);

            if (customerResponse.ErrorCode == CustomerProfileErrorCodes.CustomerProfileDoesNotExist)
            {
                return CustomerWalletBlockErrorCode.CustomerNotFound;
            }

            var repositoryEntry = await _walletFlagsRepository.GetByCustomerIdAsync(customerId);

            if (repositoryEntry != null && repositoryEntry.IsBlocked)
            {
                return CustomerWalletBlockErrorCode.CustomerWalletAlreadyBlocked;
            }

            await _walletFlagsRepository.CreateOrUpdateAsync(customerId, true);

            await _walletStatusUpdatePublisher.PublishAsync(new CustomerWalletStatusUpdatedEvent
            {
                CustomerId = customerId,
                WalletBlocked = true,
            });

            return CustomerWalletBlockErrorCode.None;
        }

        public async Task<CustomerWalletUnblockErrorCode> UnblockCustomerWalletAsync(string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
                throw new ArgumentNullException(nameof(customerId));
            
            var customerResponse = await _customerProfileClient.CustomerProfiles.GetByCustomerIdAsync(customerId, true);

            if (customerResponse.ErrorCode == CustomerProfileErrorCodes.CustomerProfileDoesNotExist)
            {
                return CustomerWalletUnblockErrorCode.CustomerNotFound;
            }

            var repositoryEntry = await _walletFlagsRepository.GetByCustomerIdAsync(customerId);

            if (repositoryEntry == null || !repositoryEntry.IsBlocked)
            {
                return CustomerWalletUnblockErrorCode.CustomerNotBlocked;
            }

            await _walletFlagsRepository.CreateOrUpdateAsync(customerId, false);

            await _walletStatusUpdatePublisher.PublishAsync(new CustomerWalletStatusUpdatedEvent
            {
                CustomerId = customerId,
                WalletBlocked = false,
            });

            return CustomerWalletUnblockErrorCode.None;
        }

        public async Task<bool?> IsCustomerWalletBlockedAsync(string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
                throw new ArgumentNullException(nameof(customerId));
            
            var customerResponse = await _customerProfileClient.CustomerProfiles.GetByCustomerIdAsync(customerId, true);

            if (customerResponse.ErrorCode == CustomerProfileErrorCodes.CustomerProfileDoesNotExist)
            {
                return null;
            }
            
            var repositoryEntry = await _walletFlagsRepository.GetByCustomerIdAsync(customerId);

            return repositoryEntry != null && repositoryEntry.IsBlocked;
        }
    }
}
