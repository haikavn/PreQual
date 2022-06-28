// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="ICurrencyService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Directory;
using System.Collections.Generic;

namespace Adrack.Service.Directory
{
    /// <summary>
    /// Represents a Currency Service
    /// </summary>
    public partial interface ICurrencyService
    {
        #region Methods

        /// <summary>
        /// Get Currency By Id
        /// </summary>
        /// <param name="currencyId">Currency Identifier</param>
        /// <returns>Currency Item</returns>
        Currency GetCurrencyById(long currencyId);

        /// <summary>
        /// Get Currency By Code
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns>Currency Item</returns>
        Currency GetCurrencyByCode(string code);

        /// <summary>
        /// Get Live Rates
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns>Exchange Rate Collection Item</returns>
        IList<ExchangeRate> GetLiveRates(string code);

        /// <summary>
        /// Get All Currencies
        /// </summary>
        /// <returns>Currency Collection Item</returns>
        IList<Currency> GetAllCurrencies();

        /// <summary>
        /// Insert Currency
        /// </summary>
        /// <param name="currency">Currency</param>
        void InsertCurrency(Currency currency);

        /// <summary>
        /// Update Currency
        /// </summary>
        /// <param name="currency">Currency</param>
        void UpdateCurrency(Currency currency);

        /// <summary>
        /// Delete Currency
        /// </summary>
        /// <param name="currency">Currency</param>
        void DeleteCurrency(Currency currency);

        #region Convert

        /// <summary>
        /// Convert Currency
        /// </summary>
        /// <param name="amount">Amount</param>
        /// <param name="exchangeRate">Exchange Rate</param>
        /// <returns>Decimal Item</returns>
        decimal ConvertCurrency(decimal amount, decimal exchangeRate);

        /// <summary>
        /// Convert Currency
        /// </summary>
        /// <param name="amount">Amount</param>
        /// <param name="sourceCode">Source Code</param>
        /// <param name="targetCode">Target Code</param>
        /// <returns>Decimal Item</returns>
        decimal ConvertCurrency(decimal amount, Currency sourceCode, Currency targetCode);

        /// <summary>
        /// Convert To Primary Exchange Rate Currency
        /// </summary>
        /// <param name="amount">Amount</param>
        /// <param name="sourceCode">Source Code</param>
        /// <returns>Decimal Item</returns>
        decimal ConvertToPrimaryExchangeRateCurrency(decimal amount, Currency sourceCode);

        /// <summary>
        /// Convert From Primary Exchange Rate Currency
        /// </summary>
        /// <param name="amount">Amount</param>
        /// <param name="targetCode">Target Code</param>
        /// <returns>Decimal Item</returns>
        decimal ConvertFromPrimaryExchangeRateCurrency(decimal amount, Currency targetCode);

        #endregion Convert

        #endregion Methods
    }
}