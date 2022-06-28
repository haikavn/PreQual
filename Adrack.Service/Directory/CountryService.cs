// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="CountryService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Cache;
using Adrack.Core.Domain.Directory;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using Adrack.Service.Infrastructure.ApplicationEvent;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Adrack.Service.Directory
{
    /// <summary>
    /// Represents a Country Service
    /// Implements the <see cref="Adrack.Service.Directory.ICountryService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Directory.ICountryService" />
    public partial class CountryService : ICountryService
    {
        #region Constants

        /// <summary>
        /// Cache Country By Id Key
        /// </summary>
        private const string CACHE_COUNTRY_BY_ID_KEY = "App.Cache.Country.By.Id-{0}";

        /// <summary>
        /// Cache Country All Key
        /// </summary>
        private const string CACHE_COUNTRY_ALL_KEY = "App.Cache.Country.All-{0}";

        /// <summary>
        /// Cache Country Pattern Key
        /// </summary>
        private const string CACHE_COUNTRY_PATTERN_KEY = "App.Cache.Country.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Country
        /// </summary>
        private readonly IRepository<Country> _countryRepository;

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
        /// Country Service
        /// </summary>
        /// <param name="countryRepository">Country Repository</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        public CountryService(IRepository<Country> countryRepository, ICacheManager cacheManager, IAppEventPublisher appEventPublisher)
        {
            this._countryRepository = countryRepository;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get Country By Id
        /// </summary>
        /// <param name="countryId">Country Identifier</param>
        /// <returns>Country Item</returns>
        public virtual Country GetCountryById(long countryId)
        {
            if (countryId == 0)
                return null;

            string key = string.Format(CACHE_COUNTRY_BY_ID_KEY, countryId);

            return _cacheManager.Get(key, () => { return _countryRepository.GetById(countryId); });
        }

        /// <summary>
        /// Get All Countries
        /// </summary>
        /// <returns>Country Collection Item</returns>
        public virtual IList<Country> GetAllCountries()
        {
            string key = CACHE_COUNTRY_ALL_KEY;

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _countryRepository.Table
                            orderby x.DisplayOrder descending, x.Name
                            select x;

                var country = query.ToList();

                return country;
            });
        }

        /// <summary>
        /// Insert Country
        /// </summary>
        /// <param name="country">Country</param>
        /// <exception cref="ArgumentNullException">country</exception>
        public virtual void InsertCountry(Country country)
        {
            if (country == null)
                throw new ArgumentNullException("country");

            _countryRepository.Insert(country);

            _cacheManager.RemoveByPattern(CACHE_COUNTRY_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(country);
        }

        /// <summary>
        /// Update Country
        /// </summary>
        /// <param name="country">Country</param>
        /// <exception cref="ArgumentNullException">country</exception>
        public virtual void UpdateCountry(Country country)
        {
            if (country == null)
                throw new ArgumentNullException("country");

            _countryRepository.Update(country);

            _cacheManager.RemoveByPattern(CACHE_COUNTRY_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityUpdated(country);
        }

        /// <summary>
        /// Delete Country
        /// </summary>
        /// <param name="country">Country</param>
        /// <exception cref="ArgumentNullException">country</exception>
        public virtual void DeleteCountry(Country country)
        {
            if (country == null)
                throw new ArgumentNullException("country");

            _countryRepository.Delete(country);

            _cacheManager.RemoveByPattern(CACHE_COUNTRY_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(country);
        }

        #endregion Methods
    }
}