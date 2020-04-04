namespace MAVN.Service.WalletManagement.Domain.Enums
{
    public enum TransferErrorCodes
    {
        None,
        InvalidSenderId,
        InvalidRecipientId,
        SenderWalletMissing,
        RecipientWalletMissing,
        InvalidAmount,
        NotEnoughFunds,
        DuplicateRequest,
        InvalidAdditionalDataFormat,
        TargetCustomerNotFound,
        SourceCustomerNotFound,
        TransferSourceAndTargetMustBeDifferent,
        SourceCustomerWalletBlocked,
        TargetCustomerWalletBlocked
    }
}
