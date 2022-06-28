// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="PostedDataService.cs" company="Adrack.com">
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
    /// Implements the <see cref="Adrack.Service.Lead.IPostedDataService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Lead.IPostedDataService" />
    public partial class PostedDataService : IPostedDataService
    {
        #region Constants

        /// <summary>
        /// Cache Profile By Id Key
        /// </summary>
        private const string CACHE_POSTED_DATA_BY_ID_KEY = "App.Cache.PostedData.By.Id-{0}";

        /// <summary>
        /// Cache Profile All Key
        /// </summary>
        private const string CACHE_POSTED_DATA_ALL_KEY = "App.Cache.PostedData.All";

        /// <summary>
        /// Cache Profile Pattern Key
        /// </summary>
        private const string CACHE_POSTED_DATA_PATTERN_KEY = "App.Cache.PostedData.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Profile
        /// </summary>
        private readonly IRepository<PostedData> _postedDataRepository;

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
        public PostedDataService(IRepository<PostedData> affiliateResponseRepository, ICacheManager cacheManager, IAppEventPublisher appEventPublisher, IUserService userService)
        {
            this._postedDataRepository = affiliateResponseRepository;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;

            this._userService = userService;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get Profile By Id
        /// </summary>
        /// <param name="postedDataId">The posted data identifier.</param>
        /// <returns>Profile Item</returns>
        public virtual PostedData GetPostedDataById(long postedDataId)
        {
            if (postedDataId == 0)
                return null;

            string key = string.Format(CACHE_POSTED_DATA_BY_ID_KEY, postedDataId);

            return _cacheManager.Get(key, () => { return _postedDataRepository.GetById(postedDataId); });
        }

        /// <summary>
        /// Get All Profiles
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        public virtual IList<PostedData> GetAllPostedDatas()
        {
            string key = CACHE_POSTED_DATA_ALL_KEY;

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _postedDataRepository.Table
                            orderby x.Id descending
                            select x;

                var profiles = query.ToList();

                return profiles;
            });
        }

        /// <summary>
        /// Gets the posted datas by buyer channel identifier.
        /// </summary>
        /// <param name="BuyerChannelId">The buyer channel identifier.</param>
        /// <returns>IList&lt;PostedData&gt;.</returns>
        public virtual IList<PostedData> GetPostedDatasByBuyerChannelId(long buyerChannelId)
        {
                var query = from x in _postedDataRepository.Table
                            where x.BuyerChannelId == buyerChannelId
                            orderby x.Id descending
                            select x;

                var profiles = query.ToList();

                return profiles;            
        }

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="affiliateResponse">The affiliate response.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">affiliateResponse</exception>
        public virtual long InsertPostedData(PostedData affiliateResponse)
        {
            if (affiliateResponse == null)
                throw new ArgumentNullException("affiliateResponse");

            _postedDataRepository.Insert(affiliateResponse);

            _cacheManager.RemoveByPattern(CACHE_POSTED_DATA_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(affiliateResponse);

            return affiliateResponse.Id;
        }

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="affiliateResponse">The affiliate response.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">affiliateResponse</exception>
        public virtual long InsertPostedDataList(IEnumerable<PostedData> affiliateResponse)
        {
            if (affiliateResponse == null)
                throw new ArgumentNullException("affiliateResponse");

            _postedDataRepository.Insert(affiliateResponse);

            _cacheManager.RemoveByPattern(CACHE_POSTED_DATA_PATTERN_KEY);
            return 0;            
        }
        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="affiliateResponse">The affiliate response.</param>
        /// <exception cref="ArgumentNullException">affiliateResponse</exception>
        public virtual void UpdatePostedData(PostedData affiliateResponse)
        {
            if (affiliateResponse == null)
                throw new ArgumentNullException("affiliateResponse");

            _postedDataRepository.Update(affiliateResponse);

            _cacheManager.RemoveByPattern(CACHE_POSTED_DATA_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityUpdated(affiliateResponse);
        }

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="affiliateResponse">The affiliate response.</param>
        /// <exception cref="ArgumentNullException">affiliateResponse</exception>
        public virtual void DeletePostedData(PostedData affiliateResponse)
        {
            if (affiliateResponse == null)
                throw new ArgumentNullException("affiliateResponse");

            _postedDataRepository.Delete(affiliateResponse);

            _cacheManager.RemoveByPattern(CACHE_POSTED_DATA_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(affiliateResponse);
        }

        #endregion Methods
    }
}