using System;
using System.Threading.Tasks;
using MAVN.Numerics;

namespace MAVN.Service.WalletManagement.Domain.Publishers
{
    public interface IEmailNotificationsPublisher
    {
        Task SendP2PSucceededForSenderAsync(string customerId,
            string transactionId,
            Money18 amount,
            DateTime timestamp,
            Money18 currentBalance,
            string receiverEmail);

        Task SendP2PSucceededForReceiverAsync(string customerId,
            string transactionId,
            Money18 amount,
            DateTime timestamp,
            Money18 currentBalance,
            string senderEmail);

        Task SendP2PFailedForSenderAsync(string customerId,
            string transactionId,
            Money18 amount,
            DateTime timestamp,
            Money18 currentBalance,
            string receiverEmail);
    }
}
