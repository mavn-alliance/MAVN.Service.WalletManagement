using System;
using Falcon.Numerics;

namespace MAVN.Service.WalletManagement.Domain.Models
{
    public class BonusIssuedDto : IBonusIssued
    {
        public Guid OperationId { get; set; }

        public string PartnerId { get; set; }

        public string LocationId { get; set; }

        public string UnitLocationCode { get; set; }

        public Guid CampaignId { get; set; }

        public Guid ConditionId { get; set; }

        public string CustomerId { get; set; }

        public string BonusType { get; set; }

        public Money18 Amount { get; set; }

        public DateTime TimeStamp { get; set; }

        public string ReferralId { get; set; }
    }
}
