using System;
using Lykke.Service.WalletManagement.Domain.Enums;

namespace Lykke.Service.WalletManagement.Domain.Models
{
    public class TransferResultModel
    {
        /// <summary>
        /// Transfer transaction Id
        /// </summary>
        public string TransactionId { get; set; }

        /// <summary>
        /// External id of the operation
        /// </summary>
        public string ExternalOperationId { get; set; }

        /// <summary>
        /// Error code
        /// </summary>
        public TransferErrorCodes ErrorCode { get; set; }
        
        /// <summary>
        /// Transfer date and time
        /// </summary>
        public DateTime Timestamp { get; set; }
    }
}
