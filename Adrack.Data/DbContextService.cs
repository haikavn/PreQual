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
using Adrack.Core.Helpers;
using Adrack.Core.Infrastructure;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;

namespace Adrack.Data
{
    /// <summary>
    /// Represents a Navigation Service
    /// Implements the <see cref="Adrack.Service.Data.IDbContextService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Data.IDbContextService" />
    public partial class DbContextService : IDbContextService
    {
        public static IDbContextService Instance = new DbContextService();

        protected class ClientContextContainer
        {
            public IDbClientContext DbClientContext { get; set; }

            public DateTime CreatedAt { get; set; }
        }

        #region Fields

        private ConcurrentDictionary<string, ClientContextContainer> _clientContexts;
        private ConcurrentDictionary<string, DbConnection> _dbConnections;


        private ConcurrentDictionary<string, int> _clientContextCounts;


        private ConcurrentDictionary<string, string> _connectionStrings;


        private readonly ICacheManager _cacheManager;

        public int ClientContextCount { 
            get {
                return _clientContextCounts.Count;              
            } 
        }


        #endregion Fields

        #region Constructor

        public DbContextService()
        {
            _clientContexts = new ConcurrentDictionary<string, ClientContextContainer>();
            _dbConnections = new ConcurrentDictionary<string, DbConnection>();
            _connectionStrings = new ConcurrentDictionary<string, string>();
            _clientContextCounts = new ConcurrentDictionary<string, int>();
        }


        public DbContextService(ICacheManager cacheManager)
        { 
            _clientContexts = new ConcurrentDictionary<string, ClientContextContainer>();
            _dbConnections = new ConcurrentDictionary<string, DbConnection>();
            _connectionStrings = new ConcurrentDictionary<string, string>();
            _clientContextCounts = new ConcurrentDictionary<string, int>();
            _cacheManager = cacheManager;
        }

        #endregion Constructor

        #region Methods

        public void ClearExpiredDbContexts()
        {
            foreach(var key in _clientContexts.Keys)
            {
                ClientContextContainer value;

                if (_clientContexts.TryRemove(key, out value))
                {
                    if ((DateTime.UtcNow - value.CreatedAt).TotalSeconds >= 120)
                    {
                        if (value != null && value.DbClientContext != null)
                            (value.DbClientContext as AppObjectClientContext).Dispose();
                    }
                }

            }
        }

        public void AddClientContext(string domain, string key, string connectionString)
        {
            ClientContextContainer value;
            AppObjectClientContext clientContext = null;
            if (_clientContexts.TryGetValue(key, out value))
            {
                //clientContext = new AppObjectClientContext(connectionString);
                //clientContext.TryReconnect();
                //_clientContexts.TryUpdate(domain, clientContext, value);
                return;
            }

            //DbConnection dbConnection = new SqlConnection(connectionString);
            //clientContext = new AppObjectClientContext(dbConnection);

            clientContext = new AppObjectClientContext(connectionString);

            value = new ClientContextContainer()
            {
                DbClientContext = clientContext,
                CreatedAt = DateTime.UtcNow
            };

            //clientContext.TryReconnect();

            _clientContexts.TryAdd(key, value);
        }

        public void RemoveClientContext(string key)
        {
            ClientContextContainer value;

            if (_clientContexts.TryRemove(key, out value))
            {
                if (value != null && value.DbClientContext != null)
                {
                    (value.DbClientContext as AppObjectClientContext).Database.Connection.Close();
                    (value.DbClientContext as AppObjectClientContext).Dispose();
                }
            }
        }

        public IDbClientContext GetClientContext(string key)
        {
            /*string dbConnectionString = ConfigurationManager.AppSettings["DataConnectionString"];

            dbConnectionString = string.Format(dbConnectionString, WebHelper.GetSubdomain());

            var clientContext = AppEngineContext.Current.Resolve<IDbClientContext>();
            if ((clientContext as AppObjectClientContext).Database.Connection.ConnectionString != dbConnectionString)
                (clientContext as AppObjectClientContext).Database.Connection.ConnectionString = dbConnectionString;
            return clientContext;*/

            /*string value;

            if (_connectionStrings.TryGetValue(key, out value))
            {
                var clientContext = AppEngineContext.Current.Resolve<IDbClientContext>();
                if ((clientContext as AppObjectClientContext).Database.Connection.ConnectionString != value)
                    (clientContext as AppObjectClientContext).Database.Connection.ConnectionString = value;
                return clientContext;
            }

            return null;*/

            ClientContextContainer value;

            if (string.IsNullOrEmpty(key) || !_clientContexts.TryGetValue(key, out value))
            {
                if (_clientContexts.Keys.Count == 0)
                    return null;
                else
                    _clientContexts.TryGetValue(_clientContexts.Keys.ElementAt(0), out value);
            }

            //value.TryReconnect();

            return value.DbClientContext;
        }

        /*public void AddClientContext(string domain, string connectionString)
        {
            string value = "";
            if (_connectionStrings.TryGetValue(domain, out value))
            {
                return;
            }

            _connectionStrings.TryAdd(domain, connectionString);
        }

        public IDbClientContext GetClientContext(string domain)
        {
            string connectionString = "";

            if (string.IsNullOrEmpty(domain) || !_connectionStrings.TryGetValue(domain, out connectionString))
            {
                if (_connectionStrings.Keys.Count == 0)
                    return null;
                else
                    _connectionStrings.TryGetValue(_connectionStrings.Keys.ElementAt(0), out connectionString);
            }

            AppObjectClientContext clientContext = new AppObjectClientContext(connectionString);

            //clientContext.TryReconnect();

            //var clientContext = AppEngineContext.Current.Resolve<IDbClientContext>();

            return clientContext;
        }*/

        #endregion Methods
    }
}