// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="BuyerResponseService.cs" company="Adrack.com">
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
using System.Linq;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Represents a Profile Service
    /// Implements the <see cref="Adrack.Service.Lead.IBuyerResponseService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Lead.IBuyerResponseService" />
    public partial class BuyerResponseService : IBuyerResponseService
    {
        #region Constants

        /// <summary>
        /// Cache Profile By Id Key
        /// </summary>
        private const string CACHE_BUYER_BY_ID_KEY = "App.Cache.BuyerResponse.By.Id-{0}";

        /// <summary>
        /// Cache Profile All Key
        /// </summary>
        private const string CACHE_BUYER_ALL_KEY = "App.Cache.BuyerResponse.All";

        /// <summary>
        /// Cache Profile All Key
        /// </summary>
        private const string CACHE_BUYER_RESPONCE_BY_ID = "App.Cache.BuyerResponse.Id.{0}";

        /// <summary>
        /// Cache Profile Pattern Key
        /// </summary>
        private const string CACHE_BUYER_PATTERN_KEY = "App.Cache.BuyerResponse.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Profile
        /// </summary>
        private readonly IRepository<BuyerResponse> _affiliateResponseRepository;

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
        public BuyerResponseService(IRepository<BuyerResponse> affiliateResponseRepository, ICacheManager cacheManager, IAppEventPublisher appEventPublisher, IUserService userService)
        {
            this._affiliateResponseRepository = affiliateResponseRepository;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;

            this._userService = userService;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get Profile By Id
        /// </summary>
        /// <param name="buyerId">The buyer identifier.</param>
        /// <returns>Profile Item</returns>
        public virtual BuyerResponse GetBuyerResponseById(long buyerId)
        {
            if (buyerId == 0)
                return null;

            string key = string.Format(CACHE_BUYER_BY_ID_KEY, buyerId);

            return _cacheManager.Get(key, () => { return _affiliateResponseRepository.GetById(buyerId); });
        }

        /// <summary>
        /// Get All Profiles
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        public virtual IList<BuyerResponse> GetAllBuyerResponses()
        {
            string key = CACHE_BUYER_ALL_KEY;

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
        /// Gets the buyer responses by buyer channel identifier.
        /// </summary>
        /// <param name="BuyerChannelId">The buyer channel identifier.</param>
        /// <returns>IList&lt;BuyerResponse&gt;.</returns>
        public virtual IList<BuyerResponse> GetBuyerResponsesByBuyerChannelId(long buyerChannelId)
        {
            string key = String.Format(CACHE_BUYER_RESPONCE_BY_ID,buyerChannelId);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _affiliateResponseRepository.Table
                            where x.BuyerChannelId == buyerChannelId
                            orderby x.Id descending
                            select x;

                var profiles = query.ToList();

                return profiles;
            });
        }

        /// <summary>
        /// Gets the buyer responses by lead identifier.
        /// </summary>
        /// <param name="leadId">The lead identifier.</param>
        /// <returns>IList&lt;BuyerResponse&gt;.</returns>
        public virtual IList<BuyerResponse> GetBuyerResponsesByLeadId(long leadId)
        {
                var query = from x in _affiliateResponseRepository.Table
                            where x.LeadId == leadId
                            orderby x.Id
                            select x;

                var profiles = query.ToList();

                return profiles;           
        }

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="buyerResponse">The buyer response.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">buyerResponse</exception>
        public virtual long InsertBuyerResponse(BuyerResponse buyerResponse)
        {
            if (buyerResponse == null)
                throw new ArgumentNullException("buyerResponse");

            _affiliateResponseRepository.Insert(buyerResponse);

            _cacheManager.RemoveByPattern(CACHE_BUYER_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(buyerResponse);

            return buyerResponse.Id;
        }

        public virtual long InsertBuyerResponseList(IEnumerable<BuyerResponse> list)
        {
            if (list == null)
                throw new ArgumentNullException("list");

            _affiliateResponseRepository.Insert(list);

            _cacheManager.RemoveByPattern(CACHE_BUYER_PATTERN_KEY);
            return 0;
        }

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="buyerResponse">The buyer response.</param>
        /// <exception cref="ArgumentNullException">buyerResponse</exception>
        public virtual void UpdateBuyerResponse(BuyerResponse buyerResponse)
        {
            if (buyerResponse == null)
                throw new ArgumentNullException("buyerResponse");

            _affiliateResponseRepository.Update(buyerResponse);

            _cacheManager.RemoveByPattern(CACHE_BUYER_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityUpdated(buyerResponse);
        }

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="buyerResponse">The buyer response.</param>
        /// <exception cref="ArgumentNullException">buyerResponse</exception>
        public virtual void DeleteBuyerResponse(BuyerResponse buyerResponse)
        {
            if (buyerResponse == null)
                throw new ArgumentNullException("buyerResponse");

            _affiliateResponseRepository.Delete(buyerResponse);

            _cacheManager.RemoveByPattern(CACHE_BUYER_PATTERN_KEY);

            _appEventPublisher.EntityDeleted(buyerResponse);
        }

        #endregion Methods
    }
}