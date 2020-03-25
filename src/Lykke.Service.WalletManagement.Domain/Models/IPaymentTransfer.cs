using System;
using Falcon.Numerics;
using Lykke.Service.WalletManagement.Domain.Enums;

namespace Lykke.Service.WalletManagement.Domain.Models
{
    public interface IPaymentTransfer
    {
        string TransferId { get; set; }
        string CustomerId { get; set; }
        string CampaignId { get; set; }
        string InvoiceId { get; set; }
        string InstalmentName { get; set; }
        Money18 Amount { get; set; }
        PaymentTransferStatus Status { get; set; }
        DateTime Timestamp { get; set; }
        string LocationCode { get; set; }
    }
}
