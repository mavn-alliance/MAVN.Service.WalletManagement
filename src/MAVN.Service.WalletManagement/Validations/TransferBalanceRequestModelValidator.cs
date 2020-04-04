using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using MAVN.Service.WalletManagement.Client.Models.Requests;

namespace MAVN.Service.WalletManagement.Validations
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
