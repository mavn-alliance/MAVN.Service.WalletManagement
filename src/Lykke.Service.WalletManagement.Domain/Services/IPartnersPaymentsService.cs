using System.Threading.Tasks;
using Lykke.Service.WalletManagement.Domain.Models;

namespace Lykke.Service.WalletManagement.Domain.Services
{
    public interface IPartnersPaymentsService
    {
        Task HandlePartnerPaymentCreatedAsync(string customerId, string paymentRequestId);

        Task HandlePartnersPaymentProcessedAsync(PartnerPaymentDto partnerPayment);
    }
}
