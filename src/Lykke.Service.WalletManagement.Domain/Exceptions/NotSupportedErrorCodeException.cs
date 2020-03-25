using System;

namespace Lykke.Service.WalletManagement.Domain.Exceptions
{
    public class NotSupportedErrorCodeException : Exception
    {
        public NotSupportedErrorCodeException(string message) : base(message)
        {            
        }
    }
}
