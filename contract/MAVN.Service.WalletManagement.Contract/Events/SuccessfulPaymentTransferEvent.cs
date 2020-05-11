using System;
using MAVN.Numerics;

namespace MAVN.Service.WalletManagement.Contract.Events
{
    public class SuccessfulPaymentTransferEvent
    {
        /// <summary>
        /// Id of the payment transfer
        /// </summary>
        public string TransferId { get; set; }

        /// <summary>
        /// Id of the customer
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// Id of the campaign
        /// </summary>
        public string CampaignId { get; set; }

        /// <summary>
        /// Id of the invoice
        /// </summary>
        public string InvoiceId { get; set; }

        /// <summary>
        /// Name of the Instalment
        /// </summary>
        public string InstalmentName { get; set; }

        /// <summary>
        /// Amount of tokens paid
        /// </summary> 
        public Money18 Amount { get; set; }

        /// <summary>
        /// Timestamp
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// The Location Code from sf
        /// </summary>
        public string LocationCode { get; set; }
    }
}
