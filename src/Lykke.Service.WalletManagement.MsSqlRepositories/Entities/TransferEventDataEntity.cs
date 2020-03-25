using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Falcon.Numerics;
using Lykke.Service.WalletManagement.Domain.Models;

namespace Lykke.Service.WalletManagement.MsSqlRepositories.Entities
{
    [Table("transfer_event_data")]
    public class TransferEventDataEntity : ITransfer
    {
        [Key]
        [Column("id")]
        public string Id { get; set; }
        
        [Column("operation_id")]
        [Required]
        public string OperationId { get; set; }
        
        [Column("external_operation_id")]
        public string ExternalOperationId { get; set; }
        
        [Column("amount")]
        public Money18 Amount { get; set; }
        
        [Column("asset_symbol")]
        [Required]
        public string AssetSymbol { get; set; }
        
        [Column("sender_customer_id")]
        [Required]
        public string SenderCustomerId { get; set; }
        
        [Column("recipient_customer_id")]
        [Required]
        public string RecipientCustomerId { get; set; }
        
        [Column("timestamp")]
        [Required]
        public DateTime TimeStamp { get; set; }

        public static TransferEventDataEntity Create(ITransfer src)
        {
            return new TransferEventDataEntity
            {
                Id = src.Id,
                OperationId = src.OperationId,
                ExternalOperationId = src.ExternalOperationId,
                SenderCustomerId = src.SenderCustomerId,
                RecipientCustomerId = src.RecipientCustomerId,
                Amount = src.Amount,
                AssetSymbol = src.AssetSymbol,
                TimeStamp = src.TimeStamp
            };
        }
    }
}
