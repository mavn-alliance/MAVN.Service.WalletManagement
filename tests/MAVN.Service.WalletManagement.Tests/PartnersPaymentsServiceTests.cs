using System.Threading.Tasks;
using Lykke.Logs;
using Lykke.RabbitMqBroker.Publisher;
using MAVN.Service.WalletManagement.Contract.Events;
using MAVN.Service.WalletManagement.Domain.Enums;
using MAVN.Service.WalletManagement.Domain.Models;
using MAVN.Service.WalletManagement.Domain.Publishers;
using MAVN.Service.WalletManagement.DomainServices;
using Moq;
using Xunit;

namespace MAVN.Service.WalletManagement.Tests
{
    public class PartnersPaymentsServiceTests
    {
        private readonly Mock<IPushNotificationsPublisher> _pushNotificationPublisherMock = new Mock<IPushNotificationsPublisher>();
        private readonly Mock<IRabbitPublisher<RefundPartnersPaymentEvent>> _refundPpPublisherMock = new Mock<IRabbitPublisher<RefundPartnersPaymentEvent>>();
        private readonly Mock<IRabbitPublisher<SuccessfulPartnersPaymentEvent>> _successfulPpPublisherMock = new Mock<IRabbitPublisher<SuccessfulPartnersPaymentEvent>>();

        [Fact]
        public async Task HandlePartnerPaymentProcessed_EventWithAcceptedStatus_SuccessfulPartnerPaymentPublisherCalled()
        {
            var sut = CreateSutInstance();

            await sut.HandlePartnersPaymentProcessedAsync(new PartnerPaymentDto
            {
                Status = PartnerPaymentStatus.Accepted
            });

            _refundPpPublisherMock.Verify(x => x.PublishAsync(It.IsAny<RefundPartnersPaymentEvent>()), Times.Never);
            _successfulPpPublisherMock.Verify(x => x.PublishAsync(It.IsAny<SuccessfulPartnersPaymentEvent>()), Times.Once);
        }

        [Fact]
        public async Task HandlePartnerPaymentProcessed_EventWithRejectedStatus_RefundPartnerPaymentPublisherCalled()
        {
            var sut = CreateSutInstance();

            await sut.HandlePartnersPaymentProcessedAsync(new PartnerPaymentDto
            {
                Status = PartnerPaymentStatus.Rejected
            });

            _refundPpPublisherMock.Verify(x => x.PublishAsync(It.IsAny<RefundPartnersPaymentEvent>()), Times.Once);
            _successfulPpPublisherMock.Verify(x => x.PublishAsync(It.IsAny<SuccessfulPartnersPaymentEvent>()), Times.Never);
        }

        public PartnersPaymentsService CreateSutInstance()
        {
            return new PartnersPaymentsService(
                _pushNotificationPublisherMock.Object,
                _refundPpPublisherMock.Object,
                _successfulPpPublisherMock.Object,
                EmptyLogFactory.Instance);
        }
    }
}
