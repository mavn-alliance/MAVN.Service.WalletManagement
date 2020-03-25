using System.Threading.Tasks;
using Lykke.Service.WalletManagement.Domain.Models;

namespace Lykke.Service.WalletManagement.Domain.Services
{
    public interface IPaymentTransfersService
    {
        Task HandlePaymentTransferProcessed(IPaymentTransfer paymentTransfer);
    }
}
