// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="ICacheManager.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;

namespace Adrack.Core.Cache
{
    /// <summary>
    /// Represents a Cache Manager
    /// Implements the <see cref="System.IDisposable" />
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface ICacheManager : IDisposable
    {
        #region Methods

        /// <summary>
        /// Gets or Sets the value associated with the specified key
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">The key of the value to get</param>
        /// <returns>The value associated with the specified key</returns>
        T Get<T>(string key);

        object Get(string key);


        /// <summary>
        /// Adds the specified key and object to the cache
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="data">Data</param>
        /// <param name="cacheTime">Cache Time</param>
        void Set(string key, object data, int cacheTime);

        /// <summary>
        /// Gets a value indicating whether the value associated with the specified key is cached
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Results</returns>
        bool IsSet(string key);

        /// <summary>
        /// Removes the value with the specified key from the cache
        /// </summary>
        /// <param name="key">Key</param>
        void Remove(string key, bool convertKey = false);


        void ClearRemoteServersCache();
        /// <summary>
        /// Removes items by pattern
        /// </summary>
        /// <param name="pattern">Pattern</param>
        void RemoveByPattern(string pattern);

        /// <summary>
        /// Clear all cache data
        /// </summary>
        void Clear();

        List<string> GetAllKeys();

        #endregion Methods
    }
}