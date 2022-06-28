// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="RedirectUrlService.cs" company="Adrack.com">
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
    /// Implements the <see cref="Adrack.Service.Lead.IRedirectUrlService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Lead.IRedirectUrlService" />
    public partial class RedirectUrlService : IRedirectUrlService
    {
        #region Constants

        /// <summary>
        /// Cache Profile By Id Key
        /// </summary>
        private const string CACHE_REDIRECTURL_BY_ID_KEY = "App.Cache.RedirectUrl.By.Id-{0}";

        /// <summary>
        /// Cache Profile All Key
        /// </summary>
        private const string CACHE_REDIRECTURL_ALL_KEY = "App.Cache.RedirectUrl.All.{0}";

        /// <summary>
        /// Cache Profile Pattern Key
        /// </summary>
        private const string CACHE_REDIRECTURL_PATTERN_KEY = "App.Cache.RedirectUrl.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Profile
        /// </summary>
        private readonly IRepository<RedirectUrl> _redirectUrlRepository;

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
        /// <param name="redirectUrlRepository">The redirect URL repository.</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="userService">The user service.</param>
        public RedirectUrlService(IRepository<RedirectUrl> redirectUrlRepository, ICacheManager cacheManager, IAppEventPublisher appEventPublisher, IUserService userService)
        {
            this._redirectUrlRepository = redirectUrlRepository;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
            this._userService = userService;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get Profile By Id
        /// </summary>
        /// <param name="redirectUrlId">The redirect URL identifier.</param>
        /// <returns>Profile Item</returns>
        public virtual RedirectUrl GetRedirectUrlById(long redirectUrlId)
        {
            if (redirectUrlId == 0)
                return null;

            string key = string.Format(CACHE_REDIRECTURL_BY_ID_KEY, redirectUrlId);

            return _cacheManager.Get(key, () => { return _redirectUrlRepository.GetById(redirectUrlId); });
        }

        /// <summary>
        /// Gets the redirect URL by lead identifier.
        /// </summary>
        /// <param name="leadid">The leadid.</param>
        /// <returns>RedirectUrl.</returns>
        public virtual RedirectUrl GetRedirectUrlByLeadId(long leadid)
        {
            string key = String.Format(CACHE_REDIRECTURL_ALL_KEY,leadid);

            return _cacheManager.Get(key, () =>
            {
                return (from x in _redirectUrlRepository.Table
                        where x.LeadId == leadid /*AZ && x.Clicked == false */
                        select x).FirstOrDefault();
            });
        }

        /// <summary>
        /// Gets the redirect URL by key.
        /// </summary>
        /// <param name="navkey">The navkey.</param>
        /// <returns>RedirectUrl.</returns>
        public virtual RedirectUrl GetRedirectUrlByKey(string navkey)
        {
            string key = String.Format(CACHE_REDIRECTURL_ALL_KEY,navkey);

            return _cacheManager.Get(key, () =>
            {
                return (from x in _redirectUrlRepository.Table
                        where x.NavigationKey == navkey && x.Clicked == false
                        select x).FirstOrDefault();
            });
        }

        /// <summary>
        /// Get All Profiles
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        public virtual IList<RedirectUrl> GetAllRedirectUrls()
        {
            string key = String.Format(CACHE_REDIRECTURL_ALL_KEY,"ALL");

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _redirectUrlRepository.Table
                            orderby x.Id descending
                            select x;

                var profiles = query.ToList();

                return profiles;
            });
        }

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="redirectUrl">The redirect URL.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">redirectUrl</exception>
        public virtual long InsertRedirectUrl(RedirectUrl redirectUrl)
        {
            if (redirectUrl == null)
                throw new ArgumentNullException("redirectUrl");

            _redirectUrlRepository.Insert(redirectUrl);

            _cacheManager.RemoveByPattern(CACHE_REDIRECTURL_PATTERN_KEY);
            //_cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(redirectUrl);

            return redirectUrl.Id;
        }

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="redirectUrl">The redirect URL.</param>
        /// <exception cref="ArgumentNullException">redirectUrl</exception>
        public virtual void UpdateRedirectUrl(RedirectUrl redirectUrl)
        {
            if (redirectUrl == null)
                throw new ArgumentNullException("redirectUrl");

            _redirectUrlRepository.Update(redirectUrl);

            _cacheManager.RemoveByPattern(CACHE_REDIRECTURL_PATTERN_KEY);
            //_cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityUpdated(redirectUrl);
        }

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="redirectUrl">The redirect URL.</param>
        /// <exception cref="ArgumentNullException">redirectUrl</exception>
        public virtual void DeleteRedirectUrl(RedirectUrl redirectUrl)
        {
            if (redirectUrl == null)
                throw new ArgumentNullException("redirectUrl");

            _redirectUrlRepository.Delete(redirectUrl);

            _cacheManager.RemoveByPattern(CACHE_REDIRECTURL_PATTERN_KEY);
            //_cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(redirectUrl);
        }

        #endregion Methods
    }
}