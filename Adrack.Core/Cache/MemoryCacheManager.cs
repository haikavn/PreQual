// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="MemoryCacheManager.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Helpers;
using Adrack.Core.Infrastructure;
using Adrack.Core.Infrastructure.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Caching;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Adrack.Core.Cache
{
    /// <summary>
    /// Represents a Memory Cache Manager
    /// Implements the <see cref="Adrack.Core.Cache.ICacheManager" />
    /// </summary>
    /// <seealso cref="Adrack.Core.Cache.ICacheManager" />
    public partial class MemoryCacheManager : ICacheManager
    {
        #region Fields

        /// <summary>Used internally for cache cleanign control of processing servers</summary>
        public static bool EnableRemoteCacheCleaner = true;

        /// <summary>Enable automatic cache cleaner</summary>
        public static bool EnableAutoCacheMode = false;

        /// <summary>The automatic cache urls</summary>
        public static string AutoCacheUrls = "";
        /// <summary>
        /// Application Configuration
        /// </summary>
        private readonly AppConfiguration _appConfiguration;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Memory Cache Manager
        /// </summary>
        /// <param name="appConfiguration">Application Configuration</param>
        public MemoryCacheManager(AppConfiguration appConfiguration)
        {
            this._appConfiguration = appConfiguration;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Gets or Sets the value associated with the specified key
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">The key of the value to get</param>
        /// <returns>The value associated with the specified key</returns>
        public virtual T Get<T>(string key)
        {
            string newKey = WebHelper.GetSubdomain()+ "-" + key + "-" + WebHelper.GetCurrentUserId();

            if (!_appConfiguration.ApplicationCacheEnabled)
                return (T)Cache[null];

            return (T)Cache[newKey];
        }

        public virtual object Get(string key)
        {
            string newKey = WebHelper.GetSubdomain() + "-" + key + "-" + WebHelper.GetCurrentUserId();

            if (!_appConfiguration.ApplicationCacheEnabled)
                return (object)Cache[null];

            return (object)Cache[newKey];
        }

        private DateTime RemoteCacheCleanerTime=DateTime.MinValue;

        public string GetSiteUrl()
        {
            string url = string.Empty;
            try
            {
                HttpRequest request = HttpContext.Current?.Request;

                if (request == null)
                    return url;

                if (request.IsSecureConnection)
                    url = "https://";
                else
                    url = "http://";

                url += request["HTTP_HOST"] + "/";
            }
            catch
            {

            }

            return url;
        }

        /// <summary>Clears the remote servers cache.</summary>
        public void ClearRemoteServersCache()
        {
            string baseUrl = GetSiteUrl();

            try
            {
                var client = new WebClient();
                client.DownloadString(baseUrl + "Home/ClearCachedUrls");
            }
            catch
            {
            }

            return;

            if (!EnableRemoteCacheCleaner) return;
            if (!EnableAutoCacheMode || string.IsNullOrEmpty(AutoCacheUrls)) return;
            TimeSpan cachePeriod = DateTime.UtcNow - RemoteCacheCleanerTime;

            if (cachePeriod.TotalSeconds < 5)
            {
                return;
            }

            RemoteCacheCleanerTime = DateTime.UtcNow;            

            Task.Run(() =>
            {
                Thread.Sleep(5000);

                
                    string[] urls = AutoCacheUrls.Split(new string[1] { "\r\n" }, StringSplitOptions.None);

                    foreach (string url in urls)
                    {
                        try
                        {
                            var urlToPost = url;
                            if (!url.StartsWith("http://") && !url.StartsWith("https://"))
                                continue;

                            if (!urlToPost.EndsWith("/")) urlToPost = urlToPost + "/";

                            var client = new WebClient();
                            client.DownloadString(urlToPost + "Home/ClearCacheManager");
                        }
                        catch
                        {
                        }

                    }
                
            });
        }

        /// <summary>
        /// Adds the specified key and object to the cache
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="data">Data</param>
        /// <param name="cacheTime">Cache Time</param>
        public virtual void Set(string key, object data, int cacheTime)
        {
            string newKey = WebHelper.GetSubdomain() + "-" + key + "-" + WebHelper.GetCurrentUserId();

            if (!_appConfiguration.ApplicationCacheEnabled)
                return;

            if (data == null)
                return;

            var policy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime)
            };

            Cache.Add(new CacheItem(newKey, data), policy);
        }


        /// <summary>
        /// Gets a value indicating whether the value associated with the specified key is cached
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Result</returns>
        public virtual bool IsSet(string key)
        {
            string newKey = WebHelper.GetSubdomain() + "-" + key + "-" + WebHelper.GetCurrentUserId();

            if (!_appConfiguration.ApplicationCacheEnabled)
                return false;

            return (Cache.Contains(newKey));
        }

        /// <summary>
        /// Removes the value with the specified key from the cache
        /// </summary>
        /// <param name="key">Key</param>
        public virtual void Remove(string key, bool convertKey = false)
        {
            if (!_appConfiguration.ApplicationCacheEnabled)
                return;

            string newKey = key;

            if (convertKey)
                newKey = WebHelper.GetSubdomain() + "-" + key + "-" + WebHelper.GetCurrentUserId();

            Cache.Remove(newKey);
        }

        /// <summary>
        /// Removes items by pattern
        /// </summary>
        /// <param name="pattern">Pattern</param>
        public virtual void RemoveByPattern(string pattern)
        {
            if (!_appConfiguration.ApplicationCacheEnabled)
                return;

            this.RemoveByPattern(pattern, Cache.Select(x => x.Key));
        }

        /// <summary>
        /// Clear all cache data
        /// </summary>
        public virtual void Clear()
        {
            foreach (var item in Cache)
                Remove(item.Key);
        }

        // The bulk of the clean-up code is implemented in Dispose(bool)
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
            List<string> keys = new List<string>();
            foreach (var item in Cache)
                keys.Add(item.Key);
            return keys;
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Object Cache
        /// </summary>
        /// <value>The cache.</value>
        protected ObjectCache Cache
        {
            get
            {
                return MemoryCache.Default;
            }
        }

        #endregion Properties
    }
}