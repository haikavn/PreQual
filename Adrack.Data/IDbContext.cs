// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="IDbContext.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using System.Collections.Generic;
using System.Data.Entity;

namespace Adrack.Data
{
    /// <summary>
    /// Represents a Db Context
    /// </summary>
    public interface IDbContext
    {
        #region Methods

        /// <summary>
        /// Get DbSet
        /// </summary>
        /// <typeparam name="TEntity">Entity Type</typeparam>
        /// <returns>DbSet</returns>
        IDbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity;

        /// <summary>
        /// Save changes
        /// </summary>
        /// <returns>Integer</returns>
        int SaveChanges();

        /// <summary>
        /// Execute stores procedure and load a list of entities at the end
        /// </summary>
        /// <typeparam name="TEntity">Entity Type</typeparam>
        /// <param name="sqlCommandText">SQL Command Text</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>Entities</returns>
        IList<TEntity> ExecuteStoredProcedureList<TEntity>(string sqlCommandText, params object[] parameters) where TEntity : BaseEntity, new();

        /// <summary>
        /// Creates a raw SQL query that will return elements of the given generic type. The type can be any type that has properties that match the names of the columns returned from the query, or can be a simple primitive type. The type does not have to be an entity type. The results of this query are never tracked by the context even if the type of object returned is an entity type.
        /// </summary>
        /// <typeparam name="TElement">The type of object returned by the query</typeparam>
        /// <param name="sqlQuery">SQL Query</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>Result</returns>
        IEnumerable<TElement> SqlQuery<TElement>(string sqlQuery, params object[] parameters);

        /// <summary>
        /// Executes the given DDL/DML command against the database
        /// </summary>
        /// <param name="sqlCommandText">SQL Command Text</param>
        /// <param name="doNotEnsureTransaction">false - the transaction creation is not ensured; true - the transaction creation is ensured</param>
        /// <param name="timeout">Timeout value, in seconds. A null value indicates that the default value of the underlying provider will be used</param>
        /// <param name="parameters">Parameter</param>
        /// <returns>The result returned by the database after executing the command</returns>
        int ExecuteSqlCommandText(string sqlCommandText, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters);

        bool GetCanTrackChanges();

        void SetCanTrackChanges(bool canTrackChanges);

        #endregion Methods

        #region Properties

        /// <summary>
        /// Detach an entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Detach(object entity);

        /// <summary>
        /// Gets or Sets the value indicating whether proxy creation setting is enabled (used in EF)
        /// </summary>
        /// <value><c>true</c> if [proxy creation enabled]; otherwise, <c>false</c>.</value>
        bool ProxyCreationEnabled { get; set; }

        /// <summary>
        /// Gets or Sets the value indicating whether auto detect changes setting is enabled (used in EF)
        /// </summary>
        /// <value><c>true</c> if [automatic detect changes enabled]; otherwise, <c>false</c>.</value>
        bool AutoDetectChangesEnabled { get; set; }

        /// <summary>
        /// Gets or Sets the Connection String
        /// </summary>
        /// <value>The connection string.</value>
        string ConnectionString { get; set; }

        #endregion Properties
    }
}