// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="CacheExtensions.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Adrack.Core.Cache
{
    /// <summary>
    /// Represents a Extensions
    /// </summary>
    public static class CacheExtensions
    {
        #region Methods

        /// <summary>
        /// Get Type
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="key">Key</param>
        /// <param name="acquire">Acquire</param>
        /// <returns>T Collection</returns>
        public static T Get<T>(this ICacheManager cacheManager, string key, Func<T> acquire)
        {
            return Get(cacheManager, key, 2280, acquire);
        }

        /// <summary>
        /// Get Type
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="key">Key</param>
        /// <param name="cacheTime">Cache Time</param>
        /// <param name="acquire">Acquire</param>
        /// <returns>T Collection</returns>
        public static T Get<T>(this ICacheManager cacheManager, string key, int cacheTime, Func<T> acquire)
        {
            if (cacheManager.IsSet(key))
            {
                return cacheManager.Get<T>(key);
            }

            var result = acquire();

            if (cacheTime > 0)
                cacheManager.Set(key, result, cacheTime);

            return result;
        }

        /// <summary>
        /// Remove By Pattern
        /// </summary>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="pattern">Pattern</param>
        /// <param name="keys">Keys</param>
        public static void RemoveByPattern(this ICacheManager cacheManager, string pattern, IEnumerable<string> keys)
        {
            WebHelper.GetCurrentUserId();
            //var regex = new Regex(WebHelper.GetSubdomain() + "-" + WebHelper.GetCurrentUserId() + "-" + pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var regex = new Regex(WebHelper.GetSubdomain() + "-" + pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);

            foreach (var key in keys.Where(x => regex.IsMatch(x.ToString())).ToList())
            {
                cacheManager.Remove(key);
            }
        }

        #endregion Methods
    }
}