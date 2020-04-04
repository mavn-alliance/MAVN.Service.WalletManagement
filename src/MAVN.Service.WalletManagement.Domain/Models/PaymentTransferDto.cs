using System;
using Falcon.Numerics;
using MAVN.Service.WalletManagement.Domain.Enums;

namespace MAVN.Service.WalletManagement.Domain.Models
{
    public class PaymentTransferDto : IPaymentTransfer
    {
        public string TransferId { get; set; }
        public string CustomerId { get; set; }
        public string CampaignId { get; set; }
        public string InvoiceId { get; set; }
        public string InstalmentName { get; set; }
        public Money18 Amount { get; set; }
        public PaymentTransferStatus Status { get; set; }
        public DateTime Timestamp { get; set; }
        public string LocationCode { get; set; }
    }
}
