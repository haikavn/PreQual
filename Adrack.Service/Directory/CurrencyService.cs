// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="CurrencyService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Cache;
using Adrack.Core.Domain.Directory;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using Adrack.Service.Infrastructure.ApplicationEvent;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Xml;

namespace Adrack.Service.Directory
{
    /// <summary>
    /// Represents a Currency Service
    /// Implements the <see cref="Adrack.Service.Directory.ICurrencyService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Directory.ICurrencyService" />
    public partial class CurrencyService : ICurrencyService
    {
        #region Constants

        /// <summary>
        /// Cache Currency By Id Key
        /// </summary>
        private const string CACHE_CURRENCY_BY_ID_KEY = "App.Cache.Currency.By.Id-{0}";

        /// <summary>
        /// Cache Currency By Code Key
        /// </summary>
        private const string CACHE_CURRENCY_BY_CODE_KEY = "App.Cache.Currency.By.Code-{0}";

        /// <summary>
        /// Cache Currency All Key
        /// </summary>
        private const string CACHE_CURRENCY_ALL_KEY = "App.Cache.Currency.All";

        /// <summary>
        /// Cache Currency Pattern Key
        /// </summary>
        private const string CACHE_CURRENCY_PATTERN_KEY = "App.Cache.Currency.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Currency Setting
        /// </summary>
        private readonly CurrencySetting _currencySetting;

        /// <summary>
        /// Currency
        /// </summary>
        private readonly IRepository<Currency> _currencyRepository;

        /// <summary>
        /// Cache Manager
        /// </summary>
        private readonly ICacheManager _cacheManager;

        /// <summary>
        /// Application Event Publisher
        /// </summary>
        private readonly IAppEventPublisher _appEventPublisher;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Currency Service
        /// </summary>
        /// <param name="currencySetting">Currency Setting</param>
        /// <param name="currencyRepository">Currency Repository</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        public CurrencyService(CurrencySetting currencySetting, IRepository<Currency> currencyRepository, ICacheManager cacheManager, IAppEventPublisher appEventPublisher)
        {
            this._currencySetting = currencySetting;
            this._currencyRepository = currencyRepository;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get Currency By Id
        /// </summary>
        /// <param name="currencyId">Currency Identifier</param>
        /// <returns>Currency Item</returns>
        public virtual Currency GetCurrencyById(long currencyId)
        {
            return _currencyRepository.GetById(currencyId);
        }

        /// <summary>
        /// Get Currency By Code
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns>Currency Item</returns>
        public virtual Currency GetCurrencyByCode(string code)
        {
            if (String.IsNullOrEmpty(code))
                return null;

            string key = string.Format(CACHE_CURRENCY_BY_CODE_KEY, code);

            return _cacheManager.Get(key, () =>
            {
                return GetAllCurrencies().FirstOrDefault(x => x.Code.ToLower() == code.ToLower());
            });
        }

        /// <summary>
        /// Get Live Rates
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns>Exchange Rate Collection Item</returns>
        public virtual IList<ExchangeRate> GetLiveRates(string code)
        {
            //if (String.IsNullOrEmpty(code) || code.ToLower() != "eur")
            //    throw new AppException("Error Set Currency To EURO");   //_localizationService.GetResource("Plugins.ExchangeRate.EcbExchange.SetCurrencyToEURO"));

            var exchangeRates = new List<ExchangeRate>();

            var webRequest = WebRequest.Create("http://www.ecb.int/stats/eurofxref/eurofxref-daily.xml") as HttpWebRequest;

            using (WebResponse response = webRequest.GetResponse())
            {
                var xmlDocument = new XmlDocument();

                xmlDocument.Load(response.GetResponseStream());

                var xmlNamespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);

                xmlNamespaceManager.AddNamespace("ns", "http://www.ecb.int/vocabulary/2002-08-01/eurofxref");
                xmlNamespaceManager.AddNamespace("gesmes", "http://www.gesmes.org/xml/2002-08-01");

                var node = xmlDocument.SelectSingleNode("gesmes:Envelope/ns:Cube/ns:Cube", xmlNamespaceManager);
                var updateDate = DateTime.ParseExact(node.Attributes["time"].Value, "yyyy-MM-dd", null);

                var numberFormatInfo = new NumberFormatInfo
                {
                    NumberDecimalSeparator = ".",
                    NumberGroupSeparator = ""
                };

                foreach (XmlNode node2 in node.ChildNodes)
                {
                    exchangeRates.Add(new ExchangeRate()
                    {
                        Code = node2.Attributes["currency"].Value,
                        Rate = decimal.Parse(node2.Attributes["rate"].Value, numberFormatInfo),
                        UpdatedOn = updateDate
                    }
                    );
                }
            }

            return exchangeRates;
        }

        /// <summary>
        /// Get All Currencies
        /// </summary>
        /// <returns>Currency Collection Item</returns>
        public virtual IList<Currency> GetAllCurrencies()
        {
            string key = CACHE_CURRENCY_ALL_KEY;

            var currencies = _cacheManager.Get(key, () =>
            {
                var query = from x in _currencyRepository.Table
                            orderby x.DisplayOrder, x.Id
                            select x;

                return query.ToList();
            });

            return currencies;
        }

        /// <summary>
        /// Insert Currency
        /// </summary>
        /// <param name="currency">Currency</param>
        /// <exception cref="ArgumentNullException">currency</exception>
        public virtual void InsertCurrency(Currency currency)
        {
            if (currency == null)
                throw new ArgumentNullException("currency");

            _currencyRepository.Insert(currency);

            _cacheManager.RemoveByPattern(CACHE_CURRENCY_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(currency);
        }

        /// <summary>
        /// Update Currency
        /// </summary>
        /// <param name="currency">Currency</param>
        /// <exception cref="ArgumentNullException">currency</exception>
        public virtual void UpdateCurrency(Currency currency)
        {
            if (currency == null)
                throw new ArgumentNullException("currency");

            _currencyRepository.Update(currency);

            _cacheManager.RemoveByPattern(CACHE_CURRENCY_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityUpdated(currency);
        }

        /// <summary>
        /// Delete Currency
        /// </summary>
        /// <param name="currency">Currency</param>
        /// <exception cref="ArgumentNullException">currency</exception>
        public virtual void DeleteCurrency(Currency currency)
        {
            if (currency == null)
                throw new ArgumentNullException("currency");

            _currencyRepository.Delete(currency);

            _cacheManager.RemoveByPattern(CACHE_CURRENCY_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(currency);
        }

        #region Convert

        /// <summary>
        /// Convert Currency
        /// </summary>
        /// <param name="amount">Amount</param>
        /// <param name="exchangeRate">Exchange Rate</param>
        /// <returns>Decimal Item</returns>
        public virtual decimal ConvertCurrency(decimal amount, decimal exchangeRate)
        {
            if (amount != decimal.Zero && exchangeRate != decimal.Zero)
                return amount * exchangeRate;

            return decimal.Zero;
        }

        /// <summary>
        /// Convert Currency
        /// </summary>
        /// <param name="amount">Amount</param>
        /// <param name="sourceCode">Source Code</param>
        /// <param name="targetCode">Target Code</param>
        /// <returns>Decimal Item</returns>
        public virtual decimal ConvertCurrency(decimal amount, Currency sourceCode, Currency targetCode)
        {
            decimal result = amount;

            if (sourceCode.Id == targetCode.Id)
                return result;

            if (result != decimal.Zero && sourceCode.Id != targetCode.Id)
            {
                result = ConvertToPrimaryExchangeRateCurrency(result, sourceCode);
                result = ConvertFromPrimaryExchangeRateCurrency(result, targetCode);
            }

            return result;
        }

        /// <summary>
        /// Convert To Primary Exchange Rate Currency
        /// </summary>
        /// <param name="amount">Amount</param>
        /// <param name="sourceCode">Source Code</param>
        /// <returns>Decimal Item</returns>
        /// <exception cref="AppException"></exception>
        public virtual decimal ConvertToPrimaryExchangeRateCurrency(decimal amount, Currency sourceCode)
        {
            decimal result = amount;

            var primaryExchangeRateCurrency = GetCurrencyById(_currencySetting.PrimaryExchangeRateCurrencyId);

            if (result != decimal.Zero && sourceCode.Id != primaryExchangeRateCurrency.Id)
            {
                decimal exchangeRate = sourceCode.Rate;

                if (exchangeRate == decimal.Zero)
                    throw new AppException(string.Format("Exchange rate not found for currency [{0}]", sourceCode.Name));

                result = result / exchangeRate;
            }

            return result;
        }

        /// <summary>
        /// Convert From Primary Exchange Rate Currency
        /// </summary>
        /// <param name="amount">Amount</param>
        /// <param name="targetCode">Target Code</param>
        /// <returns>Decimal Item</returns>
        /// <exception cref="AppException"></exception>
        public virtual decimal ConvertFromPrimaryExchangeRateCurrency(decimal amount, Currency targetCode)
        {
            decimal result = amount;

            var primaryExchangeRateCurrency = GetCurrencyById(_currencySetting.PrimaryExchangeRateCurrencyId);

            if (result != decimal.Zero && targetCode.Id != primaryExchangeRateCurrency.Id)
            {
                decimal exchangeRate = targetCode.Rate;

                if (exchangeRate == decimal.Zero)
                    throw new AppException(string.Format("Exchange rate not found for currency [{0}]", targetCode.Name));

                result = result * exchangeRate;
            }

            return result;
        }

        #endregion Convert

        #endregion Methods
    }
}