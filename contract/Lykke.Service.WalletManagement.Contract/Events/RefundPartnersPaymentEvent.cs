using System;
using Falcon.Numerics;

namespace Lykke.Service.WalletManagement.Contract.Events
{
    public class RefundPartnersPaymentEvent
    {
        /// <summary>
        /// Id of the request
        /// </summary>
        public string PaymentRequestId { get; set; }

        /// <summary>
        /// Id of the customer
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// Id of the partner
        /// </summary>
        public string PartnerId { get; set; }

        /// <summary>
        /// Id of the location
        /// </summary>
        public string LocationId { get; set; }

        /// <summary>
        /// Amount paid
        /// </summary>
        public Money18 Amount { get; set; }

        /// <summary>
        /// Timestamp
        /// </summary>
        public DateTime Timestamp { get; set; }
    }
}
