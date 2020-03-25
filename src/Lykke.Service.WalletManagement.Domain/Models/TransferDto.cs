using System;
using Falcon.Numerics;

namespace Lykke.Service.WalletManagement.Domain.Models
{
    public class TransferDto : ITransfer
    {
        public string Id { get; set; }
        public string OperationId { get; set; }
        public string ExternalOperationId { get; set; }
        public Money18 Amount { get; set; }
        public string AssetSymbol { get; set; }
        public string SenderCustomerId { get; set; }
        public string RecipientCustomerId { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
