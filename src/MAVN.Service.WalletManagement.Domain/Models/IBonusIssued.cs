using System;
using Falcon.Numerics;

namespace MAVN.Service.WalletManagement.Domain.Models
{
    public interface IBonusIssued
    {
        Guid OperationId { get; }

        string PartnerId { get; }

        string LocationId { get; }

        string UnitLocationCode { get; }

        Guid CampaignId { get; }

        Guid ConditionId { get; }

        string CustomerId { get; }

        string BonusType { get; }

        Money18 Amount { get; }

        DateTime TimeStamp { get; }

        string ReferralId { get; }
    }
}
