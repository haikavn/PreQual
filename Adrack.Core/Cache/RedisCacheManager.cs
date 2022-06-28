// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="RedisCacheManager.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Infrastructure;
using Adrack.Core.Infrastructure.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adrack.Core.Cache
{
    /// <summary>
    /// Represents a Redis Cache Manager
    /// Implements the <see cref="Adrack.Core.Cache.ICacheManager" />
    /// </summary>
    /// <seealso cref="Adrack.Core.Cache.ICacheManager" />
    public partial class RedisCacheManager : ICacheManager
    {
        #region Fields

        /// <summary>
        /// Application Configuration
        /// </summary>
        private readonly AppConfiguration _appConfiguration;

        /// <summary>
        /// Redis Connection Wrapper
        /// </summary>
        private readonly IRedisConnectionWrapper _redisConnectionWrapper;

        /// <summary>
        /// Database
        /// </summary>
        private readonly IDatabase _database;

        /// <summary>
        /// Cache Manager
        /// </summary>
        private readonly ICacheManager _perRequestCacheManager;

        #endregion Fields

        #region Utilities

        /// <summary>
        /// Serialize
        /// </summary>
        /// <param name="item">Item</param>
        /// <returns>Byte Item</returns>
        protected virtual byte[] Serialize(object item)
        {
            var jsonString = JsonConvert.SerializeObject(item);
            return Encoding.UTF8.GetBytes(jsonString);
        }

        /// <summary>
        /// Deserialize
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="serializedObject">Serialized Object</param>
        /// <returns>Type Item</returns>
        protected virtual T Deserialize<T>(byte[] serializedObject)
        {
            if (serializedObject == null)
                return default(T);

            var jsonString = Encoding.UTF8.GetString(serializedObject);

            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        #endregion Utilities

        #region Constructor

        /// <summary>
        /// Redis Cache Manager
        /// </summary>
        /// <param name="appConfiguration">Application Configuration</param>
        /// <param name="redisConnectionWrapper">Redis Connection Wrapper</param>
        /// <exception cref="Exception">Redis connection string is empty</exception>
        public RedisCacheManager(AppConfiguration appConfiguration, IRedisConnectionWrapper redisConnectionWrapper)
        {
            if (String.IsNullOrEmpty(appConfiguration.RedisCachingConnectionString))
                throw new Exception("Redis connection string is empty");

            this._appConfiguration = appConfiguration;
            this._redisConnectionWrapper = redisConnectionWrapper;
            this._database = _redisConnectionWrapper.Database();
            this._perRequestCacheManager = AppEngineContext.Current.Resolve<ICacheManager>();
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">Key</param>
        /// <returns>The value associated with the specified key</returns>
        public virtual T Get<T>(string key)
        {
            if (!_appConfiguration.ApplicationCacheEnabled)
                return default(T);

            if (_perRequestCacheManager.IsSet(key))
                return _perRequestCacheManager.Get<T>(key);

            var rValue = _database.StringGet(key);

            if (!rValue.HasValue)
                return default(T);

            var result = Deserialize<T>(rValue);

            _perRequestCacheManager.Set(key, result, 0);

            return result;
        }

        public virtual object Get(string key)
        {
            return null;
        }

        /// <summary>
        /// Set
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="data">Data</param>
        /// <param name="cacheTime">Cache Time</param>
        public virtual void Set(string key, object data, int cacheTime)
        {
            if (!_appConfiguration.ApplicationCacheEnabled)
                return;

            if (data == null)
                return;

            var entryBytes = Serialize(data);

            var expiresIn = TimeSpan.FromMinutes(cacheTime);

            _database.StringSet(key, entryBytes, expiresIn);
        }

        public void ClearRemoteServersCache()
        {

        }

        /// <summary>
        /// Is Set
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Boolean Item</returns>
        public virtual bool IsSet(string key)
        {
            if (!_appConfiguration.ApplicationCacheEnabled)
                return false;

            if (_perRequestCacheManager.IsSet(key))
                return true;

            return _database.KeyExists(key);
        }

        /// <summary>
        /// Remove
        /// </summary>
        /// <param name="key">Key</param>
        public virtual void Remove(string key, bool convertKey = false)
        {
            if (!_appConfiguration.ApplicationCacheEnabled)
                return;

            _database.KeyDelete(key);

            _perRequestCacheManager.Remove(key);
        }

        /// <summary>
        /// Remove By Pattern
        /// </summary>
        /// <param name="pattern">Pattern</param>
        public virtual void RemoveByPattern(string pattern)
        {
            if (!_appConfiguration.ApplicationCacheEnabled)
                return;

            foreach (var ep in _redisConnectionWrapper.GetEndpoints())
            {
                var server = _redisConnectionWrapper.Server(ep);

                var keys = server.Keys(pattern: "*" + pattern + "*");

                foreach (var key in keys)
                    _database.KeyDelete(key);
            }
        }

        /// <summary>
        /// Clear
        /// </summary>
        public virtual void Clear()
        {
            foreach (var ep in _redisConnectionWrapper.GetEndpoints())
            {
                var server = _redisConnectionWrapper.Server(ep);

                var keys = server.Keys();

                foreach (var key in keys)
                    _database.KeyDelete(key);
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        protected virtual void Dispose(bool IsManaged)
        {
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        public List<string> GetAllKeys()
        {
            return new List<string>();
        }

        #endregion Methods
    }
}