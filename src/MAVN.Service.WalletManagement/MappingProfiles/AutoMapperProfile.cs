using AutoMapper;
using MAVN.Service.WalletManagement.Client.Models.Responses;
using MAVN.Service.WalletManagement.Domain.Models;

namespace MAVN.Service.WalletManagement.MappingProfiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<TransferResultModel, TransferBalanceResponse>();
        }
    }
}
