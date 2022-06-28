// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="NavigationService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Cache;
using Adrack.Core.Domain.Common;
using Adrack.Core.Domain.Security;
using Adrack.Core.Infrastructure;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using Adrack.Service.Infrastructure.ApplicationEvent;
using Adrack.Service.Security;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Adrack.Service.Common
{
    /// <summary>
    /// Represents a Navigation Service
    /// Implements the <see cref="Adrack.Service.Common.INavigationService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Common.INavigationService" />
    public partial class NavigationService : INavigationService
    {
        #region Constants

        /// <summary>
        /// Cache Navigation By Id Key
        /// </summary>
        private const string CACHE_NAVIGATION_BY_ID_KEY = "App.Cache.Navigation.By.Id-{0}";

        /// <summary>
        /// Cache Navigation By Layout Key
        /// </summary>
        private const string CACHE_NAVIGATION_BY_LAYOUT_KEY = "App.Cache.Navigation.By.Layout-{0}";

        /// <summary>
        /// The cache navigation by layout controller action published key
        /// </summary>
        private const string CACHE_NAVIGATION_BY_LAYOUT_CONTROLLER_ACTION_PUBLISHED_KEY = "App.Cache.Navigation.By.Layout-{0}-{1}-{2}-{3}";

        /// <summary>
        /// Cache Navigation All Key
        /// </summary>
        private const string CACHE_NAVIGATION_ALL_KEY = "App.Cache.Navigation.All";

        /// <summary>
        /// Cache Navigation Pattern Key
        /// </summary>
        private const string CACHE_NAVIGATION_PATTERN_KEY = "App.Cache.Navigation.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Navigation
        /// </summary>
        private readonly IRepository<Navigation> _navigationRepository;

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
        /// Navigation Service
        /// </summary>
        /// <param name="navigationRepository">Navigation Repository</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        public NavigationService(IRepository<Navigation> navigationRepository, ICacheManager cacheManager, IAppEventPublisher appEventPublisher)
        {
            this._navigationRepository = navigationRepository;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get Navigation By Id
        /// </summary>
        /// <param name="navigationId">Navigation Identifier</param>
        /// <returns>Navigation Item</returns>
        public virtual Navigation GetNavigationById(long navigationId)
        {
            if (navigationId == 0)
                return null;

            string key = string.Format(CACHE_NAVIGATION_BY_ID_KEY, navigationId);

            return _cacheManager.Get(key, () => { return _navigationRepository.GetById(navigationId); });
        }

        /// <summary>
        /// Get Navigation By Layout
        /// </summary>
        /// <param name="layout">The layout.</param>
        /// <returns>Navigation Collection Item</returns>
        public virtual IList<Navigation> GetNavigationByLayout(string layout)
        {
            string key = string.Format(CACHE_NAVIGATION_BY_LAYOUT_KEY, layout);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _navigationRepository.Table
                            orderby x.DisplayOrder, x.Id
                            where x.Layout == layout &&
                                  x.Published &&
                                 !x.Deleted
                            select x;

                var navigations = query.ToList();

                return navigations;
            });
        }

        /// <summary>
        /// Checks the permission.
        /// </summary>
        /// <param name="layout">The layout.</param>
        /// <param name="controler">The controler.</param>
        /// <param name="action">The action.</param>
        /// <param name="onlyPublished">if set to <c>true</c> [only published].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public virtual bool CheckPermission(string layout, string controler, string action, bool onlyPublished = true)
        {
            string key = string.Format(CACHE_NAVIGATION_BY_LAYOUT_CONTROLLER_ACTION_PUBLISHED_KEY, layout, controler, action, onlyPublished);

            var permissionService = AppEngineContext.Current.Resolve<IPermissionService>();
            var appContext = AppEngineContext.Current.Resolve<IAppContext>();

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _navigationRepository.Table
                            orderby x.DisplayOrder, x.Id
                            where x.Layout == layout && x.Controller == controler && x.Action == action &&
                                  ((x.Published && onlyPublished) || !onlyPublished) &&
                                 !x.Deleted
                            select x;

                var item = query.FirstOrDefault();

                if (item == null) return true;

                string[] str = item.Permission.Split(new char[1] { ',' });

                List<long> str1 = new List<long>();

                for (int i = 0; i < str.Length; i++)
                {
                    if (!str[i].Contains("!"))
                    {
                        Permission p = permissionService.GetPermissionById(long.Parse(str[i].Replace(" ", "").Replace("!", "")));
                        if (!permissionService.Authorize(p)) return false;
                    }
                    else
                    {
                        str1.Add(long.Parse(str[i].Replace(" ", "").Replace("!", "")));
                    }
                }

                if (str1.Count > 0)
                {
                    for (int i = 0; i < str1.Count; i++)
                    {
                        Permission p = permissionService.GetPermissionById(str1[i]);
                        if (permissionService.Authorize(p)) return true;
                    }

                    return false;
                }

                return true;
            });
        }

        /// <summary>
        /// Get All Navigations
        /// </summary>
        /// <returns>Navigation Collection Item</returns>
        public virtual IList<Navigation> GetAllNavigations()
        {
            string key = CACHE_NAVIGATION_ALL_KEY;

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _navigationRepository.Table
                            orderby x.DisplayOrder, x.Id
                            select x;

                var navigations = query.ToList();

                return navigations;
            });
        }

        /// <summary>
        /// Insert Navigation
        /// </summary>
        /// <param name="navigation">Navigation</param>
        /// <exception cref="ArgumentNullException">navigation</exception>
        public virtual void InsertNavigation(Navigation navigation)
        {
            if (navigation == null)
                throw new ArgumentNullException("navigation");

            _navigationRepository.Insert(navigation);

            _cacheManager.RemoveByPattern(CACHE_NAVIGATION_PATTERN_KEY);

            _appEventPublisher.EntityInserted(navigation);
        }

        /// <summary>
        /// Update Navigation
        /// </summary>
        /// <param name="navigation">Navigation</param>
        /// <exception cref="ArgumentNullException">navigation</exception>
        public virtual void UpdateNavigation(Navigation navigation)
        {
            if (navigation == null)
                throw new ArgumentNullException("navigation");

            _navigationRepository.Update(navigation);

            _cacheManager.RemoveByPattern(CACHE_NAVIGATION_PATTERN_KEY);

            _appEventPublisher.EntityUpdated(navigation);
        }

        /// <summary>
        /// Delete Navigation
        /// </summary>
        /// <param name="navigation">Navigation</param>
        /// <exception cref="ArgumentNullException">navigation</exception>
        public virtual void DeleteNavigation(Navigation navigation)
        {
            if (navigation == null)
                throw new ArgumentNullException("navigation");

            _navigationRepository.Delete(navigation);

            _cacheManager.RemoveByPattern(CACHE_NAVIGATION_PATTERN_KEY);

            _appEventPublisher.EntityDeleted(navigation);
        }

        #endregion Methods
    }
}