using System;
using System.Threading.Tasks;
using Falcon.Numerics;
using Lykke.Service.WalletManagement.Domain.Models;

namespace Lykke.Service.WalletManagement.Domain.Services
{
    public interface ITransferService
    {
        Task<TransferResultModel> TransferBalanceAsync(
            string externalOperationId,
            string senderCustomerId,
            string receiverCustomerId,
            Money18 amount);

        Task ProcessP2PTransferDetectedEventAsync(
            string contextId,
            string transactionHash,
            string senderCustomerId, 
            string receiverCustomerId, 
            Money18 amount, 
            DateTime timestamp);

        Task ProcessP2PTransferFailed(
            string contextId,
            string transactionHash,
            string senderCustomerId,
            string receiverCustomerId,
            Money18 amount,
            DateTime timestamp);
    }
}
