using Falcon.Numerics;
using MAVN.Service.WalletManagement.Domain.Services;
using System;
using System.Globalization;

namespace MAVN.Service.WalletManagement.DomainServices
{
    public class MoneyFormatter : IMoneyFormatter
    {
        private readonly string _tokenFormatCultureInfo;
        private readonly int _tokenNumberDecimalPlaces;
        private readonly string _tokenIntegerPartFormat;

        public MoneyFormatter(
            string tokenFormatCultureInfo,
            int tokenNumberDecimalPlaces,
            string tokenIntegerPartFormat)
        {
            _tokenNumberDecimalPlaces = tokenNumberDecimalPlaces;
            _tokenFormatCultureInfo = tokenFormatCultureInfo
                                      ?? throw new ArgumentNullException(nameof(tokenFormatCultureInfo));
            _tokenIntegerPartFormat = tokenIntegerPartFormat
                                      ?? throw new ArgumentNullException(nameof(tokenIntegerPartFormat));
        }

        public string FormatAmountToDisplayString(Money18 money18)
        {
            var formatInfo =
                new CultureInfo(_tokenFormatCultureInfo).NumberFormat;

            return money18.ToString(_tokenIntegerPartFormat, _tokenNumberDecimalPlaces, formatInfo);
        }
    }
}
