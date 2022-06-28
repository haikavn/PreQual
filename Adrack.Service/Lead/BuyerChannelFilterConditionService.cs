// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="BuyerChannelFilterConditionService.cs" company="Adrack.com">
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
    /// Class BuyerChannelFilterConditionService.
    /// Implements the <see cref="Adrack.Service.Lead.IBuyerChannelFilterConditionService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Lead.IBuyerChannelFilterConditionService" />
    public partial class BuyerChannelFilterConditionService : IBuyerChannelFilterConditionService
    {
        #region Constants

        /// <summary>
        /// Cache Profile By Id Key
        /// </summary>
        private const string CACHE_FILTER_BY_ID_KEY = "App.Cache.Filter.BuyerChannelFilter.By.Id-{0}";

        /// <summary>
        /// The cache filter by buyer channel identifier and campaign template identifier key
        /// </summary>
        private const string CACHE_FILTER_BY_BUYER_CHANNEL_ID_AND_CAMPAIGN_TEMPLATE_ID_KEY = "App.Cache.Filter.BuyerChannelFilter.By.ButyerChannelId.CampaignTemplateId-{0}-{1}-{2}";

        private const string CACHE_FILTER_HAS_CHILDREN_KEY = "App.Cache.BuyerChannelFilter.HasChildren-{0}";

        /// <summary>
        /// Cache Profile All Key
        /// </summary>
        private const string CACHE_FILTER_ALL_KEY = "App.Cache.Filter.BuyerChannelFilter.All";

        /// <summary>
        /// Cache Profile Pattern Key
        /// </summary>
        private const string CACHE_FILTER_PATTERN_KEY = "App.Cache.Filter.BuyerChannelFilter.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// The filter condition repository
        /// </summary>
        private readonly IRepository<BuyerChannelFilterCondition> _filterConditionRepository;

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
        public BuyerChannelFilterConditionService(IRepository<BuyerChannelFilterCondition> filterConditionRepository, ICacheManager cacheManager, IAppEventPublisher appEventPublisher)
        {
            this._filterConditionRepository = filterConditionRepository;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
        }

        #endregion Constructor

        #region Methods

        public virtual BuyerChannelFilterCondition GetFilterConditionById(long Id)
        {
            return (from x in _filterConditionRepository.Table
                    where x.Id == Id
                    select x).FirstOrDefault();
        }

        /// <summary>
        /// Gets the filter conditions by buyer channel identifier.
        /// </summary>
        /// <param name="BuyerChannelId">The buyer channel identifier.</param>
        /// <returns>IList&lt;BuyerChannelFilterCondition&gt;.</returns>
        public virtual IList<BuyerChannelFilterCondition> GetFilterConditionsByBuyerChannelId(long buyerChannelId, long parentId = 0)
        {
            return (from x in _filterConditionRepository.Table
                    where x.BuyerChannelId == buyerChannelId && ((parentId == 0 && !x.ParentId.HasValue) || x.ParentId == parentId || parentId == -1)
                    select x).ToList();
        }

        /// <summary>
        /// Gets the filter conditions by buyer channel identifier and campaign template identifier.
        /// </summary>
        /// <param name="BuyerChannelId">The buyer channel identifier.</param>
        /// <param name="campaignTemplateId">The campaign template identifier.</param>
        /// <returns>IList&lt;BuyerChannelFilterCondition&gt;.</returns>
        public virtual IList<BuyerChannelFilterCondition> GetFilterConditionsByBuyerChannelIdAndCampaignTemplateId(long buyerChannelId, long campaignTemplateId, long parentId = 0)
        {
            string key = string.Format(CACHE_FILTER_BY_BUYER_CHANNEL_ID_AND_CAMPAIGN_TEMPLATE_ID_KEY, buyerChannelId, campaignTemplateId, parentId);

            return _cacheManager.Get(key, () =>
            {
                return (from x in _filterConditionRepository.Table
                        where x.BuyerChannelId == buyerChannelId && x.CampaignTemplateId == campaignTemplateId && (parentId == -1 || (parentId == 0 && !x.ParentId.HasValue) || x.ParentId == parentId)
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
        /// Inserts the filter condition.
        /// </summary>
        /// <param name="filterCondition">The filter condition.</param>
        /// <exception cref="ArgumentNullException">filterCondition</exception>
        public virtual long InsertFilterCondition(BuyerChannelFilterCondition filterCondition)
        {
            if (filterCondition == null)
                throw new ArgumentNullException("filterCondition");

            _filterConditionRepository.Insert(filterCondition);

            _cacheManager.RemoveByPattern(CACHE_FILTER_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(filterCondition);

            return filterCondition.Id;
        }

        /// <summary>
        /// Updates the filter condition.
        /// </summary>
        /// <param name="filterCondition">The filter condition.</param>
        /// <exception cref="ArgumentNullException">filterCondition</exception>
        public virtual void UpdateFilterCondition(BuyerChannelFilterCondition filterCondition)
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
        public virtual void DeleteFilterCondition(BuyerChannelFilterCondition filterCondition)
        {
            if (filterCondition == null)
                throw new ArgumentNullException("filterCondition");

            _filterConditionRepository.Delete(filterCondition);

            _cacheManager.RemoveByPattern(CACHE_FILTER_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(filterCondition);
        }

        /// <summary>
        /// Deletes the filter conditions.
        /// </summary>
        /// <param name="BuyerChannelId">The buyer channel identifier.</param>
        public virtual void DeleteFilterConditions(long buyerChannelId)
        {
            var query = (from x in _filterConditionRepository.Table
                         where x.BuyerChannelId == buyerChannelId
                         select x).ToList();

            foreach (BuyerChannelFilterCondition fv in query)
            {
                DeleteFilterCondition(fv);
            }
        }

        #endregion Methods
    }
}