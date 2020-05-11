using System.Threading.Tasks;
using MAVN.Numerics;

namespace MAVN.Service.WalletManagement.Domain.Publishers
{
    public interface IPushNotificationsPublisher
    {
        Task PublishP2PSucceededForSenderAsync(string customerId);

        Task PublishP2PSucceededForReceiverAsync(string customerId, Money18 amount, string senderEmail);

        Task PublishP2PFailedForSenderAsync(string customerId);

        Task PublishCampaignCompletedAsync(string customerId, Money18 amount, string campaignName);

        Task PublishCampaignConditionCompletedAsync(string customerId, Money18 amount, string campaignName,
            string conditionName);
        Task PublishPartnerPaymentCreatedAsync(string customerId, string paymentRequestId);
    }
}
