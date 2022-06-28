// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="StateProvinceService.cs" company="Adrack.com">
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
    /// Represents a State Province Service
    /// Implements the <see cref="Adrack.Service.Directory.IStateProvinceService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Directory.IStateProvinceService" />
    public partial class StateProvinceService : IStateProvinceService
    {
        #region Constants

        /// <summary>
        /// Cache State Province By Id Key
        /// </summary>
        private const string CACHE_STATEPROVINCE_BY_ID_KEY = "App.Cache.StateProvince.By.Id-{0}";

        /// <summary>
        /// Cache State Province By Country Id Key
        /// </summary>
        private const string CACHE_STATEPROVINCE_BY_COUNTRY_ID_KEY = "App.Cache.StateProvince.By.Country.Id-{0}";

        /// <summary>
        /// Cache State Province All Key
        /// </summary>
        private const string CACHE_STATEPROVINCE_ALL_KEY = "App.Cache.StateProvince.All";

        /// <summary>
        /// Cache State Province Pattern Key
        /// </summary>
        private const string CACHE_STATEPROVINCE_PATTERN_KEY = "App.Cache.StateProvince.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// State Province
        /// </summary>
        private readonly IRepository<StateProvince> _stateProvinceRepository;

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
        /// State Province Service
        /// </summary>
        /// <param name="stateProvinceRepository">State Province Repository</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        public StateProvinceService(IRepository<StateProvince> stateProvinceRepository, ICacheManager cacheManager, IAppEventPublisher appEventPublisher)
        {
            this._stateProvinceRepository = stateProvinceRepository;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get State Province By Id
        /// </summary>
        /// <param name="stateProvinceId">State Province Identifier</param>
        /// <returns>State Province Item</returns>
        public virtual StateProvince GetStateProvinceById(long stateProvinceId)
        {
            if (stateProvinceId == 0)
                return null;

            string key = string.Format(CACHE_STATEPROVINCE_BY_ID_KEY, stateProvinceId);

            return _cacheManager.Get(key, () => { return _stateProvinceRepository.GetById(stateProvinceId); });
        }

        /// <summary>
        /// Get State Province By Country Id
        /// </summary>
        /// <param name="countryId">Country Identifier</param>
        /// <returns>State Province Collection Item</returns>
        public virtual IList<StateProvince> GetStateProvinceByCountryId(long countryId)
        {
            string key = string.Format(CACHE_STATEPROVINCE_BY_COUNTRY_ID_KEY, countryId);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _stateProvinceRepository.Table
                            where x.CountryId == countryId
                            orderby x.DisplayOrder, x.Name
                            select x;

                var stateProvinces = query.ToList();

                return stateProvinces;
            });
        }

        /// <summary>
        /// Get All State Provinces
        /// </summary>
        /// <returns>State Province Collection Item</returns>
        public virtual IList<StateProvince> GetAllStateProvinces()
        {
            string key = CACHE_STATEPROVINCE_ALL_KEY;

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _stateProvinceRepository.Table
                            orderby x.DisplayOrder, x.Id
                            select x;

                var stateProvinces = query.ToList();

                return stateProvinces;
            });
        }

        /// <summary>
        /// Insert State Province
        /// </summary>
        /// <param name="stateProvince">State Province</param>
        /// <exception cref="ArgumentNullException">stateProvince</exception>
        public virtual void InsertStateProvince(StateProvince stateProvince)
        {
            if (stateProvince == null)
                throw new ArgumentNullException("stateProvince");

            _stateProvinceRepository.Insert(stateProvince);

            _cacheManager.RemoveByPattern(CACHE_STATEPROVINCE_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(stateProvince);
        }

        /// <summary>
        /// Update State Province
        /// </summary>
        /// <param name="stateProvince">State Province</param>
        /// <exception cref="ArgumentNullException">stateProvince</exception>
        public virtual void UpdateStateProvince(StateProvince stateProvince)
        {
            if (stateProvince == null)
                throw new ArgumentNullException("stateProvince");

            _stateProvinceRepository.Update(stateProvince);

            _cacheManager.RemoveByPattern(CACHE_STATEPROVINCE_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityUpdated(stateProvince);
        }

        /// <summary>
        /// Delete State Province
        /// </summary>
        /// <param name="stateProvince">State Province</param>
        /// <exception cref="ArgumentNullException">stateProvince</exception>
        public virtual void DeleteStateProvince(StateProvince stateProvince)
        {
            if (stateProvince == null)
                throw new ArgumentNullException("stateProvince");

            _stateProvinceRepository.Delete(stateProvince);

            _cacheManager.RemoveByPattern(CACHE_STATEPROVINCE_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(stateProvince);
        }

        #endregion Methods
    }
}