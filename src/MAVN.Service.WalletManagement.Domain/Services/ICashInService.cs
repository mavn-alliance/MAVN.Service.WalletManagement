using System;
using System.Threading.Tasks;
using MAVN.Numerics;
using MAVN.Service.WalletManagement.Domain.Models;

namespace MAVN.Service.WalletManagement.Domain.Services
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
