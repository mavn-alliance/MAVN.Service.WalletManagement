using System;
using Falcon.Numerics;

namespace MAVN.Service.WalletManagement.Domain.Models
{
    public interface ITransfer
    {
        string Id { get; set; }
        
        string OperationId { get; set; }
        
        string ExternalOperationId { get; set; }
        
        Money18 Amount { get; set; }
        
        string AssetSymbol { get; set; }
        
        string SenderCustomerId { get; set; }
        
        string RecipientCustomerId { get; set; }
        
        DateTime TimeStamp { get; set; }
    }
}
