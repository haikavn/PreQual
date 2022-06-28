// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="AffiliateResponseService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Cache;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using Adrack.Service.Infrastructure.ApplicationEvent;
using Adrack.Service.Membership;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Represents a Profile Service
    /// Implements the <see cref="Adrack.Service.Lead.IAffiliateResponseService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Lead.IAffiliateResponseService" />
    public partial class AffiliateResponseService : IAffiliateResponseService
    {
        #region Constants

        /// <summary>
        /// Cache Profile By Id Key
        /// </summary>
        private const string CACHE_PROFILE_BY_ID_KEY = "App.Cache.AffiliateResponse.By.Id-{0}";

        /// <summary>
        /// Cache Profile All Key
        /// </summary>
        private const string CACHE_PROFILE_ALL_KEY = "App.Cache.AffiliateResponse.All";

        /// <summary>
        /// Cache Profile Pattern Key
        /// </summary>
        private const string CACHE_PROFILE_PATTERN_KEY = "App.Cache.AffiliateResponse.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Profile
        /// </summary>
        private readonly IRepository<AffiliateResponse> _affiliateResponseRepository;

        /// <summary>
        /// Cache Manager
        /// </summary>
        private readonly ICacheManager _cacheManager;

        /// <summary>
        /// Application Event Publisher
        /// </summary>
        private readonly IAppEventPublisher _appEventPublisher;

        /// <summary>
        /// The user service
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// The data provider
        /// </summary>
        private readonly IDataProvider _dataProvider;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Profile Service
        /// </summary>
        /// <param name="affiliateResponseRepository">The affiliate response repository.</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="userService">The user service.</param>
        /// <param name="dataProvider">The data provider.</param>
        public AffiliateResponseService(IRepository<AffiliateResponse> affiliateResponseRepository, ICacheManager cacheManager, IAppEventPublisher appEventPublisher, IUserService userService, IDataProvider dataProvider)
        {
            this._affiliateResponseRepository = affiliateResponseRepository;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;

            this._dataProvider = dataProvider;

            this._userService = userService;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get Profile By Id
        /// </summary>
        /// <param name="affiliateId">The affiliate identifier.</param>
        /// <returns>Profile Item</returns>
        public virtual AffiliateResponse GetAffiliateResponseById(long affiliateId)
        {
            if (affiliateId == 0)
                return null;

            string key = string.Format(CACHE_PROFILE_BY_ID_KEY, affiliateId);

            return _cacheManager.Get(key, () => { return _affiliateResponseRepository.GetById(affiliateId); });
        }

        /// <summary>
        /// Get All Profiles
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        public virtual IList<AffiliateResponse> GetAllAffiliateResponses()
        {
            string key = CACHE_PROFILE_ALL_KEY;

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _affiliateResponseRepository.Table
                            orderby x.Id descending
                            select x;

                var profiles = query.ToList();

                return profiles;
            });
        }

        /// <summary>
        /// Gets the affiliate responses by affiliate channel identifier.
        /// </summary>
        /// <param name="affiliateChannelId">The affiliate channel identifier.</param>
        /// <returns>IList&lt;AffiliateResponse&gt;.</returns>
        public virtual IList<AffiliateResponse> GetAffiliateResponsesByAffiliateChannelId(long affiliateChannelId)
        {
                var query = from x in _affiliateResponseRepository.Table
                            where x.AffiliateChannelId == affiliateChannelId
                            orderby x.Id descending
                            select x;

                var profiles = query.ToList();

                return profiles;            
        }

        /// <summary>
        /// Gets the affiliate responses by filters.
        /// </summary>
        /// <param name="AffiliateId">The affiliate identifier.</param>
        /// <param name="AffiliateChannelId">The affiliate channel identifier.</param>
        /// <param name="DateFrom">The date from.</param>
        /// <param name="DateTo">The date to.</param>
        /// <returns>IList&lt;AffiliateResponse&gt;.</returns>
        public virtual IList<AffiliateResponse> GetAffiliateResponsesByFilters(long AffiliateId, long AffiliateChannelId, DateTime DateFrom, DateTime DateTo)
        {
                var query = from x in _affiliateResponseRepository.Table
                            where x.LeadId == null && (AffiliateId == 0 || x.AffiliateId == AffiliateId) && (AffiliateChannelId == 0 || x.AffiliateChannelId == AffiliateChannelId) && (x.Created >= DateFrom && x.Created <= DateTo)
                            orderby x.Id descending
                            select x;

                var profiles = query.ToList();

                return profiles;            
        }

        /// <summary>
        /// Gets the affiliate responses by lead identifier.
        /// </summary>
        /// <param name="leadId">The lead identifier.</param>
        /// <returns>IList&lt;AffiliateResponse&gt;.</returns>
        public virtual IList<AffiliateResponse> GetAffiliateResponsesByLeadId(long leadId)
        {
            var leadIdParam = _dataProvider.GetParameter();
            leadIdParam.ParameterName = "leadId";
            leadIdParam.Value = leadId;
            leadIdParam.DbType = DbType.Int64;

            return _affiliateResponseRepository.GetDbClientContext().SqlQuery<AffiliateResponse>("EXECUTE [dbo].[GetAffiliateResponsesByLeadId] @leadId", leadIdParam).ToList();
        }


        
        public virtual void UpdateDatabase()
        {
            _affiliateResponseRepository.Truncate();
            _affiliateResponseRepository.SaveChanges();
        }

        public virtual void ResetDatabase()
        {
            _affiliateResponseRepository.Reset();
        }

      
        string LimitString(string s, int len)
        {
            if (s == null) return null;
            if (s.Length > len)
                return s.Substring(0, len);
            return s;
        }
        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="affiliateResponse">The affiliate response.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">affiliateResponse</exception>
        public virtual long InsertAffiliateResponse(AffiliateResponse affiliateResponse)
        {
            if (affiliateResponse == null)
                throw new ArgumentNullException("affiliateResponse");

            affiliateResponse.Message = LimitString(affiliateResponse.Message,990);
            affiliateResponse.ReceivedData = LimitString(affiliateResponse.ReceivedData, 5000);
            affiliateResponse.Response = LimitString(affiliateResponse.Response, 5000);
            affiliateResponse.State = LimitString(affiliateResponse.State, 49);

            _affiliateResponseRepository.InsertNoUpdate(affiliateResponse);

            _cacheManager.RemoveByPattern(CACHE_PROFILE_PATTERN_KEY);

            _appEventPublisher.EntityInserted(affiliateResponse);
            //_cacheManager.ClearRemoteServersCache();
            return affiliateResponse.Id;
        }

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="affiliateResponse">The affiliate response.</param>
        /// <exception cref="ArgumentNullException">affiliateResponse</exception>
        public virtual void UpdateAffiliateResponse(AffiliateResponse affiliateResponse)
        {
            if (affiliateResponse == null)
                throw new ArgumentNullException("affiliateResponse");

            _affiliateResponseRepository.Update(affiliateResponse);

            _cacheManager.RemoveByPattern(CACHE_PROFILE_PATTERN_KEY);
            //_cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityUpdated(affiliateResponse);
        }

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="affiliateResponse">The affiliate response.</param>
        /// <exception cref="ArgumentNullException">affiliateResponse</exception>
        public virtual void DeleteAffiliateResponse(AffiliateResponse affiliateResponse)
        {
            if (affiliateResponse == null)
                throw new ArgumentNullException("affiliateResponse");

            _affiliateResponseRepository.Delete(affiliateResponse);

            _cacheManager.RemoveByPattern(CACHE_PROFILE_PATTERN_KEY);
            //_cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(affiliateResponse);
        }


        public virtual AffiliateResponse CheckAffiliateResponse(long leadId, decimal minPrice)
        {
            var leadIdParam = _dataProvider.GetParameter();
            leadIdParam.ParameterName = "leadId";
            leadIdParam.Value = leadId;
            leadIdParam.DbType = DbType.Int64;

            var minPriceParam = _dataProvider.GetParameter();
            minPriceParam.ParameterName = "minPrice";
            minPriceParam.Value = minPrice;
            minPriceParam.DbType = DbType.Decimal;

            return _affiliateResponseRepository.GetDbClientContext().SqlQuery<AffiliateResponse>("EXECUTE [dbo].[CheckAffiliateResponse] @leadId,@minPrice", leadIdParam, minPriceParam).FirstOrDefault();
        }

        #endregion Methods
    }
}