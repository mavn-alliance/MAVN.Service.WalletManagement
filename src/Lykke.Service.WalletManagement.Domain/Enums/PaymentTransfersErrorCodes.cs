namespace Lykke.Service.WalletManagement.Domain.Enums
{
    public enum PaymentTransfersErrorCodes
    {
        None,
        /// <summary>Sender customer id is not valid</summary>
        InvalidSenderId,
        /// <summary>Recipient customer id is not valid</summary>
        InvalidRecipientId,
        /// <summary>Sender customer wallet has not been assigned yet</summary>
        SenderWalletMissing,
        /// <summary>Recipient customer wallet has not been assigned yet</summary>
        RecipientWalletMissing,
        /// <summary>Invalid amount</summary>
        InvalidAmount,
        /// <summary>
        /// Sender customer wallet doesn't have enough funds to transfer
        /// </summary>
        NotEnoughFunds,
        /// <summary>The same transfer request has already been received</summary>
        DuplicateRequest,
        /// <summary>Additional data property does not start with "0x"</summary>
        InvalidAdditionalDataFormat,
        /// <summary>
        /// Passed campaignId was not valid
        /// </summary>
        InvalidCampaignId,
        /// <summary>
        /// There is no existing campaign with such id
        /// </summary>
        CampaignNotFound,
        /// <summary>
        /// Customer with such id does not exist
        /// </summary>
        CustomerDoesNotExist,
        /// <summary>
        /// Customer's Wallet is blocked
        /// </summary>
        CustomerWalletBlocked,
    }
}
