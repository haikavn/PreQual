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
using System.Data;
using System.Linq;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Class CachedUrlService.
    /// Implements the <see cref="Adrack.Service.Lead.ICachedUrlService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Lead.ICachedUrlService" />
    public partial class CachedUrlService : ICachedUrlService
    {
        #region Constants

        /// <summary>
        /// Cache Profile By Id Key
        /// </summary>
        private const string CACHE_CACHED_URL_BY_ID_KEY = "App.Cache.CachedUrl.By.Id-{0}";

        /// <summary>
        /// Cache Profile All Key
        /// </summary>
        private const string CACHE_CACHED_URL_ALL_KEY = "App.Cache.CachedUrl.All";

        /// <summary>
        /// Cache Profile Pattern Key
        /// </summary>
        private const string CACHE_CACHED_URL_PATTERN_KEY = "App.Cache.CachedUrl.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Profile
        /// </summary>
        private readonly IRepository<CachedUrl> _cachedUrlRepository;

        /// <summary>
        /// Cache Manager
        /// </summary>
        private readonly ICacheManager _cacheManager;

        /// <summary>
        /// Application Event Publisher
        /// </summary>
        private readonly IAppEventPublisher _appEventPublisher;

        /// <summary>
        /// The data provider
        /// </summary>
        private readonly IDataProvider _dataProvider;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Profile Service
        /// </summary>
        /// <param name="cachedUrlRepository">The do not present repository.</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        public CachedUrlService(IRepository<CachedUrl> cachedUrlRepository, ICacheManager cacheManager, IAppEventPublisher appEventPublisher, IDataProvider dataProvider)
        {
            this._cachedUrlRepository = cachedUrlRepository;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
            this._dataProvider = dataProvider;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Insert CachedUrl
        /// </summary>
        /// <param name="filter">The CachedUrl.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">CachedUrl</exception>
        public virtual long InsertCachedUrl(CachedUrl cachedUrl)
        {
            if (cachedUrl == null)
                throw new ArgumentNullException("cachedUrl");

            _cachedUrlRepository.Insert(cachedUrl);

            _cacheManager.RemoveByPattern(CACHE_CACHED_URL_PATTERN_KEY);
            _appEventPublisher.EntityInserted(cachedUrl);

            return cachedUrl.Id;
        }

        public virtual bool CheckCachedUrl(string url)
        {
            return ((from x in _cachedUrlRepository.Table
                    where x.Url == url
                    select x).FirstOrDefault() != null);
        }

        public virtual void DeleteCheckCachedUrl(CachedUrl cachedUrl)
        {
            if (cachedUrl == null)
                throw new ArgumentNullException("cachedUrl");

            _cachedUrlRepository.Delete(cachedUrl);

            _cacheManager.RemoveByPattern(CACHE_CACHED_URL_PATTERN_KEY);
            _appEventPublisher.EntityDeleted(cachedUrl);
        }

        public virtual void DeleteCheckCachedUrls()
        {
            List<CachedUrl> values = (from x in _cachedUrlRepository.Table
                                                 select x).ToList();
            foreach (CachedUrl v in values)
            {
                this.DeleteCheckCachedUrl(v);
            }
        }

        #endregion Methods
    }
}