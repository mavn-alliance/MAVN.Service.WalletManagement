using System.Threading.Tasks;
using MAVN.Service.WalletManagement.Domain.Models;

namespace MAVN.Service.WalletManagement.Domain
{
    public interface ITransfersRepository
    {
        Task AddAsync(ITransfer model);

        Task<ITransfer> GetAsync(string id);

        Task DeleteAsync(string id);
    }
}
