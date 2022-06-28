// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="RedisConnectionWrapper.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Infrastructure.Configuration;
using StackExchange.Redis;
using System;
using System.Net;

namespace Adrack.Core.Cache
{
    /// <summary>
    /// Represents a Redis Connection Manager
    /// Implements the <see cref="Adrack.Core.Cache.IRedisConnectionWrapper" />
    /// </summary>
    /// <seealso cref="Adrack.Core.Cache.IRedisConnectionWrapper" />
    public class RedisConnectionWrapper : IRedisConnectionWrapper
    {
        #region Fields

        /// <summary>
        /// Application Configuration
        /// </summary>
        private readonly AppConfiguration _appConfiguration;

        /// <summary>
        /// Connection String
        /// </summary>
        private readonly Lazy<string> _connectionString;

        /// <summary>
        /// Connection Multiplexer
        /// </summary>
        private volatile ConnectionMultiplexer _connection;

        /// <summary>
        /// Object Lock
        /// </summary>
        private readonly object _objectLock = new object();

        #endregion Fields

        #region Utilities

        /// <summary>
        /// Get Connection
        /// </summary>
        /// <returns>Connection Multiplexer Item</returns>
        private ConnectionMultiplexer GetConnection()
        {
            if (_connection != null && _connection.IsConnected)
                return _connection;

            lock (_objectLock)
            {
                if (_connection != null && _connection.IsConnected)
                    return _connection;

                if (_connection != null)
                {
                    _connection.Dispose();
                }

                _connection = ConnectionMultiplexer.Connect(_connectionString.Value);
            }

            return _connection;
        }

        /// <summary>
        /// Get Connection String
        /// </summary>
        /// <returns>String Item</returns>
        private string GetConnectionString()
        {
            return _appConfiguration.RedisCachingConnectionString;
        }

        #endregion Utilities

        #region Constructor

        /// <summary>
        /// Redis Connection Wrapper
        /// </summary>
        /// <param name="appConfiguration">Application Configuration</param>
        public RedisConnectionWrapper(AppConfiguration appConfiguration)
        {
            this._appConfiguration = appConfiguration;
            this._connectionString = new Lazy<string>(GetConnectionString);
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Database
        /// </summary>
        /// <param name="db">Database</param>
        /// <returns>Database Item</returns>
        public IDatabase Database(int? db = null)
        {
            return GetConnection().GetDatabase(db ?? -1);
        }

        /// <summary>
        /// Server
        /// </summary>
        /// <param name="endPoint">End Point</param>
        /// <returns>Server Item</returns>
        public IServer Server(EndPoint endPoint)
        {
            return GetConnection().GetServer(endPoint);
        }

        /// <summary>
        /// End Point
        /// </summary>
        /// <returns>End Point Item</returns>
        public EndPoint[] GetEndpoints()
        {
            return GetConnection().GetEndPoints();
        }

        /// <summary>
        /// Flush Db
        /// </summary>
        /// <param name="db">Database</param>
        public void FlushDb(int? db = null)
        {
            var endPoints = GetEndpoints();

            foreach (var endPoint in endPoints)
            {
                Server(endPoint).FlushDatabase(db ?? -1);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_connection != null)
                {
                    _connection.Dispose();
                }
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion Methods
    }
}