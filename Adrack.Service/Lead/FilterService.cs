// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="FilterService.cs" company="Adrack.com">
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
    /// Class FilterService.
    /// Implements the <see cref="Adrack.Service.Lead.IFilterService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Lead.IFilterService" />
    public partial class FilterService : IFilterService
    {
        #region Constants

        /// <summary>
        /// Cache Profile By Id Key
        /// </summary>
        private const string CACHE_FILTER_BY_ID_KEY = "App.Cache.Filter.By.Id-{0}";

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
        /// Profile
        /// </summary>
        private readonly IRepository<Filter> _filterRepository;

        /// <summary>
        /// The filter condition repository
        /// </summary>
        private readonly IRepository<FilterCondition> _filterConditionRepository;

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
        /// <param name="filterRepository">The filter repository.</param>
        /// <param name="filterConditionRepository">The filter condition repository.</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        public FilterService(IRepository<Filter> filterRepository, IRepository<FilterCondition> filterConditionRepository, ICacheManager cacheManager, IAppEventPublisher appEventPublisher)
        {
            this._filterRepository = filterRepository;
            this._filterConditionRepository = filterConditionRepository;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get Profile By Id
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>Profile Item</returns>
        public virtual Filter GetFilterById(long Id)
        {
            if (Id == 0)
                return null;

            return _filterRepository.GetById(Id);
        }

        /// <summary>
        /// Get filter condition By Id
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>Filtet condition Item</returns>
        public FilterCondition GetFilterConditionById(long Id)
        {
            if (Id == 0)
                return null;

            return _filterConditionRepository.GetById(Id);
        }

        /// <summary>
        /// Get All Profiles
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        public virtual IList<Filter> GetAllFilters()
        {
            string key = CACHE_FILTER_ALL_KEY;

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _filterRepository.Table
                            orderby x.Id descending
                            select x;

                var profiles = query.ToList();

                return profiles;
            });
        }

        /// <summary>
        /// Gets the filters by campaign identifier.
        /// </summary>
        /// <param name="campaignId">The campaign identifier.</param>
        /// <returns>IList&lt;Filter&gt;.</returns>
        public IList<Filter> GetFiltersByCampaignId(long campaignId)
        {
            var query = from x in _filterRepository.Table
                        where x.CampaignId == campaignId
                        select x;

            var profiles = query.ToList();

            return profiles;
        }

        /// <summary>
        /// Gets the filter conditions by filter identifier.
        /// </summary>
        /// <param name="filterId">The filter identifier.</param>
        /// <returns>IList&lt;FilterCondition&gt;.</returns>
        public virtual IList<FilterCondition> GetFilterConditionsByFilterId(long filterId, long parentId = 0)
        {
            return (from x in _filterConditionRepository.Table
                    where x.FilterId == filterId && ((parentId == 0 && !x.ParentId.HasValue) || x.ParentId == parentId)
                    select x).ToList();
        }

        public virtual IList<FilterCondition> GetFilterConditionsByCampaignFieldId(long filterId, long campaignFieldId)
        {
            return (from x in _filterConditionRepository.Table
                    where x.FilterId == filterId && x.CampaignTemplateId == campaignFieldId
                    select x).ToList();
        }


        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">filter</exception>
        public virtual long InsertFilter(Filter filter)
        {
            if (filter == null)
                throw new ArgumentNullException("filter");

            _filterRepository.Insert(filter);

            _cacheManager.RemoveByPattern(CACHE_FILTER_PATTERN_KEY);
            //_cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(filter);

            return filter.Id;
        }

        /// <summary>
        /// Inserts the filter condition.
        /// </summary>
        /// <param name="filterCondition">The filter condition.</param>
        /// <exception cref="ArgumentNullException">filterCondition</exception>
        public virtual void InsertFilterCondition(FilterCondition filterCondition)
        {
            if (filterCondition == null)
                throw new ArgumentNullException("filterCondition");

            _filterConditionRepository.Insert(filterCondition);
            //_cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(filterCondition);
        }

        /// <summary>
        /// Update filter set
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <exception cref="ArgumentNullException">filter</exception>
        public virtual void UpdateFilter(Filter filter)
        {
            if (filter == null)
                throw new ArgumentNullException("filter");

            _filterRepository.Update(filter);

            _cacheManager.RemoveByPattern(CACHE_FILTER_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityUpdated(filter);
        }

        /// <summary>
        /// Update filter set condition
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <exception cref="ArgumentNullException">filter</exception>
        public virtual void UpdateFilterCondition(FilterCondition filterCondition)
        {
            if (filterCondition == null)
                throw new ArgumentNullException("filterCondition");

            _filterConditionRepository.Update(filterCondition);

            _cacheManager.RemoveByPattern(CACHE_FILTER_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityUpdated(filterCondition);
        }

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <exception cref="ArgumentNullException">filter</exception>
        public virtual void DeleteFilter(Filter filter)
        {
            if (filter == null)
                throw new ArgumentNullException("filter");

            _filterRepository.Delete(filter);

            _cacheManager.RemoveByPattern(CACHE_FILTER_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(filter);
        }

        /// <summary>
        /// Deletes the filter condition.
        /// </summary>
        /// <param name="filterCondition">The filter condition.</param>
        /// <exception cref="ArgumentNullException">filterCondition</exception>
        public virtual void DeleteFilterCondition(FilterCondition filterCondition)
        {
            if (filterCondition == null)
                throw new ArgumentNullException("filterCondition");

            _filterConditionRepository.Delete(filterCondition);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(filterCondition);
        }

        /// <summary>
        /// Deletes the filter conditions.
        /// </summary>
        /// <param name="filterId">The filter identifier.</param>
        public virtual void DeleteFilterConditions(long filterId)
        {
            var query = (from x in _filterConditionRepository.Table
                         where x.FilterId == filterId
                         select x).ToList();

            foreach (FilterCondition fv in query)
            {
                DeleteFilterCondition(fv);
            }
        }

        #endregion Methods
    }
}