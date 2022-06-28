// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="AffiliateChannelFilterConditionService.cs" company="Adrack.com">
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
using System.Collections.Generic;
using System.Linq;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Class AffiliateChannelFilterConditionService.
    /// Implements the <see cref="Adrack.Service.Lead.IAffiliateChannelFilterConditionService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Lead.IAffiliateChannelFilterConditionService" />
    public partial class AffiliateChannelFilterConditionService : IAffiliateChannelFilterConditionService
    {
        #region Constants

        /// <summary>
        /// Cache Profile By Id Key
        /// </summary>
        private const string CACHE_FILTER_BY_ID_KEY = "App.Cache.Filter.By.Id-{0}";

        /// <summary>
        /// The cache filter by affiliate channel identifier campaign template identifier key
        /// </summary>
        private const string CACHE_FILTER_BY_AFFILIATE_CHANNEL_ID_CAMPAIGN_TEMPLATE_ID_KEY = "App.Cache.Filter.By.AffiliateChannelId.CampaignTemplateId-{0}-{1}-{2}";

        private const string CACHE_FILTER_HAS_CHILDREN_KEY = "App.Cache.Filter.AffiliateChannel.HasChildren-{0}";

        /// <summary>
        /// Cache Profile All Key
        /// </summary>
        private const string CACHE_FILTER_ALL_KEY = "App.Cache.Filter.All";

        /// <summary>
        /// Cache Profile Pattern Key
        /// </summary>
        private const string CACHE_FILTER_PATTERN_KEY = "App.Cache.Filter.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// The filter condition repository
        /// </summary>
        private readonly IRepository<AffiliateChannelFilterCondition> _filterConditionRepository;

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
        /// Profile Service
        /// </summary>
        /// <param name="filterConditionRepository">The filter condition repository.</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        public AffiliateChannelFilterConditionService(IRepository<AffiliateChannelFilterCondition> filterConditionRepository, ICacheManager cacheManager, IAppEventPublisher appEventPublisher)
        {
            this._filterConditionRepository = filterConditionRepository;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Gets the filter conditions by affiliate channel identifier.
        /// </summary>
        /// <param name="AffiliateChannelId">The affiliate channel identifier.</param>
        /// <returns>IList&lt;AffiliateChannelFilterCondition&gt;.</returns>
        public virtual IList<AffiliateChannelFilterCondition> GetFilterConditionsByAffiliateChannelId(long AffiliateChannelId, long parentId = 0)
        {
            return (from x in _filterConditionRepository.Table
                    where x.AffiliateChannelId == AffiliateChannelId && (parentId == -1 || (parentId == 0 && !x.ParentId.HasValue) || x.ParentId == parentId)
                    select x).ToList();
        }

        /// <summary>
        /// Gets the filter conditions by affiliate channel identifier and campaign template identifier.
        /// </summary>
        /// <param name="AffiliateChannelId">The affiliate channel identifier.</param>
        /// <param name="campaignTemplateId">The campaign template identifier.</param>
        /// <returns>IList&lt;AffiliateChannelFilterCondition&gt;.</returns>
        public virtual IList<AffiliateChannelFilterCondition> GetFilterConditionsByAffiliateChannelIdAndCampaignTemplateId(long AffiliateChannelId, long campaignTemplateId, long parentId = 0)
        {
            string key = string.Format(CACHE_FILTER_BY_AFFILIATE_CHANNEL_ID_CAMPAIGN_TEMPLATE_ID_KEY, AffiliateChannelId, campaignTemplateId, parentId);

            return _cacheManager.Get(key, () =>
            {
                return (from x in _filterConditionRepository.Table
                        where x.AffiliateChannelId == AffiliateChannelId && x.CampaignTemplateId == campaignTemplateId && (parentId == -1 || (parentId == 0 && !x.ParentId.HasValue) || x.ParentId == parentId)
                        select x).ToList();
            });
        }

        public virtual bool HasChildren(long Id)
        {
            string key = string.Format(CACHE_FILTER_HAS_CHILDREN_KEY, Id);

            return _cacheManager.Get(key, () =>
            {
                if ((from x in _filterConditionRepository.Table
                        where x.ParentId == Id
                        select x).FirstOrDefault() == null) return false;
                return true;
            });
        }

        /// <summary>
        /// Get the filter condition.
        /// </summary>
        /// <param name="Id">long</param>
        /// <returns></returns>
        public virtual AffiliateChannelFilterCondition GetFilterConditionById(long Id)
        {
            return (from x in _filterConditionRepository.Table
                where x.Id == Id
                select x).FirstOrDefault();
        }

        /// <summary>
        /// Inserts the filter condition.
        /// </summary>
        /// <param name="filterCondition">The filter condition.</param>
        /// <exception cref="ArgumentNullException">filterCondition</exception>
        public virtual void InsertFilterCondition(AffiliateChannelFilterCondition filterCondition)
        {
            if (filterCondition == null)
                throw new ArgumentNullException("filterCondition");

            _filterConditionRepository.Insert(filterCondition);

            _cacheManager.RemoveByPattern(CACHE_FILTER_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(filterCondition);
        }

        /// <summary>
        /// Updates the filter condition.
        /// </summary>
        /// <param name="filterCondition">The filter condition.</param>
        /// <exception cref="ArgumentNullException">filterCondition</exception>
        public virtual void UpdateFilterCondition(AffiliateChannelFilterCondition filterCondition)
        {
            if (filterCondition == null)
                throw new ArgumentNullException("filterCondition");

            _filterConditionRepository.Update(filterCondition);

            _cacheManager.RemoveByPattern(CACHE_FILTER_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityUpdated(filterCondition);
        }

        /// <summary>
        /// Deletes the filter condition.
        /// </summary>
        /// <param name="filterCondition">The filter condition.</param>
        /// <exception cref="ArgumentNullException">filterCondition</exception>
        public virtual void DeleteFilterCondition(AffiliateChannelFilterCondition filterCondition)
        {
            if (filterCondition == null)
                throw new ArgumentNullException("filterCondition");

            _filterConditionRepository.Delete(filterCondition);

            _cacheManager.RemoveByPattern(CACHE_FILTER_PATTERN_KEY);

            _appEventPublisher.EntityDeleted(filterCondition);
        }

        /// <summary>
        /// Deletes the filter conditions.
        /// </summary>
        /// <param name="AffiliateChannelId">The affiliate channel identifier.</param>
        public virtual void DeleteFilterConditions(long AffiliateChannelId)
        {
            var query = (from x in _filterConditionRepository.Table
                         where x.AffiliateChannelId == AffiliateChannelId
                         select x).ToList();

            foreach (AffiliateChannelFilterCondition fv in query)
            {
                DeleteFilterCondition(fv);
            }
        }

        #endregion Methods
    }
}