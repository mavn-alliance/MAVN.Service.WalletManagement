using Autofac;
using JetBrains.Annotations;
using Lykke.Sdk;
using Lykke.Service.Campaign.Client;
using Lykke.Service.CustomerProfile.Client;
using Lykke.Service.PaymentTransfers.Client;
using Lykke.Service.PrivateBlockchainFacade.Client;
using MAVN.Service.WalletManagement.Domain.Services;
using MAVN.Service.WalletManagement.DomainServices;
using MAVN.Service.WalletManagement.Services;
using MAVN.Service.WalletManagement.Settings;
using Lykke.SettingsReader;

namespace MAVN.Service.WalletManagement.Modules
{
    [UsedImplicitly]
    public class ServiceModule : Module
    {
        private readonly IReloadingManager<AppSettings> _appSettings;

        public ServiceModule(IReloadingManager<AppSettings> appSettings)
        {
            _appSettings = appSettings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<CashInService>()
                .As<ICashInService>()
                .WithParameter(TypedParameter.From(_appSettings.CurrentValue.Constants.TokenSymbol))
                .SingleInstance();

            builder.RegisterCustomerProfileClient(_appSettings.CurrentValue.CustomerProfileService, null);

            builder.RegisterType<StartupManager>()
                .As<IStartupManager>();
            builder.RegisterType<ShutdownManager>()
                .As<IShutdownManager>()
                .AutoActivate();

            builder.RegisterType<TransferService>()
                .As<ITransferService>()
                .WithParameter(TypedParameter.From(_appSettings.CurrentValue.Constants.TokenSymbol))
                .SingleInstance();

            builder.RegisterType<WalletManagementService>()
                .As<IWalletManagementService>()
                .SingleInstance();

            builder.RegisterPrivateBlockchainFacadeClient(_appSettings.CurrentValue.PrivateBlockchainFacadeService, null);

            builder.RegisterPaymentTransfersClient(_appSettings.CurrentValue.PaymentTransfersService, null);

            builder.RegisterType<PaymentTransfersService>()
                .As<IPaymentTransfersService>()
                .SingleInstance();

            builder.RegisterCampaignClient(_appSettings.CurrentValue.CampaignService, null);

            builder.RegisterType<PartnersPaymentsService>()
                .As<IPartnersPaymentsService>();

            builder.RegisterType<PushNotificationsSettingsService>()
                .WithParameter("p2PTransferSucceededForSenderTemplateId",
                    _appSettings.CurrentValue.WalletManagementService.Notifications.PushNotifications
                        .P2PTransferSucceededForSenderTemplateId)
                .WithParameter("p2PTransferSucceededForReceiverTemplateId",
                    _appSettings.CurrentValue.WalletManagementService.Notifications.PushNotifications
                        .P2PTransferSucceededForReceiverTemplateId)
                .WithParameter("p2PTransferFailedForSenderTemplateId",
                    _appSettings.CurrentValue.WalletManagementService.Notifications.PushNotifications
                        .P2PTransferFailedForSenderTemplateId)
                .WithParameter("campaignCompletedTemplateId",
                    _appSettings.CurrentValue.WalletManagementService.Notifications.PushNotifications
                        .CampaignCompletedTemplateId)
                .WithParameter("campaignConditionCompletedTemplateId",
                    _appSettings.CurrentValue.WalletManagementService.Notifications.PushNotifications
                        .CampaignConditionCompletedTemplateId)
                .WithParameter("partnerPaymentCreatedTemplateId",
                    _appSettings.CurrentValue.WalletManagementService.Notifications.PushNotifications
                        .PartnerPaymentCreatedTemplateId)
                .WithParameter("paymentTransferAcceptedTemplateId",
                    _appSettings.CurrentValue.WalletManagementService.Notifications.PushNotifications
                        .PaymentTransferAcceptedTemplateId)
                .WithParameter("paymentTransferRejectedTemplateId",
                    _appSettings.CurrentValue.WalletManagementService.Notifications.PushNotifications
                        .PaymentTransferRejectedTemplateId)
                .As<IPushNotificationsSettingsService>()
                .SingleInstance();

            builder.RegisterType<EmailNotificationsSettingsService>()
                .WithParameter("p2PSuccessForSenderTemplateSettings",
                    _appSettings.CurrentValue.WalletManagementService.Notifications.EmailNotifications
                        .P2PTransferSucceededForSender)
                .WithParameter("p2PSuccessForReceiverTemplateSettings",
                    _appSettings.CurrentValue.WalletManagementService.Notifications.EmailNotifications
                        .P2PTransferSucceededForReceiver)
                .WithParameter("p2PFailureForSenderTemplateSettings",
                    _appSettings.CurrentValue.WalletManagementService.Notifications.EmailNotifications
                        .P2PTransferFailedForSender)
                .WithParameter("paymentTransferAcceptedTemplateSettings",
                    _appSettings.CurrentValue.WalletManagementService.Notifications.EmailNotifications
                        .PaymentTransferAccepted)
                .WithParameter("paymentTransferRejectedTemplateSettings",
                    _appSettings.CurrentValue.WalletManagementService.Notifications.EmailNotifications
                        .PaymentTransferRejected)
                .As<IEmailNotificationsSettingsService>()
                .SingleInstance();

            builder.RegisterType<MoneyFormatter>()
                .WithParameter("tokenFormatCultureInfo",
                    _appSettings.CurrentValue.Constants.TokenFormatCultureInfo)
                .WithParameter("tokenNumberDecimalPlaces",
                    _appSettings.CurrentValue.Constants.TokenNumberDecimalPlaces)
                .WithParameter("tokenIntegerPartFormat",
                    _appSettings.CurrentValue.Constants.TokenIntegerPartFormat)
                .As<IMoneyFormatter>();
        }
    }
}
