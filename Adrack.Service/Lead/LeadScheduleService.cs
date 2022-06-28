// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="BuyerChannelScheduleService.cs" company="Adrack.com">
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
    /// Class BuyerChannelScheduleService.
    /// Implements the <see cref="IBuyerChannelScheduleService" />
    /// </summary>
    /// <seealso cref="IBuyerChannelScheduleService" />
    public partial class BuyerChannelScheduleService : IBuyerChannelScheduleService
    {
        #region Constants

        /// <summary>
        /// Cache Profile By Id Key
        /// </summary>
        private const string CACHE_LEAD_SCHEDULE_BY_ID_KEY = "App.Cache.BuyerChannelSchedule.By.Id-{0}";

        /// <summary>
        /// Cache Profile All Key
        /// </summary>
        private const string CACHE_LEAD_SCHEDULE_ALL_KEY = "App.Cache.BuyerChannelSchedule.All";

        /// <summary>
        /// Cache Channel Schedule Key
        /// </summary>
        private const string CACHE_BUYER_CHANNEL_SCHEDULE_KEY = "App.Cache.BuyerChannelSchedule.All-{0}-{1}";

        /// <summary>
        /// Cache Profile Pattern Key
        /// </summary>
        private const string CACHE_BUYER_CHANNEL_SCHEDULE_PATTERN_KEY = "App.Cache.BuyerChannelSchedule.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Profile
        /// </summary>
        private readonly IRepository<BuyerChannelSchedule> _buyerChannelScheduleRepository;

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
        /// <param name="buyerChannelScheduleRepository">The buyer channel schedule repository.</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        public BuyerChannelScheduleService(IRepository<BuyerChannelSchedule> buyerChannelScheduleRepository, ICacheManager cacheManager, IAppEventPublisher appEventPublisher)
        {
            this._buyerChannelScheduleRepository = buyerChannelScheduleRepository;
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
        public virtual BuyerChannelSchedule GetBuyerChannelScheduleById(long Id)
        {
            if (Id == 0)
                return null;

            string key = string.Format(CACHE_LEAD_SCHEDULE_BY_ID_KEY, Id);

            return _cacheManager.Get(key, () => { return _buyerChannelScheduleRepository.GetById(Id); });
        }

        /// <summary>
        /// Get All Profiles
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        public virtual IList<BuyerChannelSchedule> GetAllBuyerChannelSchedules()
        {
            string key = CACHE_LEAD_SCHEDULE_ALL_KEY;

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _buyerChannelScheduleRepository.Table
                            orderby x.Id descending
                            select x;

                var profiles = query.ToList();

                return profiles;
            });
        }

        /// <summary>
        /// Gets the lead schedules by buyer channel identifier.
        /// </summary>
        /// <param name="BuyerChannelId">The buyer channel identifier.</param>
        /// <param name="cache"></param>
        /// <returns>IList&lt;BuyerChannelSchedule&gt;.</returns>
        public virtual IList<BuyerChannelSchedule> GetBuyerChannelsByBuyerChannelId(long buyerChannelId, bool cache = true, bool allQuantities = false)
        {
            if (!cache)
            {
                var query1 = from x in _buyerChannelScheduleRepository.Table
                    where x.BuyerChannelId == buyerChannelId && ((!allQuantities && x.Quantity >= 0) || (allQuantities))
                    orderby x.Id
                    select x;

                return query1.ToList();
            }

            string key = String.Format(CACHE_BUYER_CHANNEL_SCHEDULE_KEY, buyerChannelId, allQuantities);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _buyerChannelScheduleRepository.Table
                            where x.BuyerChannelId == buyerChannelId && ((!allQuantities && x.Quantity >= 0) || (allQuantities))
                            orderby x.Id
                            select x;

                var profiles = query.ToList();

                return profiles;
            });
        }

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="buyerChannelSchedule">The buyer channel schedule.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">buyerChannelSchedule</exception>
        public virtual long InsertBuyerChannelSchedule(BuyerChannelSchedule buyerChannelSchedule)
        {
            if (buyerChannelSchedule == null)
                throw new ArgumentNullException("buyerChannelSchedule");

            _buyerChannelScheduleRepository.Insert(buyerChannelSchedule);

            _cacheManager.RemoveByPattern(CACHE_BUYER_CHANNEL_SCHEDULE_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(buyerChannelSchedule);

            return buyerChannelSchedule.Id;
        }

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="buyerChannelSchedule">The lead schedule.</param>
        /// <exception cref="ArgumentNullException">buyerChannelSchedule</exception>
        public virtual void UpdateBuyerChannelSchedule(BuyerChannelSchedule buyerChannelSchedule)
        {
            if (buyerChannelSchedule == null)
                throw new ArgumentNullException("buyerChannelSchedule");

            _buyerChannelScheduleRepository.Update(buyerChannelSchedule);

            _cacheManager.RemoveByPattern(CACHE_BUYER_CHANNEL_SCHEDULE_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityUpdated(buyerChannelSchedule);
        }

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="buyerChannelSchedule">The buyer channel schedule.</param>
        /// <exception cref="ArgumentNullException">buyerChannelSchedule</exception>
        public virtual void DeleteBuyerChannelSchedule(BuyerChannelSchedule buyerChannelSchedule)
        {
            if (buyerChannelSchedule == null)
                throw new ArgumentNullException("buyerChannelSchedule");

            _buyerChannelScheduleRepository.Delete(buyerChannelSchedule);

            _cacheManager.RemoveByPattern(CACHE_BUYER_CHANNEL_SCHEDULE_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(buyerChannelSchedule);
        }

        /// <summary>
        /// Deletes the lead schedules by buyer channel identifier.
        /// </summary>
        /// <param name="BuyerChannelId">The buyer channel identifier.</param>
        public virtual void DeleteBuyerChannelSchedulesByBuyerChannelId(long buyerChannelId)
        {
            List<BuyerChannelSchedule> list = (List<BuyerChannelSchedule>)GetBuyerChannelsByBuyerChannelId(buyerChannelId, false, true);

            foreach (var item in list)
            {
                DeleteBuyerChannelSchedule(item);
            }
        }

        #endregion Methods
    }
}