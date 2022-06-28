// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="BuyerChannelTemplateMatchingService.cs" company="Adrack.com">
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
    /// Class BuyerChannelTemplateMatchingService.
    /// Implements the <see cref="Adrack.Service.Lead.IBuyerChannelTemplateMatchingService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Lead.IBuyerChannelTemplateMatchingService" />
    public partial class BuyerChannelTemplateMatchingService : IBuyerChannelTemplateMatchingService
    {
        #region Constants

        /// <summary>
        /// Cache Profile By Id Key
        /// </summary>
        private const string CACHE_BUYER_CHANNEL_TEMPLATE_MATCHING_BY_ID_KEY = "App.Cache.BC.BuyerChannelTemplateMatching.By.Id-{0}";

        /// <summary>
        /// Cache Profile By Template Field Id Key
        /// </summary>
        private const string CACHE_BUYER_CHANNEL_TEMPLATE_MATCHING_BY_TEMPLATE_ID_KEY = "App.Cache.BC.BuyerChannelTemplateMatching.By.Template.Id-{0}";

        /// <summary>
        /// Cache Profile All Key
        /// </summary>
        private const string CACHE_BUYER_CHANNEL_TEMPLATE_MATCHING_ALL_KEY = "App.Cache.BC.BuyerChannelTemplateMatching.All";

        /// <summary>
        /// Cache Profile Pattern Key
        /// </summary>
        private const string CACHE_BUYER_CHANNEL_TEMPLATE_MATCHING_PATTERN_KEY = "App.Cache.BC.BuyerChannelTemplateMatching.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Profile
        /// </summary>
        private readonly IRepository<BuyerChannelTemplateMatching> _buyerChannelTemplateMatchingRepository;

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
        /// <param name="buyerChannelTemplateMatchingRepository">The buyer channel template matching repository.</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        public BuyerChannelTemplateMatchingService(IRepository<BuyerChannelTemplateMatching> buyerChannelTemplateMatchingRepository, ICacheManager cacheManager, IAppEventPublisher appEventPublisher)
        {
            this._buyerChannelTemplateMatchingRepository = buyerChannelTemplateMatchingRepository;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Gets the buyer channel template matching by identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>BuyerChannelTemplateMatching.</returns>
        public virtual BuyerChannelTemplateMatching GetBuyerChannelTemplateMatchingById(long Id)
        {
            if (Id == 0)
                return null;

            return _buyerChannelTemplateMatchingRepository.GetById(Id);
        }

        /// <summary>
        /// Gets the buyer channel template matchings by template identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>IList&lt;BuyerChannelTemplateMatching&gt;.</returns>
        public virtual IList<BuyerChannelTemplateMatching> GetBuyerChannelTemplateMatchingsByTemplateId(long Id)
        {
            string key = string.Format(CACHE_BUYER_CHANNEL_TEMPLATE_MATCHING_BY_TEMPLATE_ID_KEY, Id);

            return _cacheManager.Get(key, () =>
            {
                return (from x in _buyerChannelTemplateMatchingRepository.Table
                        where x.BuyerChannelTemplateId == Id
                        orderby x.Id
                        select x).ToList();
            });
        }

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="BuyerChannelTemplateMatching">The buyer channel template matching.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">BuyerChannelTemplateMatching</exception>
        public virtual long InsertBuyerChannelTemplateMatching(BuyerChannelTemplateMatching BuyerChannelTemplateMatching)
        {
            if (BuyerChannelTemplateMatching == null)
                throw new ArgumentNullException("BuyerChannelTemplateMatching");

            _buyerChannelTemplateMatchingRepository.Insert(BuyerChannelTemplateMatching);

            _cacheManager.RemoveByPattern(CACHE_BUYER_CHANNEL_TEMPLATE_MATCHING_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(BuyerChannelTemplateMatching);

            return BuyerChannelTemplateMatching.Id;
        }

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="BuyerChannelTemplateMatching">The buyer channel template matching.</param>
        /// <exception cref="ArgumentNullException">BuyerChannelTemplateMatching</exception>
        public virtual void UpdateBuyerChannelTemplateMatching(BuyerChannelTemplateMatching BuyerChannelTemplateMatching)
        {
            if (BuyerChannelTemplateMatching == null)
                throw new ArgumentNullException("BuyerChannelTemplateMatching");

            _buyerChannelTemplateMatchingRepository.Update(BuyerChannelTemplateMatching);

            _cacheManager.RemoveByPattern(CACHE_BUYER_CHANNEL_TEMPLATE_MATCHING_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityUpdated(BuyerChannelTemplateMatching);
        }

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="BuyerChannelTemplateMatching">The buyer channel template matching.</param>
        /// <exception cref="ArgumentNullException">BuyerChannelTemplateMatching</exception>
        public virtual void DeleteBuyerChannelTemplateMatching(BuyerChannelTemplateMatching BuyerChannelTemplateMatching)
        {
            if (BuyerChannelTemplateMatching == null)
                throw new ArgumentNullException("BuyerChannelTemplateMatching");

            _buyerChannelTemplateMatchingRepository.Delete(BuyerChannelTemplateMatching);

            _cacheManager.RemoveByPattern(CACHE_BUYER_CHANNEL_TEMPLATE_MATCHING_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(BuyerChannelTemplateMatching);
        }

        /// <summary>
        /// Deletes the buyer channel template matchings by template identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public virtual void DeleteBuyerChannelTemplateMatchingsByTemplateId(long id)
        {
            var list = (from x in _buyerChannelTemplateMatchingRepository.Table
                        where x.BuyerChannelTemplateId == id
                        select x).ToList();

            foreach (var item in list)
            {
                DeleteBuyerChannelTemplateMatching(item);
            }
        }

        #endregion Methods
    }
}