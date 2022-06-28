// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="PerRequestCacheManager.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Infrastructure.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.Core.Cache
{
    /// <summary>
    /// Represents a Per Request Cache Manager
    /// Implements the <see cref="Adrack.Core.Cache.ICacheManager" />
    /// </summary>
    /// <seealso cref="Adrack.Core.Cache.ICacheManager" />
    public partial class PerRequestCacheManager : ICacheManager
    {
        #region Fields

        /// <summary>
        /// Application Configuration
        /// </summary>
        private readonly AppConfiguration _appConfiguration;

        /// <summary>
        /// Http Context Base
        /// </summary>
        private readonly HttpContextBase _httpContextBase;

        #endregion Fields

        #region Utilities

        /// <summary>
        /// Creates a new instance of the PerRequestCacheManager class
        /// </summary>
        /// <returns>IDictionary.</returns>
        protected virtual IDictionary GetItems()
        {
            if (_httpContextBase != null)
                return _httpContextBase.Items;

            return null;
        }

        #endregion Utilities

        #region Constructor

        /// <summary>
        /// Per Request Cache Manager
        /// </summary>
        /// <param name="httpContextBase">Http Context Base</param>
        /// <param name="appConfiguration">Application Configuration</param>
        public PerRequestCacheManager(HttpContextBase httpContextBase, AppConfiguration appConfiguration)
        {
            this._httpContextBase = httpContextBase;
            this._appConfiguration = appConfiguration;
        }

        #endregion Constructor

        #region Methods

        public void ClearRemoteServersCache()
        {

        }
        /// <summary>
        /// Gets or Sets the value associated with the specified key
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">The key of the value to get</param>
        /// <returns>The value associated with the specified key</returns>
        public virtual T Get<T>(string key)
        {
            if (!_appConfiguration.ApplicationCacheEnabled)
                return default(T);

            var items = GetItems();

            if (items == null)
                return default(T);

            return (T)items[key];
        }

        public virtual object Get(string key)
        {
            return null;
        }

        /// <summary>
        /// Adds the specified key and object to the cache
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="data">Data</param>
        /// <param name="cacheTime">Cache Time</param>
        public virtual void Set(string key, object data, int cacheTime)
        {
            if (!_appConfiguration.ApplicationCacheEnabled)
                return;

            var items = GetItems();

            if (items == null)
                return;

            if (data != null)
            {
                if (items.Contains(key))
                    items[key] = data;
                else
                    items.Add(key, data);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the value associated with the specified key is cached
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Result</returns>
        public virtual bool IsSet(string key)
        {
            if (!_appConfiguration.ApplicationCacheEnabled)
                return false;

            var items = GetItems();

            if (items == null)
                return false;

            return (items[key] != null);
        }

        /// <summary>
        /// Removes the value with the specified key from the cache
        /// </summary>
        /// <param name="key">Key</param>
        public virtual void Remove(string key, bool convertKey = false)
        {
            if (!_appConfiguration.ApplicationCacheEnabled)
                return;

            var items = GetItems();

            if (items == null)
                return;

            items.Remove(key);
        }

        /// <summary>
        /// Removes items by pattern
        /// </summary>
        /// <param name="pattern">Pattern</param>
        public virtual void RemoveByPattern(string pattern)
        {
            if (!_appConfiguration.ApplicationCacheEnabled)
                return;

            var items = GetItems();

            if (items == null)
                return;

            this.RemoveByPattern(pattern, items.Keys.Cast<object>().Select(p => p.ToString()));
        }

        /// <summary>
        /// Clear all cache data
        /// </summary>
        public virtual void Clear()
        {
            var items = GetItems();

            if (items == null)
                return;

            items.Clear();
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
        }

        public List<string> GetAllKeys()
        {
            return new List<string>();
        }
        #endregion Methods
    }
}