// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="IRedisConnectionWrapper.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using StackExchange.Redis;
using System;
using System.Net;

namespace Adrack.Core.Cache
{
    /// <summary>
    /// Represents a Redis Connection Wrapper
    /// Implements the <see cref="System.IDisposable" />
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface IRedisConnectionWrapper : IDisposable
    {
        #region Methods

        /// <summary>
        /// Database
        /// </summary>
        /// <param name="db">Database</param>
        /// <returns>Database Item</returns>
        IDatabase Database(int? db = null);

        /// <summary>
        /// Server
        /// </summary>
        /// <param name="endPoint">End Point</param>
        /// <returns>Server Item</returns>
        IServer Server(EndPoint endPoint);

        /// <summary>
        /// End Point
        /// </summary>
        /// <returns>End Point Item</returns>
        EndPoint[] GetEndpoints();

        /// <summary>
        /// Flush Db
        /// </summary>
        /// <param name="db">Database</param>
        void FlushDb(int? db = null);

        #endregion Methods
    }
}