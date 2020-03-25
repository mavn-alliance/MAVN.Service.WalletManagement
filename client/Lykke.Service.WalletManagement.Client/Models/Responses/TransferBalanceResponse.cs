using System;
using JetBrains.Annotations;
using Lykke.Service.WalletManagement.Client.Enums;

namespace Lykke.Service.WalletManagement.Client.Models.Responses
{
    /// <summary>
    /// Transfer Balance operation response
    /// </summary>
    [PublicAPI]
    public class TransferBalanceResponse
    {
        /// <summary>
        /// External id of the operation
        /// </summary>
        public string ExternalOperationId { get; set; }

        /// <summary>
        /// Transfer transaction Id
        /// </summary>
        public string TransactionId { get; set; }

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
