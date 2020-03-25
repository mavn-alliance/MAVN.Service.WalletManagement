using System;
using Falcon.Numerics;

namespace Lykke.Service.WalletManagement.Contract.Events
{
    /// <summary>
    /// Happens when Transfer between two Customers is completed and provides
    /// additional information about the Transfer.
    /// </summary>
    public class P2PTransferEvent
    {
        /// <summary>
        /// The id of the Sender
        /// </summary>
        public string SenderCustomerId { get; set; }
        /// <summary>
        /// The id of the Receiver
        /// </summary>
        public string ReceiverCustomerId { get; set; }
        /// <summary>
        /// The Symbol that represents the Asset that is being transferred
        /// </summary>
        public string AssetSymbol { get; set; }
        /// <summary>
        /// The Amount that is being transferred
        /// </summary>
        public Money18 Amount { get; set; }
        /// <summary>
        /// Id of the operation
        /// </summary>
        public string TransactionId { get; set; }
        /// <summary>
        /// External id of the operation
        /// </summary>
        public string ExternalOperationId { get; set; }
        /// <summary>
        /// Timestamp of when the Send was initialized
        /// </summary>
        public DateTime Timestamp { get; set; }

    }
}
