using System.ComponentModel.DataAnnotations;
using MAVN.Numerics;

namespace MAVN.Service.WalletManagement.Client.Models.Requests
{
    /// <summary>
    /// Transfer request details
    /// </summary>
    public class TransferBalanceRequestModel
    {
        /// <summary>
        /// External id of the operation
        /// </summary>
        public string OperationId { get; set; }

        /// <summary>
        /// The id of the Sender
        /// </summary>
        [Required]
        public string SenderCustomerId { get; set; }

        /// <summary>
        /// The id of the Receiver
        /// </summary>
        [Required]
        public string ReceiverCustomerId { get; set; }

        /// <summary>
        /// The Amount that is being transferred
        /// </summary>
        [Required]
        public Money18 Amount { get; set; }
    }
}
