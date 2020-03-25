using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using Lykke.Service.WalletManagement.Client.Models.Requests;

namespace Lykke.Service.WalletManagement.Validations
{
    public class TransferBalanceRequestModelValidator 
        : AbstractValidator<TransferBalanceRequestModel>
    {
        public TransferBalanceRequestModelValidator()
        {
            RuleFor(tbr => tbr.Amount)
                .GreaterThan(0);
        }
    }
}
