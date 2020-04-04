using Falcon.Numerics;

namespace MAVN.Service.WalletManagement.Domain.Services
{
    public interface IMoneyFormatter
    {
        string FormatAmountToDisplayString(Money18 money18);
    }
}
