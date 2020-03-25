using JetBrains.Annotations;

namespace Lykke.Service.WalletManagement.Client.Enums
{
    /// <summary>
    /// Transfer error codes Enum
    /// </summary>
    [PublicAPI]
    public enum TransferErrorCodes
    {
        /// <summary>
        /// no error
        /// </summary>
        None,
        /// <summary>
        /// The source customer id is not valid
        /// </summary>
        InvalidSenderId,
        /// <summary>
        /// The recipient customer id is not valid
        /// </summary>
        InvalidRecipientId,
        /// <summary>
        /// The source customer does not have a wallet
        /// </summary>
        SenderWalletMissing,
        /// <summary>
        /// The recipient customer does not have a wallet
        /// </summary>
        RecipientWalletMissing,
        /// <summary>
        /// The value for amount is not valid
        /// </summary>
        InvalidAmount,
        /// <summary>
        /// The source customer does not have enough tokens
        /// </summary>
        NotEnoughFunds,
        /// <summary>
        /// There is a request duplication
        /// </summary>
        DuplicateRequest,
        /// <summary>
        /// The additional data passed in the request was not in a valid format
        /// </summary>
        InvalidAdditionalDataFormat,
        /// <summary>
        /// The target customer is not found
        /// </summary>
        TargetCustomerNotFound,
        /// <summary>
        /// The source customer is not found
        /// </summary>
        SourceCustomerNotFound,
        /// <summary>
        /// Transfer source and target must be different
        /// </summary>
        TransferSourceAndTargetMustBeDifferent,
        /// <summary>
        /// The source Customer's Wallet is blocked
        /// </summary>
        SourceCustomerWalletBlocked,
        /// <summary>
        /// The recipient Customer's Wallet is blocked
        /// </summary>
        TargetCustomerWalletBlocked

    }
}
