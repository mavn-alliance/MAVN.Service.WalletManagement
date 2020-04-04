using System;
using System.Net;
using System.Threading.Tasks;
using Lykke.Common.Api.Contract.Responses;
using Lykke.Common.ApiLibrary.Exceptions;
using Lykke.Logs;
using Lykke.RabbitMqBroker.Publisher;
using Lykke.Service.CustomerProfile.Client;
using Lykke.Service.CustomerProfile.Client.Models.Enums;
using Lykke.Service.CustomerProfile.Client.Models.Responses;
using Lykke.Service.PrivateBlockchainFacade.Client;
using Lykke.Service.PrivateBlockchainFacade.Client.Models;
using MAVN.Service.WalletManagement.Contract.Events;
using MAVN.Service.WalletManagement.Domain;
using MAVN.Service.WalletManagement.Domain.Models;
using MAVN.Service.WalletManagement.Domain.Publishers;
using MAVN.Service.WalletManagement.Domain.Services;
using MAVN.Service.WalletManagement.DomainServices;
using Moq;
using Xunit;
using TransferErrorCodes = Lykke.Service.WalletManagement.Domain.Enums.TransferErrorCodes;

namespace MAVN.Service.WalletManagement.Tests
{
    public class TransferServiceTests
    {
        private readonly Mock<IPrivateBlockchainFacadeClient> _blockchainFacadeMock = new Mock<IPrivateBlockchainFacadeClient>();

        private readonly Mock<ITransfersRepository> _transfersRepositoryMock = new Mock<ITransfersRepository>();

        private readonly Mock<IWalletManagementService> _walletManagementServiceMock = new Mock<IWalletManagementService>();

        private readonly Mock<IEmailNotificationsPublisher> _emailNotificationsPublisherMock = new Mock<IEmailNotificationsPublisher>();

        private readonly Mock<IPushNotificationsPublisher> _pushNotificationsPublisherMock = new Mock<IPushNotificationsPublisher>();

        private readonly Mock<ICustomerProfileClient> _customerProfileMock = new Mock<ICustomerProfileClient>();

        private readonly Mock<IRabbitPublisher<P2PTransferEvent>> _publisherMock = new Mock<IRabbitPublisher<P2PTransferEvent>>();


        private const string FakeSenderCustomerId = "3f1443e2-b848-4567-8fb5-ebe7337a87e9";

        private const string FakeRecipientCustomerId = "60d4f58d-7fde-4640-8d0e-feb97b12a90e";

        private const string ValidTransactionHash = "0x09273094bf95663c9cef1b794816bc7bc530bb736311140458dc588baa26092a";

        private const int ValidAmount = 100;

        private const string TokenSymbol = "MVN";

        private const string FakeSenderEmail = "sedner@mail.com";

        private const string FakeReceiverEmail = "receiver@mail.com";

        private const string FakeContextId = "transactionId";

        private const long FakeSenderBalance = 123;

        private const long FakeReceiverBalance = 132;

        private const string UnknownCustomerEmail = "Unknown";

        private const long UnknownCustomerBalance = 0;

        [Fact]
        public async Task CustomerTriesToTransferAsset_EverythingValid_SuccessfulTransaction()
        {
            _customerProfileMock
                .Setup(x => x.CustomerProfiles.GetByCustomerIdAsync(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(new CustomerProfileResponse
                {
                    Profile = new CustomerProfile.Client.Models.Responses.CustomerProfile
                    {
                        Email = "test@email.com",
                    }
                });

            _transfersRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<ITransfer>()))
                .Returns(Task.CompletedTask);

            _blockchainFacadeMock
                .Setup(x => x.TransfersApi.TransferAsync(It.IsAny<TransferRequestModel>()))
                .ReturnsAsync(new TransferResponseModel { Error = TransferError.None });

            _walletManagementServiceMock
                .Setup(x => x.IsCustomerWalletBlockedAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            var transferService = CreateSutInstance();

            var result = await transferService.TransferBalanceAsync(
                Guid.NewGuid().ToString(),
                FakeSenderCustomerId,
                FakeRecipientCustomerId,
                1);

            Assert.NotNull(result);
            Assert.True(result.ErrorCode == (TransferErrorCodes)Client.Enums.TransferErrorCodes.None);
        }

        [Theory]
        [InlineData(TransferError.NotEnoughFunds, TransferErrorCodes.NotEnoughFunds)]
        [InlineData(TransferError.SenderWalletMissing, TransferErrorCodes.SenderWalletMissing)]
        [InlineData(TransferError.RecipientWalletMissing, TransferErrorCodes.RecipientWalletMissing)]
        public async Task CustomerTriesToTransferAsset_ErrorOnPbf_ErrorReturned(TransferError pbfError, TransferErrorCodes wmError)
        {
            _customerProfileMock
                .Setup(x => x.CustomerProfiles.GetByCustomerIdAsync(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(new CustomerProfileResponse
                {
                    Profile = new CustomerProfile.Client.Models.Responses.CustomerProfile
                    {
                        Email = "test@email.com",
                    }
                });

            _transfersRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<ITransfer>()))
                .Returns(Task.CompletedTask);

            _blockchainFacadeMock
                .Setup(x => x.TransfersApi.TransferAsync(It.IsAny<TransferRequestModel>()))
                .ReturnsAsync(new TransferResponseModel { Error = pbfError });

            _walletManagementServiceMock
                .Setup(x => x.IsCustomerWalletBlockedAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            var transferService = CreateSutInstance();

            var result = await transferService.TransferBalanceAsync(
                Guid.NewGuid().ToString(),
                FakeSenderCustomerId,
                FakeRecipientCustomerId,
                1);

            Assert.NotNull(result);
            Assert.Equal(wmError, result.ErrorCode);
        }

        [Theory]
        [InlineData("sender", FakeRecipientCustomerId, TransferErrorCodes.InvalidSenderId)]
        [InlineData(FakeSenderCustomerId, "receiver", TransferErrorCodes.InvalidRecipientId)]
        [InlineData("sender", "receiver", TransferErrorCodes.InvalidSenderId)]
        public async Task CustomerTriesToTransferAsset_InvalidCustomerId_ErrorReturned
            (string senderId, string receiverId, TransferErrorCodes wmError)
        {
            _customerProfileMock
                .Setup(x => x.CustomerProfiles.GetByCustomerIdAsync(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(new CustomerProfileResponse
                {
                    Profile = new CustomerProfile.Client.Models.Responses.CustomerProfile
                    {
                        Email = "test@email.com",
                    }
                });

            var transferService = CreateSutInstance();

            var result = await transferService.TransferBalanceAsync(
                Guid.NewGuid().ToString(),
                senderId,
                receiverId,
                1);

            Assert.NotNull(result);
            Assert.Equal(wmError, result.ErrorCode);
        }

        [Fact]
        public async Task CustomerTriesToTransferAsset_TargetIsNotValidCustomer_ResponseHasErrorCode()
        {
            _customerProfileMock
                .Setup(x => x.CustomerProfiles.GetByCustomerIdAsync(FakeSenderCustomerId, It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(new CustomerProfileResponse
                {
                    Profile = new CustomerProfile.Client.Models.Responses.CustomerProfile
                    {
                        Email = "test@email.com",
                    }
                });
            _customerProfileMock
                .Setup(x => x.CustomerProfiles.GetByCustomerIdAsync(FakeRecipientCustomerId, It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync((CustomerProfileResponse)null);

            var transferService = CreateSutInstance();

            var result = await transferService.TransferBalanceAsync(
                Guid.NewGuid().ToString(),
                FakeSenderCustomerId,
                FakeRecipientCustomerId,
                1);

            Assert.NotNull(result);
            Assert.Equal(TransferErrorCodes.TargetCustomerNotFound, result.ErrorCode);
        }

        [Fact]
        public async Task CustomerTriesToTransferAsset_SourceIsNotValidCustomer_ResponseHasErrorCode()
        {
            _customerProfileMock
                .Setup(x => x.CustomerProfiles.GetByCustomerIdAsync(FakeSenderCustomerId, It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync((CustomerProfileResponse)null);

            var transferService = CreateSutInstance();

            var result = await transferService.TransferBalanceAsync(
                Guid.NewGuid().ToString(),
                FakeSenderCustomerId,
                FakeRecipientCustomerId,
                1);

            Assert.NotNull(result);
            Assert.Equal(TransferErrorCodes.SourceCustomerNotFound, result.ErrorCode);
        }


        [Fact]
        public async Task ProcessP2PTransferDetectedEventAsync_ContextNotFound_RaisesException()
        {
            _transfersRepositoryMock
                .Setup(x => x.GetAsync(It.IsAny<string>()))
                .ReturnsAsync((ITransfer)null);

            var transferService = CreateSutInstance();

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                transferService.ProcessP2PTransferDetectedEventAsync(
                    "whatever",
                    ValidTransactionHash,
                    FakeSenderCustomerId,
                    FakeRecipientCustomerId,
                    ValidAmount,
                    DateTime.UtcNow));
        }

        [Fact]
        public async Task ProcessP2PTransferDetectedEventAsync_SuccessfullyProcessed()
        {
            _transfersRepositoryMock
                .Setup(x => x.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(new TransferDto
                {
                    AssetSymbol = TokenSymbol,
                    OperationId = "opId",
                    ExternalOperationId = "extOpId",                    
                });

            _blockchainFacadeMock.Setup(x => x.CustomersApi.GetBalanceAsync(Guid.Parse(FakeSenderCustomerId)))
                .ReturnsAsync(new CustomerBalanceResponseModel
                {
                    Error = CustomerBalanceError.None,
                    Total = FakeSenderBalance
                });

            _blockchainFacadeMock.Setup(x => x.CustomersApi.GetBalanceAsync(Guid.Parse(FakeRecipientCustomerId)))
                .ReturnsAsync(new CustomerBalanceResponseModel
                {
                    Error = CustomerBalanceError.None,
                    Total = FakeReceiverBalance
                });

            _customerProfileMock.Setup(x => x.CustomerProfiles.GetByCustomerIdAsync(FakeSenderCustomerId, It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(new CustomerProfileResponse
                {
                    ErrorCode = CustomerProfileErrorCodes.None,
                    Profile = new CustomerProfile.Client.Models.Responses.CustomerProfile
                    {
                        Email = FakeSenderEmail
                    }
                });

            _customerProfileMock.Setup(x => x.CustomerProfiles.GetByCustomerIdAsync(FakeRecipientCustomerId, It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(new CustomerProfileResponse
                {
                    ErrorCode = CustomerProfileErrorCodes.None,
                    Profile = new CustomerProfile.Client.Models.Responses.CustomerProfile
                    {
                        Email = FakeReceiverEmail
                    }
                });

            var transferService = CreateSutInstance();

            await transferService.ProcessP2PTransferDetectedEventAsync(
                FakeContextId,
                ValidTransactionHash,
                FakeSenderCustomerId,
                FakeRecipientCustomerId,
                ValidAmount,
                DateTime.UtcNow);

            _emailNotificationsPublisherMock.Verify(x => x.SendP2PSucceededForReceiverAsync(FakeRecipientCustomerId,
                FakeContextId, ValidAmount, It.IsAny<DateTime>(), FakeReceiverBalance,
                FakeSenderEmail), Times.Once);

            _emailNotificationsPublisherMock.Verify(x => x.SendP2PSucceededForSenderAsync(FakeSenderCustomerId,
                FakeContextId, ValidAmount, It.IsAny<DateTime>(), FakeSenderBalance,
                FakeReceiverEmail), Times.Once);

            _pushNotificationsPublisherMock.Verify(x => x.PublishP2PSucceededForSenderAsync(FakeSenderCustomerId),
                Times.Once);

            _pushNotificationsPublisherMock.Verify(
                x => x.PublishP2PSucceededForReceiverAsync(FakeRecipientCustomerId, ValidAmount,
                    FakeSenderEmail),
                Times.Once);
            _transfersRepositoryMock.Verify(x => x.DeleteAsync(FakeContextId), Times.Once);
        }

        [Fact]
        public async Task ProcessP2PTransferFailed_EventContextDoesNotExists_PbfIsNotCalled()
        {
            _transfersRepositoryMock.Setup(x => x.GetAsync(FakeContextId))
                .ReturnsAsync((ITransfer) null);

            var sut = CreateSutInstance();

            await sut.ProcessP2PTransferFailed(FakeContextId, ValidTransactionHash, FakeSenderCustomerId,
                FakeRecipientCustomerId, ValidAmount, DateTime.UtcNow);

            _blockchainFacadeMock.Verify(x => x.CustomersApi.GetBalanceAsync(Guid.Parse(FakeSenderCustomerId)),
                Times.Never);
        }

        [Fact]
        public async Task ProcessP2PTransferFailed_SenderBalanceAndReceiverEmailDoesNotExist_SuccessfullyProcessedWithUnknownBalanceAndEmail()
        {
            _transfersRepositoryMock.Setup(x => x.GetAsync(FakeContextId))
                .ReturnsAsync(new TransferDto());

            _blockchainFacadeMock.Setup(x => x.CustomersApi.GetBalanceAsync(Guid.Parse(FakeSenderCustomerId)))
                .ReturnsAsync(new CustomerBalanceResponseModel
                {
                    Error = CustomerBalanceError.CustomerWalletMissing
                });

            _customerProfileMock.Setup(x => x.CustomerProfiles.GetByCustomerIdAsync(FakeRecipientCustomerId, It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(new CustomerProfileResponse
                {
                    ErrorCode = CustomerProfileErrorCodes.CustomerProfileDoesNotExist
                });

            var sut = CreateSutInstance();

            await sut.ProcessP2PTransferFailed(FakeContextId, ValidTransactionHash, FakeSenderCustomerId,
                FakeRecipientCustomerId, ValidAmount, DateTime.UtcNow);

            _emailNotificationsPublisherMock.Verify(x => x.SendP2PFailedForSenderAsync(FakeSenderCustomerId,
                FakeContextId, ValidAmount, It.IsAny<DateTime>(), UnknownCustomerBalance,
                UnknownCustomerEmail), Times.Once);

            _pushNotificationsPublisherMock.Verify(x => x.PublishP2PFailedForSenderAsync(FakeSenderCustomerId),
                Times.Once);
            _transfersRepositoryMock.Verify(x => x.DeleteAsync(FakeContextId), Times.Once);
        }

        [Fact]
        public async Task ProcessP2PTransferFailed_GetSenderBalanceAndReceiverEmailThrowsException_SuccessfullyProcessedWithUnknownBalanceAndEmail()
        {
            _transfersRepositoryMock.Setup(x => x.GetAsync(FakeContextId))
                .ReturnsAsync(new TransferDto());

            _blockchainFacadeMock.Setup(x => x.CustomersApi.GetBalanceAsync(Guid.Parse(FakeSenderCustomerId)))
                .ThrowsAsync(new ClientApiException(HttpStatusCode.NotFound,new ErrorResponse()));

            _customerProfileMock.Setup(x => x.CustomerProfiles.GetByCustomerIdAsync(FakeRecipientCustomerId, It.IsAny<bool>(), It.IsAny<bool>()))
                .ThrowsAsync(new ClientApiException(HttpStatusCode.NotFound, new ErrorResponse()));

            var sut = CreateSutInstance();

            await sut.ProcessP2PTransferFailed(FakeContextId, ValidTransactionHash, FakeSenderCustomerId,
                FakeRecipientCustomerId, ValidAmount, DateTime.UtcNow);

            _emailNotificationsPublisherMock.Verify(x => x.SendP2PFailedForSenderAsync(FakeSenderCustomerId,
                FakeContextId, ValidAmount, It.IsAny<DateTime>(), UnknownCustomerBalance,
                UnknownCustomerEmail), Times.Once);

            _pushNotificationsPublisherMock.Verify(x => x.PublishP2PFailedForSenderAsync(FakeSenderCustomerId),
                Times.Once);
            _transfersRepositoryMock.Verify(x => x.DeleteAsync(FakeContextId), Times.Once);
        }

        [Fact]
        public async Task ProcessP2PTransferFailed_SuccessfullyProcessed()
        {
            _transfersRepositoryMock.Setup(x => x.GetAsync(FakeContextId))
                .ReturnsAsync(new TransferDto());

            _blockchainFacadeMock.Setup(x => x.CustomersApi.GetBalanceAsync(Guid.Parse(FakeSenderCustomerId)))
                .ReturnsAsync(new CustomerBalanceResponseModel
                {
                    Error = CustomerBalanceError.None,
                    Total = FakeSenderBalance
                });

            _customerProfileMock.Setup(x => x.CustomerProfiles.GetByCustomerIdAsync(FakeRecipientCustomerId, It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(new CustomerProfileResponse
                {
                    ErrorCode = CustomerProfileErrorCodes.None,
                    Profile = new CustomerProfile.Client.Models.Responses.CustomerProfile
                    {
                        Email = FakeReceiverEmail
                    }
                });

            var sut = CreateSutInstance();

            await sut.ProcessP2PTransferFailed(FakeContextId, ValidTransactionHash, FakeSenderCustomerId,
                FakeRecipientCustomerId, ValidAmount, DateTime.UtcNow);

            _emailNotificationsPublisherMock.Verify(x => x.SendP2PFailedForSenderAsync(FakeSenderCustomerId,
                FakeContextId, ValidAmount, It.IsAny<DateTime>(), FakeSenderBalance,
                FakeReceiverEmail), Times.Once);

            _pushNotificationsPublisherMock.Verify(x => x.PublishP2PFailedForSenderAsync(FakeSenderCustomerId),
                Times.Once);
            _transfersRepositoryMock.Verify(x => x.DeleteAsync(FakeContextId), Times.Once);
        }

        //TODO: add wallet blocking tests
        private TransferService CreateSutInstance()
        {
            return new TransferService(
                TokenSymbol,
                _customerProfileMock.Object,
                _blockchainFacadeMock.Object,
                _walletManagementServiceMock.Object,
                _transfersRepositoryMock.Object,
                _publisherMock.Object,
                _emailNotificationsPublisherMock.Object,
                _pushNotificationsPublisherMock.Object,
                EmptyLogFactory.Instance);
        }
    }
}
