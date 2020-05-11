using Autofac;
using Common;
using JetBrains.Annotations;
using Lykke.Common;
using Lykke.RabbitMqBroker.Publisher;
using Lykke.RabbitMqBroker.Subscriber;
using MAVN.Service.PartnersPayments.Contract;
using MAVN.Service.WalletManagement.Contract.Events;
using MAVN.Service.WalletManagement.Domain.Publishers;
using MAVN.Service.WalletManagement.DomainServices.Publishers;
using MAVN.Service.WalletManagement.DomainServices.Subscribers;
using MAVN.Service.WalletManagement.Settings;
using Lykke.SettingsReader;

namespace MAVN.Service.WalletManagement.Modules
{
    [UsedImplicitly]
    public class RabbitMqModule : Module
    {
        private const string BonusReceivedExchange = "lykke.wallet.bonusreceived";
        private const string TransferBalanceExchange = "lykke.wallet.transfer";
        private const string BonusDetectedExchange = "lykke.wallet.bonusrewarddetected";
        private const string P2PTransferDetectedExchange = "lykke.wallet.p2ptransferdetected";
        private const string SuccessfulPaymentTransferExchange = "lykke.wallet.successfulpaymenttransfer";
        private const string NotificationSystemEmailExchangeName = "lykke.notificationsystem.command.emailmessage";
        private const string NotificationSystemPushNotificationsExchangeName = "notificationsystem.command.pushnotification";
        private const string P2PTransferFailedExchangeName = "lykke.wallet.p2ptransferfailed";
        private const string PartnerPaymentRequestCreatedExchangeName = "lykke.wallet.partnerpaymentrequestcreated";
        private const string PartnerPaymentProcessedExchangeName = "lykke.wallet.partnerspaymentprocessed";
        private const string RefundPaymentTransferExchange = "lykke.wallet.refundpaymenttransfer";
        private const string SuccessfulPartnersPaymentExchange = "lykke.wallet.successfulpartnerspayment";
        private const string RefundPartnersPaymentExchange = "lykke.wallet.refundpartnerspayment";
        private const string CustomerWalletStatusUpdatedExchange = "lykke.wallet.walletstatusupdated";
        private const string BonusCashInExchangeName = "lykke.bonus.bonusissued";

        private readonly AppSettings _appSettings;
        private readonly RabbitMqSettings _settings;

        public RabbitMqModule(IReloadingManager<AppSettings> appSettings)
        {
            _appSettings = appSettings.CurrentValue;
            _settings = _appSettings.WalletManagementService.RabbitMq;
        }

        protected override void Load(ContainerBuilder builder)
        {
            RegisterPublishers(builder);

            RegisterSubscribers(builder);
        }

        private void RegisterPublishers(ContainerBuilder builder)
        {
            var connString = _settings.RabbitMqConnectionString;

            builder.RegisterJsonRabbitPublisher<BonusReceivedEvent>(
                connString,
                BonusReceivedExchange);

            builder.RegisterJsonRabbitPublisher<P2PTransferEvent>(
                connString,
                TransferBalanceExchange);

            builder.RegisterJsonRabbitPublisher<SuccessfulPaymentTransferEvent>(
                connString,
                SuccessfulPaymentTransferExchange);

            builder.RegisterJsonRabbitPublisher<RefundPaymentTransferEvent>(
                connString,
                RefundPaymentTransferExchange);

            builder.RegisterJsonRabbitPublisher<SuccessfulPartnersPaymentEvent>(
                connString,
                SuccessfulPartnersPaymentExchange);

            builder.RegisterJsonRabbitPublisher<RefundPartnersPaymentEvent>(
                connString,
                RefundPartnersPaymentExchange);

            builder.RegisterJsonRabbitPublisher<CustomerWalletStatusUpdatedEvent>(
                connString,
                CustomerWalletStatusUpdatedExchange);

            var notificationRabbitMqConnString = _settings.NotificationRabbitMqConnectionString;

            builder.RegisterType<PushNotificationsPublisher>()
                .WithParameter("connectionString", notificationRabbitMqConnString)
                .WithParameter("exchangeName", NotificationSystemPushNotificationsExchangeName)
                .WithParameter("tokenFormatCultureInfo",
                    _appSettings.Constants.TokenFormatCultureInfo)
                .WithParameter("tokenNumberDecimalPlaces",
                    _appSettings.Constants.TokenNumberDecimalPlaces)
                .WithParameter("tokenIntegerPartFormat",
                    _appSettings.Constants.TokenIntegerPartFormat)

                .As<IPushNotificationsPublisher>()
                .As<IStartable>()
                .As<IStopable>()
                .SingleInstance();

            builder.RegisterType<EmailNotificationPublisher>()
                .WithParameter("connectionString", notificationRabbitMqConnString)
                .WithParameter("exchangeName", NotificationSystemEmailExchangeName)
                .WithParameter("customerSupportNumber", _appSettings.WalletManagementService.CustomerSupportPhoneNumber)
                .WithParameter("tokenFormatCultureInfo",
                    _appSettings.Constants.TokenFormatCultureInfo)
                .WithParameter("tokenNumberDecimalPlaces",
                    _appSettings.Constants.TokenNumberDecimalPlaces)
                .WithParameter("tokenIntegerPartFormat",
                    _appSettings.Constants.TokenIntegerPartFormat)

                .As<IEmailNotificationsPublisher>()
                .As<IStartable>()
                .As<IStopable>()
                .SingleInstance();
        }

        private void RegisterSubscribers(ContainerBuilder builder)
        {
            var connString = _settings.RabbitMqConnectionString;

            builder.RegisterType<RegistrationBonusSubscriber>()
                .As<IStartStop>()
                .SingleInstance()
                .WithParameter("connectionString", connString)
                .WithParameter("exchangeName", BonusCashInExchangeName);

            builder.RegisterType<BonusRewardDetectedSubscriber>()
                .As<IStartStop>()
                .SingleInstance()
                .WithParameter("connectionString", connString)
                .WithParameter("exchangeName", BonusDetectedExchange)
                .WithParameter("queueName", "walletmanagement");

            builder.RegisterType<P2PTransferDetectedSubscriber>()
                .As<IStartStop>()
                .SingleInstance()
                .WithParameter("connectionString", connString)
                .WithParameter("exchangeName", P2PTransferDetectedExchange)
                .WithParameter("queueName", "walletmanagement");

            builder.RegisterType<PartnerPaymentRequestCreatedSubscriber>()
                .As<IStartStop>()
                .SingleInstance()
                .WithParameter("connectionString", connString)
                .WithParameter("exchangeName", PartnerPaymentRequestCreatedExchangeName)
                .WithParameter("queueName", "walletmanagement");

            builder.RegisterType<PartnersPaymentProcessedSubscriber>()
                .As<JsonRabbitSubscriber<PartnersPaymentProcessedEvent>>()
                .As<IStopable>()
                .SingleInstance()
                .WithParameter("connectionString", connString)
                .WithParameter("exchangeName", PartnerPaymentProcessedExchangeName)
                .WithParameter("queueName", "walletmanagement");

            builder.RegisterType<P2PTransferFailedSubscriber>()
                .As<IStartStop>()
                .SingleInstance()
                .WithParameter("connectionString", connString)
                .WithParameter("exchangeName", P2PTransferFailedExchangeName)
                .WithParameter("queueName", "walletmanagement");
        }
    }
}
