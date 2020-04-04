using System;

namespace MAVN.Service.WalletManagement.Domain.Exceptions
{
    public class NotSupportedErrorCodeException : Exception
    {
        public NotSupportedErrorCodeException(string message) : base(message)
        {            
        }
    }
}
