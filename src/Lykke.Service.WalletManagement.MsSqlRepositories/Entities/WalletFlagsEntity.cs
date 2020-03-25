using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Lykke.Service.WalletManagement.Domain.Models;

namespace Lykke.Service.WalletManagement.MsSqlRepositories.Entities
{
    [Table("wallet_flags")]
    public class WalletFlagsEntity : IWalletFlags
    {
        [Key]
        [Required]
        [Column("customer_id")]
        public string CustomerId { get; set; }
        
        [Required]
        [Column("is_blocked")]
        public bool IsBlocked { get; set; }
        
        internal static WalletFlagsEntity Create(string customerId, bool isBlocked)
        {
            return new WalletFlagsEntity
            {
                CustomerId = customerId,
                IsBlocked = isBlocked
            };
        }
    }
}