// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="UserSubscribtionService.cs" company="Adrack.com">
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
    /// Class UserSubscribtionService.
    /// Implements the <see cref="Adrack.Service.Lead.IUserSubscribtionService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Lead.IUserSubscribtionService" />
    public partial class UserSubscribtionService : IUserSubscribtionService
    {
        #region Constants

        /// <summary>
        /// Cache Profile By Id Key
        /// </summary>
        private const string CACHE_USER_SUBSCRIBTION_BY_ID_KEY = "App.Cache.UserSubscribtion.By.Id-{0}";

        /// <summary>
        /// Cache Profile All Key
        /// </summary>
        private const string CACHE_USER_SUBSCRIBTION_ALL_KEY = "App.Cache.UserSubscribtion.All";

        /// <summary>
        /// Cache Profile Pattern Key
        /// </summary>
        private const string CACHE_USER_SUBSCRIBTION_PATTERN_KEY = "App.Cache.UserSubscribtion.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Profile
        /// </summary>
        private readonly IRepository<UserSubscribtion> _userSubscribtionRepository;

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
        /// <param name="userSubscribtionRepository">The user subscribtion repository.</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        public UserSubscribtionService(IRepository<UserSubscribtion> userSubscribtionRepository, ICacheManager cacheManager, IAppEventPublisher appEventPublisher)
        {
            this._userSubscribtionRepository = userSubscribtionRepository;
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
        public virtual UserSubscribtion GetUserSubscribtionById(long Id)
        {
            if (Id == 0)
                return null;

            string key = string.Format(CACHE_USER_SUBSCRIBTION_BY_ID_KEY, Id);

            return _cacheManager.Get(key, () => { return _userSubscribtionRepository.GetById(Id); });
        }

        /// <summary>
        /// Get All Profiles
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        public virtual IList<UserSubscribtion> GetUserSubscribtions()
        {
            string key = CACHE_USER_SUBSCRIBTION_ALL_KEY;

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _userSubscribtionRepository.Table
                            orderby x.Id descending
                            select x;

                var profiles = query.ToList();

                return profiles;
            });
        }

        /// <summary>
        /// Gets the user subscribtions.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>IList&lt;UserSubscribtion&gt;.</returns>
        public virtual IList<UserSubscribtion> GetUserSubscribtions(long userId)
        {
                var query = from x in _userSubscribtionRepository.Table
                            where x.UserId == userId
                            select x;

                var profiles = query.ToList();

                return profiles;         
        }

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="userSubscribtion">The user subscribtion.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">userSubscribtion</exception>
        public virtual long InsertUserSubscribtion(UserSubscribtion userSubscribtion)
        {
            if (userSubscribtion == null)
                throw new ArgumentNullException("userSubscribtion");

            _userSubscribtionRepository.Insert(userSubscribtion);

            _cacheManager.RemoveByPattern(CACHE_USER_SUBSCRIBTION_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(userSubscribtion);

            return userSubscribtion.Id;
        }

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="userSubscribtion">The user subscribtion.</param>
        /// <exception cref="ArgumentNullException">userSubscribtion</exception>
        public virtual void UpdateUserSubscribtion(UserSubscribtion userSubscribtion)
        {
            if (userSubscribtion == null)
                throw new ArgumentNullException("userSubscribtion");

            _userSubscribtionRepository.Update(userSubscribtion);

            _cacheManager.RemoveByPattern(CACHE_USER_SUBSCRIBTION_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityUpdated(userSubscribtion);
        }

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="userSubscribtion">The user subscribtion.</param>
        /// <exception cref="ArgumentNullException">userSubscribtion</exception>
        public virtual void DeleteUserSubscribtion(UserSubscribtion userSubscribtion)
        {
            if (userSubscribtion == null)
                throw new ArgumentNullException("userSubscribtion");

            _userSubscribtionRepository.Delete(userSubscribtion);

            _cacheManager.RemoveByPattern(CACHE_USER_SUBSCRIBTION_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(userSubscribtion);
        }

        #endregion Methods
    }
}