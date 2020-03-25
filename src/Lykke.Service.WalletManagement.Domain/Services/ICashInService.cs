using System;
using System.Threading.Tasks;
using Falcon.Numerics;
using Lykke.Service.WalletManagement.Domain.Models;

namespace Lykke.Service.WalletManagement.Domain.Services
{
    public interface ICashInService
    {
        Task ProcessBonusIssuedEventAsync(IBonusIssued bonusIssued);

        Task ProcessBonusRewardDetectedEventAsync(
            string customerId,
            Money18 amount,
            string requestId,
            string bonusReason,
            DateTime timespan);
    }
}
