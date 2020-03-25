using System;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Common.Log;
using Lykke.RabbitMqBroker.Publisher;
using Lykke.Service.PrivateBlockchainFacade.Client;
using Lykke.Service.WalletManagement.Contract.Events;
using Lykke.Service.WalletManagement.Domain.Enums;
using Lykke.Service.WalletManagement.Domain.Models;
using Lykke.Service.WalletManagement.Domain.Publishers;
using Lykke.Service.WalletManagement.Domain.Services;

namespace Lykke.Service.WalletManagement.DomainServices
{
    public class PaymentTransfersService : IPaymentTransfersService
    {
        private readonly ILog _log;
        private readonly IPrivateBlockchainFacadeClient _pbfClient;
        private readonly IRabbitPublisher<SuccessfulPaymentTransferEvent> _successfulPaymentTransferPublisher;
        private readonly IRabbitPublisher<RefundPaymentTransferEvent> _refundPaymentTransferPublisher;
        private readonly IEmailNotificationsPublisher _emailsPublisher;
        private readonly IPushNotificationsPublisher _pushNotificationsPublisher;

        public PaymentTransfersService(
            IPrivateBlockchainFacadeClient pbfClient,
            IRabbitPublisher<SuccessfulPaymentTransferEvent> successfulPaymentTransferPublisher,
            IRabbitPublisher<RefundPaymentTransferEvent> refundPaymentTransferPublisher,
            ILogFactory logFactory, 
            IEmailNotificationsPublisher emailsPublisher, 
            IPushNotificationsPublisher pushNotificationsPublisher)
        {
            _log = logFactory.CreateLog(this);
            _pbfClient = pbfClient;
            _successfulPaymentTransferPublisher = successfulPaymentTransferPublisher;
            _refundPaymentTransferPublisher = refundPaymentTransferPublisher;
            _emailsPublisher = emailsPublisher;
            _pushNotificationsPublisher = pushNotificationsPublisher;
        }

        public async Task HandlePaymentTransferProcessed(IPaymentTransfer paymentTransfer)
        {
            if (paymentTransfer.Status == PaymentTransferStatus.Accepted)
            {
                await _successfulPaymentTransferPublisher.PublishAsync(new SuccessfulPaymentTransferEvent
                {
                    CustomerId = paymentTransfer.CustomerId,
                    Amount = paymentTransfer.Amount,
                    CampaignId = paymentTransfer.CampaignId,
                    TransferId = paymentTransfer.TransferId,
                    Timestamp = paymentTransfer.Timestamp,
                    InvoiceId = paymentTransfer.InvoiceId,
                    InstalmentName = paymentTransfer.InstalmentName,
                    LocationCode = paymentTransfer.LocationCode,
                });
                
                _log.Info("Published SuccessfulPaymentTransferEvent", context: paymentTransfer.TransferId);

                var balanceResponse =
                    await _pbfClient.CustomersApi.GetBalanceAsync(Guid.Parse(paymentTransfer.CustomerId));

                if (balanceResponse.Error != CustomerBalanceError.None)
                {
                    _log.Error(message: "Customer balance response error",
                        context: new {paymentTransfer.CustomerId, error = balanceResponse.Error.ToString()});
                }

                var balance = balanceResponse.Error == CustomerBalanceError.None ? balanceResponse.Total : 0;
                
                await _pushNotificationsPublisher.PublishPaymentTransferAcceptedAsync(
                    paymentTransfer.CustomerId, paymentTransfer.InvoiceId, balance);

                await _emailsPublisher.SendPaymentTransferAcceptedAsync(
                    paymentTransfer.CustomerId, paymentTransfer.InvoiceId, balance);
            }

            if (paymentTransfer.Status == PaymentTransferStatus.Rejected)
            {
                await _refundPaymentTransferPublisher.PublishAsync(new RefundPaymentTransferEvent
                {
                    CustomerId = paymentTransfer.CustomerId,
                    Amount = paymentTransfer.Amount,
                    CampaignId = paymentTransfer.CampaignId,
                    TransferId = paymentTransfer.TransferId,
                    Timestamp = paymentTransfer.Timestamp,
                    InvoiceId = paymentTransfer.InvoiceId,
                    InstalmentName = paymentTransfer.InstalmentName,
                    LocationCode = paymentTransfer.LocationCode,
                });
                
                _log.Info("Published RefundPaymentTransferEvent", context: paymentTransfer.TransferId);
                
                await _pushNotificationsPublisher.PublishPaymentTransferRejectedAsync(
                    paymentTransfer.CustomerId, paymentTransfer.InvoiceId);

                await _emailsPublisher.SendPaymentTransferRejectedAsync(
                    paymentTransfer.CustomerId, paymentTransfer.InvoiceId);
            }
        }

    }
}
