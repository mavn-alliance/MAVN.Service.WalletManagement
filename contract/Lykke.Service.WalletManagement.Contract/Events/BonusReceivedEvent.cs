using System;
using Falcon.Numerics;
using JetBrains.Annotations;

namespace Lykke.Service.WalletManagement.Contract.Events
{
    /// <summary>
    /// Event for received bonus.
    /// </summary>
    [PublicAPI]
    public class BonusReceivedEvent
    {
        /// <summary>Transaction id</summary>
        public string TransactionId { get; set; }

        /// <summary>External id of the operation</summary>
        public string ExternalOperationId { get; set; }

        /// <summary>Customer id</summary>
        public string CustomerId { get; set; }

        /// <summary>Partner id</summary>
        public string PartnerId { get; set; }

        /// <summary>Location id</summary>
        public string LocationId { get; set; }

        /// <summary>Campaign id</summary>
        public Guid CampaignId { get; set; }

        /// <summary>Condition id</summary>
        public Guid ConditionId { get; set; }

        /// <summary>Asset symbol</summary>
        public string AssetSymbol { get; set; }

        /// <summary>Amount</summary>
        public Money18 Amount { get; set; }

        /// <summary>Bonus type</summary>
        public string BonusType { get; set; }

        /// <summary>The Location Code from sf</summary>
        public string LocationCode { get; set; }

        /// <summary>Timestamp</summary>
        public DateTime Timestamp { get; set; }

        /// <summary>Referral id</summary>
        public string ReferralId { get; set; }
    }
}
