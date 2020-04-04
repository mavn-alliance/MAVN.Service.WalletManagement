using System;
using Falcon.Numerics;
using MAVN.Service.WalletManagement.Domain.Enums;

namespace MAVN.Service.WalletManagement.Domain.Models
{
    public class PartnerPaymentDto
    {
        public string PaymentRequestId { get; set; }

        public string CustomerId { get; set; }

        public string PartnerId { get; set; }

        public string LocationId { get; set; }

        public Money18 Amount { get; set; }

        public DateTime Timestamp { get; set; }

        public PartnerPaymentStatus Status { get; set; }
    }
}
