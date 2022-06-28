// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="ZipCodeRedirectService.cs" company="Adrack.com">
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
    /// Implements the <see cref="Adrack.Service.Lead.IZipCodeRedirectService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Lead.IZipCodeRedirectService" />
    public partial class ZipCodeRedirectService : IZipCodeRedirectService
    {
        #region Constants

        /// <summary>
        /// Cache Profile By Id Key
        /// </summary>
        private const string CACHE_ZIPCODE_REDIRECT_BY_ID_KEY = "App.Cache.ZipCodeRedirect.By.Id-{0}";

        /// <summary>
        /// Cache Profile By Buyer Id
        /// </summary>
        /// 
        private const string CACHE_ZIPCODE_REDIRECT_BY_BUYER_ID = "App.Cache.ZipCodeRedirect.By.Byter.Id-{0}";
        
        /// <summary>
        /// Cache Profile All Key
        /// </summary>
        private const string CACHE_ZIPCODE_REDIRECT_ALL_KEY = "App.Cache.ZipCodeRedirect.All";

        /// <summary>
        /// Cache Profile All Key
        /// </summary>
        private const string CACHE_ZIPCODE_REDIRECT_BY_ZIPCODE = "App.Cache.ZipCodeRedirect.Zip.{0}";

        /// <summary>
        /// Cache ZipCode redirect for buyer
        /// </summary>
        private const string CACHE_ZIPCODE_REDIRECT_FOR_BUYER = "App.Cache.ZipCodeRedirect.{0}.{1}";

        /// <summary>
        /// Cache Profile Pattern Key
        /// </summary>
        private const string CACHE_ZIPCODE_REDIRECT_PATTERN_KEY = "App.Cache.ZipCodeRedirect.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Profile
        /// </summary>
        private readonly IRepository<ZipCodeRedirect> _zipCodeRedirectRepository;

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
        /// <param name="zipCodeRedirectRepository">The zip code redirect repository.</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="userService">The user service.</param>
        public ZipCodeRedirectService(IRepository<ZipCodeRedirect> zipCodeRedirectRepository, ICacheManager cacheManager, IAppEventPublisher appEventPublisher, IUserService userService)
        {
            this._zipCodeRedirectRepository = zipCodeRedirectRepository;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;

            this._userService = userService;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get Profile By Id
        /// </summary>
        /// <param name="zipCodeRedirectId">The zip code redirect identifier.</param>
        /// <returns>Profile Item</returns>
        public virtual ZipCodeRedirect GetZipCodeRedirectById(long zipCodeRedirectId)
        {
            if (zipCodeRedirectId == 0)
                return null;

            string key = string.Format(CACHE_ZIPCODE_REDIRECT_BY_ID_KEY, zipCodeRedirectId);

            return _cacheManager.Get(key, () => { return _zipCodeRedirectRepository.GetById(zipCodeRedirectId); });
        }

        /// <summary>
        /// Gets the zip code redirect by zip code.
        /// </summary>
        /// <param name="zipCode">The zip code.</param>
        /// <returns>IList&lt;ZipCodeRedirect&gt;.</returns>
        public virtual IList<ZipCodeRedirect> GetZipCodeRedirectByZipCode(string zipCode)
        {
            string key = String.Format(CACHE_ZIPCODE_REDIRECT_BY_ZIPCODE,zipCode);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _zipCodeRedirectRepository.Table
                            where x.ZipCode.Contains(zipCode)
                            select x;

                var profiles = query.ToList();

                return profiles;
            });
        }

        /// <summary>
        /// Get All Profiles
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        public virtual IList<ZipCodeRedirect> GetAllZipCodeRedirects()
        {
            string key = CACHE_ZIPCODE_REDIRECT_ALL_KEY;

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _zipCodeRedirectRepository.Table
                            orderby x.Id descending
                            select x;

                var profiles = query.ToList();

                return profiles;
            });
        }

        /// <summary>
        /// Gets all zip code redirects.
        /// </summary>
        /// <param name="BuyerChannelId">The buyer channel identifier.</param>
        /// <returns>IList&lt;ZipCodeRedirect&gt;.</returns>
        public virtual IList<ZipCodeRedirect> GetAllZipCodeRedirects(long buyerChannelId)
        {
            string key = String.Format(CACHE_ZIPCODE_REDIRECT_BY_BUYER_ID,buyerChannelId);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _zipCodeRedirectRepository.Table
                            where x.BuyerChannelId == buyerChannelId
                            orderby x.Id descending
                            select x;

                var profiles = query.ToList();

                return profiles;
            });
        }

        /// <summary>
        /// Gets all zip code redirects.
        /// </summary>
        /// <param name="BuyerChannelId">The buyer channel identifier.</param>
        /// <param name="zipCode">The zip code.</param>
        /// <returns>IList&lt;ZipCodeRedirect&gt;.</returns>
        public virtual IList<ZipCodeRedirect> GetAllZipCodeRedirects(long buyerChannelId, string zipCode)
        {
            string key = String.Format(CACHE_ZIPCODE_REDIRECT_FOR_BUYER, buyerChannelId, zipCode);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _zipCodeRedirectRepository.Table
                            where x.BuyerChannelId == buyerChannelId && (x.ZipCode.Contains(zipCode) || x.ZipCode == "any")
                            orderby x.Id descending
                            select x;

                var profiles = query.ToList();

                return profiles;
            });
        }

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="zipCodeRedirect">The zip code redirect.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">zipCodeRedirect</exception>
        public virtual long InsertZipCodeRedirect(ZipCodeRedirect zipCodeRedirect)
        {
            if (zipCodeRedirect == null)
                throw new ArgumentNullException("zipCodeRedirect");

            _zipCodeRedirectRepository.Insert(zipCodeRedirect);

            _cacheManager.RemoveByPattern(CACHE_ZIPCODE_REDIRECT_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(zipCodeRedirect);

            return zipCodeRedirect.Id;
        }

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="zipCodeRedirect">The zip code redirect.</param>
        /// <exception cref="ArgumentNullException">zipCodeRedirect</exception>
        public virtual void UpdateZipCodeRedirect(ZipCodeRedirect zipCodeRedirect)
        {
            if (zipCodeRedirect == null)
                throw new ArgumentNullException("zipCodeRedirect");

            _zipCodeRedirectRepository.Update(zipCodeRedirect);

            _cacheManager.RemoveByPattern(CACHE_ZIPCODE_REDIRECT_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityUpdated(zipCodeRedirect);
        }

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="zipCodeRedirect">The zip code redirect.</param>
        /// <exception cref="ArgumentNullException">zipCodeRedirect</exception>
        public virtual void DeleteZipCodeRedirect(ZipCodeRedirect zipCodeRedirect)
        {
            if (zipCodeRedirect == null)
                throw new ArgumentNullException("zipCodeRedirect");

            _zipCodeRedirectRepository.Delete(zipCodeRedirect);

            _cacheManager.RemoveByPattern(CACHE_ZIPCODE_REDIRECT_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(zipCodeRedirect);
        }

        #endregion Methods
    }
}