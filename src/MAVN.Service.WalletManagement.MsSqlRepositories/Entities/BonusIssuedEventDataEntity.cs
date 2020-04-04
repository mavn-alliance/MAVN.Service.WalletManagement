using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Falcon.Numerics;
using MAVN.Service.WalletManagement.Domain.Models;

namespace MAVN.Service.WalletManagement.MsSqlRepositories.Entities
{
    [Table("bonus_event_data")]
    public class BonusIssuedEventDataEntity : IBonusIssued
    {
        [Key]
        [Column("operation_id")]
        public Guid OperationId { get; set; }

        [Column("partner_id")]
        public string PartnerId { get; set; }

        [Column("location_id")]
        public string LocationId { get; set; }

        [Column("location_code")]
        public string UnitLocationCode { get; set; }

        [Column("campaign_id")]
        [Required]
        public Guid CampaignId { get; set; }

        [Column("condition_id")]
        public Guid ConditionId { get; set; }

        [Column("customer_id")]
        [Required]
        public string CustomerId { get; set; }

        [Column("bonus_type")]
        public string BonusType { get; set; }

        [Column("amount")]
        [Required]
        public Money18 Amount { get; set; }

        [Column("timestamp")]
        [Required]
        public DateTime TimeStamp { get; set; }

        [Column("referral_id")]
        public string ReferralId { get; set; }

        public static BonusIssuedEventDataEntity Create(IBonusIssued model)
        {
            return new BonusIssuedEventDataEntity
            {
                CustomerId = model.CustomerId,
                OperationId = model.OperationId,
                Amount = model.Amount,
                PartnerId = model.PartnerId,
                LocationId = model.LocationId,
                UnitLocationCode = model.UnitLocationCode,
                CampaignId = model.CampaignId,
                ConditionId = model.ConditionId,
                TimeStamp = model.TimeStamp,
                BonusType = model.BonusType,
                ReferralId = model.ReferralId
            };
        }
    }
}
