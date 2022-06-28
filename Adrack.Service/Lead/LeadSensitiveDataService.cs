// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="LeadSensitiveDataService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Cache;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using Adrack.Service.Infrastructure.ApplicationEvent;
using System;
using System.Data;
using System.Linq;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Represents a Lead Service
    /// Implements the <see cref="Adrack.Service.Lead.ILeadSensitiveDataService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Lead.ILeadSensitiveDataService" />
    public partial class LeadSensitiveDataService : ILeadSensitiveDataService
    {
        #region Constants

        /// <summary>
        /// Cache Profile By Id Key
        /// </summary>
        private const string CACHE_LEAD_SENSITIVE_DATA_BY_ID_KEY = "App.Cache.LeadSensitiveData.By.Id-{0}";

        /// <summary>
        /// Cache Profile All Key
        /// </summary>
        private const string CACHE_LEAD_SENSITIVE_DATA_ALL_KEY = "App.Cache.LeadSensitiveData.All";

        
        /// <summary>
        /// Cache Profile Pattern Key
        /// </summary>
        private const string CACHE_LEAD_SENSITIVE_DATA_PATTERN_KEY = "App.Cache.LeadSensitiveData.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Profile
        /// </summary>
        private readonly IRepository<LeadSensitiveData> _leadSensitiveDataRepository;

        /// <summary>
        /// The lead geo data repository
        /// </summary>
        private readonly IRepository<LeadGeoData> _leadGeoDataRepository;

        /// <summary>
        /// Cache Manager
        /// </summary>
        private readonly ICacheManager _cacheManager;

        /// <summary>
        /// Application Event Publisher
        /// </summary>
        private readonly IAppEventPublisher _appEventPublisher;

        /// <summary>
        /// The data provider
        /// </summary>
        private readonly IDataProvider _dataProvider;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Profile Service
        /// </summary>
        /// <param name="leadSensitiveDataRepository">The lead sensitive data repository.</param>
        /// <param name="leadGeoDataRepository">The lead geo data repository.</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        /// <param name="dataProvider">The data provider.</param>
        /// <param name="dbContext">The database context.</param>
        public LeadSensitiveDataService(IRepository<LeadSensitiveData> leadSensitiveDataRepository, IRepository<LeadGeoData> leadGeoDataRepository, ICacheManager cacheManager, IAppEventPublisher appEventPublisher, IDataProvider dataProvider)
        {
            this._leadSensitiveDataRepository = leadSensitiveDataRepository;
            this._leadGeoDataRepository = leadGeoDataRepository;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
            this._dataProvider = dataProvider;
        }

        #endregion Constructor

        #region Methods

        // <summary>
        /// <summary>
        /// Gets the lead sensitive data by identifier.
        /// </summary>
        /// <param name="leadId">The lead identifier.</param>
        /// <returns>LeadSensitiveData.</returns>
        public virtual LeadSensitiveData GetLeadSensitiveDataById(long leadId)
        {
            if (leadId == 0)
                return null;

            string key = string.Format(CACHE_LEAD_SENSITIVE_DATA_BY_ID_KEY, leadId);

            return _cacheManager.Get(key, () => { return _leadSensitiveDataRepository.GetById(leadId); });
        }

        /// <summary>
        /// Gets the lead sensitive data by lead identifier.
        /// </summary>
        /// <param name="leadId">The lead identifier.</param>
        /// <returns>LeadSensitiveData.</returns>
        public virtual LeadSensitiveData GetLeadSensitiveDataByLeadId(long leadId)
        {
            if (leadId == 0)
                return null;

            var query = from x in _leadSensitiveDataRepository.Table
                        where x.LeadId == leadId
                        select x;

            var profiles = query.FirstOrDefault();

            return profiles;
        }

      
        /// <summary>
        /// Inserts the lead sensitive data.
        /// </summary>
        /// <param name="leadSensitiveData">The lead sensitive data.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">leadSensitiveData</exception>
        public virtual long InsertLeadSensitiveData(LeadSensitiveData leadSensitiveData)
        {
            if (leadSensitiveData == null)
                throw new ArgumentNullException("leadSensitiveData");

            _leadSensitiveDataRepository.Insert(leadSensitiveData);

            _cacheManager.RemoveByPattern(CACHE_LEAD_SENSITIVE_DATA_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(leadSensitiveData);

            return leadSensitiveData.Id;
        }

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="leadSensitiveData">The lead sensitive data.</param>
        /// <exception cref="ArgumentNullException">leadMain</exception>
        public virtual void UpdateLeadSensitiveData(LeadSensitiveData leadSensitiveData)
        {
            if (leadSensitiveData == null)
                throw new ArgumentNullException("leadMain");

            _leadSensitiveDataRepository.Update(leadSensitiveData);

            _cacheManager.RemoveByPattern(CACHE_LEAD_SENSITIVE_DATA_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityUpdated(leadSensitiveData);
        }

        /// <summary>
        /// Delete LeadSensitiveData
        /// </summary>
        /// <param name="leadSensitiveData">The lead sensitive data.</param>
        /// <exception cref="ArgumentNullException">leadSensitiveData</exception>
        public virtual void DeleteLeadSensitiveData(LeadSensitiveData leadSensitiveData)
        {
            if (leadSensitiveData == null)
                throw new ArgumentNullException("leadSensitiveData");

            _leadSensitiveDataRepository.Delete(leadSensitiveData);

            _cacheManager.RemoveByPattern(CACHE_LEAD_SENSITIVE_DATA_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(leadSensitiveData);
        }

        #endregion Methods
    }
}