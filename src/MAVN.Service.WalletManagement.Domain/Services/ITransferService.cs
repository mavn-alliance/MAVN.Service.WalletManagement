using System;
using System.Threading.Tasks;
using MAVN.Numerics;
using MAVN.Service.WalletManagement.Domain.Models;

namespace MAVN.Service.WalletManagement.Domain.Services
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
