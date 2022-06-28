// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="ExchangeRateTask.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Directory;
using Adrack.Service.Agent;
using System;

namespace Adrack.Service.Directory
{
    /// <summary>
    /// Represents a Exchange Rate  Task
    /// Implements the <see cref="Adrack.Service.Agent.ITask" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Agent.ITask" />
    public partial class ExchangeRateTask : ITask
    {
        #region Fields

        /// <summary>
        /// Currency Setting
        /// </summary>
        private readonly CurrencySetting _currencySetting;

        /// <summary>
        /// Currency Service
        /// </summary>
        private readonly ICurrencyService _currencyService;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Ping Task
        /// </summary>
        /// <param name="currencySetting">Currency Setting</param>
        /// <param name="currencyService">Currency Service</param>
        public ExchangeRateTask(CurrencySetting currencySetting, ICurrencyService currencyService)
        {
            this._currencySetting = currencySetting;
            this._currencyService = currencyService;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Execute
        /// </summary>
        public void Execute()
        {
            if (!_currencySetting.AutoUpdateEnabled)
                return;

            var primaryExchangeRateCurrencyCode = _currencyService.GetCurrencyById(_currencySetting.PrimaryExchangeRateCurrencyId).Code;
            var exchangeRates = _currencyService.GetLiveRates(primaryExchangeRateCurrencyCode);

            foreach (var exchangeRate in exchangeRates)
            {
                var currency = _currencyService.GetCurrencyByCode(exchangeRate.Code);

                if (currency != null)
                {
                    currency.Rate = exchangeRate.Rate;
                    currency.UpdatedOn = DateTime.UtcNow;
                    _currencyService.UpdateCurrency(currency);
                }
            }
        }

        #endregion Methods
    }
}