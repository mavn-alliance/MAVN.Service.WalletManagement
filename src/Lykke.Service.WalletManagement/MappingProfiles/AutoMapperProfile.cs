using AutoMapper;
using Lykke.Service.WalletManagement.Client.Models.Responses;
using Lykke.Service.WalletManagement.Domain.Models;

namespace Lykke.Service.WalletManagement.MappingProfiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<TransferResultModel, TransferBalanceResponse>();
        }
    }
}
