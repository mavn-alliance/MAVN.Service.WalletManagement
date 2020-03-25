using System.Threading.Tasks;
using Lykke.Service.WalletManagement.Domain.Models;

namespace Lykke.Service.WalletManagement.Domain
{
    public interface ITransfersRepository
    {
        Task AddAsync(ITransfer model);

        Task<ITransfer> GetAsync(string id);

        Task DeleteAsync(string id);
    }
}
