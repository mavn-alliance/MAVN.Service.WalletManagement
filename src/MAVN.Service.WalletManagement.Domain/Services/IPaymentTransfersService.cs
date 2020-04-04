using System.Threading.Tasks;
using MAVN.Service.WalletManagement.Domain.Models;

namespace MAVN.Service.WalletManagement.Domain.Services
{
    public interface IPaymentTransfersService
    {
        Task HandlePaymentTransferProcessed(IPaymentTransfer paymentTransfer);
    }
}
