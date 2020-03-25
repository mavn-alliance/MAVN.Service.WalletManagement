using Falcon.Numerics;

namespace Lykke.Service.WalletManagement.Domain.Services
{
    public interface IMoneyFormatter
    {
        string FormatAmountToDisplayString(Money18 money18);
    }
}
