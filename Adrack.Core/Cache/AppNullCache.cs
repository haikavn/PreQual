// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="AppNullCache.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;

namespace Adrack.Core.Cache
{
    /// <summary>
    /// Represents a Application Null Cache
    /// Implements the <see cref="Adrack.Core.Cache.ICacheManager" />
    /// </summary>
    /// <seealso cref="Adrack.Core.Cache.ICacheManager" />
    public partial class AppNullCache : ICacheManager
    {
        #region Methods

        /// <summary>
        /// Gets or Sets the value associated with the specified key
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">The key of the value to get</param>
        /// <returns>The value associated with the specified key</returns>
        public virtual T Get<T>(string key)
        {
            return default(T);
        }

        public virtual object Get(string key)
        {
            return null;
        }

        public void ClearRemoteServersCache()
        {


        }

        /// <summary>
        /// Adds the specified key and object to the cache
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="data">Data</param>
        /// <param name="cacheTime">Cache Time</param>
        public virtual void Set(string key, object data, int cacheTime)
        {
        }

        /// <summary>
        /// Gets a value indicating whether the value associated with the specified key is cached
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Result</returns>
        public bool IsSet(string key)
        {
            return false;
        }

        /// <summary>
        /// Removes the value with the specified key from the cache
        /// </summary>
        /// <param name="key">Key</param>
        public virtual void Remove(string key, bool convertKey = false)
        {
        }

        /// <summary>
        /// Removes items by pattern
        /// </summary>
        /// <param name="pattern">Patern</param>
        public virtual void RemoveByPattern(string pattern)
        {
        }

        /// <summary>
        /// Clear all cache data
        /// </summary>
        public virtual void Clear()
        {
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public List<string> GetAllKeys()
        {
            return new List<string>();
        }

        #endregion Methods
    }
}