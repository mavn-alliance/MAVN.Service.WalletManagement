using System.Threading.Tasks;
using MAVN.Service.WalletManagement.Domain.Models;

namespace MAVN.Service.WalletManagement.Domain.Services
{
    public interface IPartnersPaymentsService
    {
        Task HandlePartnerPaymentCreatedAsync(string customerId, string paymentRequestId);

        Task HandlePartnersPaymentProcessedAsync(PartnerPaymentDto partnerPayment);
    }
}
