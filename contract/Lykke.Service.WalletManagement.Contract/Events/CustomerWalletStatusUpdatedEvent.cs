namespace Lykke.Service.WalletManagement.Contract.Events
{
    public class CustomerWalletStatusUpdatedEvent
    {
        public string CustomerId { get; set; }

        public bool WalletBlocked { get; set; }
    }
}
