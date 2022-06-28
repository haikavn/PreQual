// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="DbContextExtensions.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Adrack.Data
{
    /// <summary>
    /// Represents a Db Context Extensions
    /// </summary>
    public static class DbContextExtensions
    {
        #region Utilities

        /// <summary>
        /// Entity Inner Get Copy
        /// </summary>
        /// <typeparam name="T">Entity Type</typeparam>
        /// <param name="dbContext">Db Context</param>
        /// <param name="currentCopy">Current Copy</param>
        /// <param name="func">Function</param>
        /// <returns>T.</returns>
        private static T InnerGetCopy<T>(IDbClientContext dbContext, T currentCopy, Func<DbEntityEntry<T>, DbPropertyValues> func) where T : BaseEntity
        {
            //Get the database context
            DbContext xDbContext = CastOrThrow(dbContext);

            //Get the entity tracking object
            DbEntityEntry<T> entry = GetEntityOrReturnNull(currentCopy, xDbContext);

            //The output
            T output = null;

            //Try and get the values
            if (entry != null)
            {
                DbPropertyValues dbPropertyValues = func(entry);

                if (dbPropertyValues != null)
                {
                    output = dbPropertyValues.ToObject() as T;
                }
            }

            return output;
        }

        /// <summary>
        /// Get Entity Or Return Null
        /// </summary>
        /// <typeparam name="T">Entity Type</typeparam>
        /// <param name="currentCopy">Current Copy</param>
        /// <param name="dbContext">Db Context</param>
        /// <returns>Db Entity Entry Item</returns>
        private static DbEntityEntry<T> GetEntityOrReturnNull<T>(T currentCopy, DbContext dbContext) where T : BaseEntity
        {
            return dbContext.ChangeTracker.Entries<T>().FirstOrDefault(e => e.Entity == currentCopy);
        }

        /// <summary>
        /// Cast Or Throw
        /// </summary>
        /// <param name="dbContext">Db Context</param>
        /// <returns>Db Context Item</returns>
        /// <exception cref="InvalidOperationException">Context does not support operation.</exception>
        private static DbContext CastOrThrow(IDbClientContext dbContext)
        {
            var xDbContext = dbContext as DbContext;

            if (xDbContext == null)
            {
                throw new InvalidOperationException("Context does not support operation.");
            }

            return xDbContext;
        }

        #endregion Utilities

        #region Methods

        /// <summary>
        /// Load Original Copy
        /// </summary>
        /// <typeparam name="T">Entity Type</typeparam>
        /// <param name="dbContext">Db Context</param>
        /// <param name="currentCopy">Current Copy</param>
        /// <returns>Entity Type Item</returns>
        public static T LoadOriginalCopy<T>(this IDbClientContext dbContext, T currentCopy) where T : BaseEntity
        {
            return InnerGetCopy(dbContext, currentCopy, e => e.OriginalValues);
        }

        /// <summary>
        /// Load Database Copy
        /// </summary>
        /// <typeparam name="T">Entity Type</typeparam>
        /// <param name="dbContext">Db Context</param>
        /// <param name="currentCopy">Current Copy</param>
        /// <returns>Entity Type Item</returns>
        public static T LoadDatabaseCopy<T>(this IDbClientContext dbContext, T currentCopy) where T : BaseEntity
        {
            return InnerGetCopy(dbContext, currentCopy, e => e.GetDatabaseValues());
        }

        /// <summary>
        /// Get Table Name
        /// </summary>
        /// <typeparam name="T">Entity Type</typeparam>
        /// <param name="dbContext">Db Context</param>
        /// <returns>String Item</returns>
        public static string GetTableName<T>(this IDbClientContext dbContext) where T : BaseEntity
        {
            //var tableName = typeof(T).Name;
            //return tableName;

            //this code works only with Entity Framework.
            //If you want to support other database, then use the code above (commented)

            var adapter = ((IObjectContextAdapter)dbContext).ObjectContext;

            var storageModel = (StoreItemCollection)adapter.MetadataWorkspace.GetItemCollection(DataSpace.SSpace);

            var containers = storageModel.GetItems<EntityContainer>();

            var entitySetBase = containers.SelectMany(c => c.BaseEntitySets.Where(bes => bes.Name == typeof(T).Name)).First();

            // Here are variables that will hold table and schema name
            string tableName = entitySetBase.MetadataProperties.First(p => p.Name == "Table").Value.ToString();

            //string schemaName = productEntitySetBase.MetadataProperties.First(p => p.Name == "Schema").Value.ToString();

            return tableName;
        }

        /// <summary>
        /// Get column maximum length
        /// </summary>
        /// <param name="context">Context</param>
        /// <param name="entityTypeName">Entity type name</param>
        /// <param name="columnName">Column name</param>
        /// <returns>Maximum length. Null if such rule does not exist</returns>
        public static int? GetColumnMaxLength(this IDbClientContext context, string entityTypeName, string columnName)
        {
            var rez = GetColumnsMaxLength(context, entityTypeName, columnName);

            return rez.ContainsKey(columnName) ? rez[columnName] as int? : null;
        }

        /// <summary>
        /// Get columns maximum length
        /// </summary>
        /// <param name="context">Context</param>
        /// <param name="entityTypeName">Entity type name</param>
        /// <param name="columnNames">Column names</param>
        /// <returns>IDictionary&lt;System.String, System.Int32&gt;.</returns>
        public static IDictionary<string, int> GetColumnsMaxLength(this IDbClientContext context, string entityTypeName, params string[] columnNames)
        {
            var entType = Type.GetType(entityTypeName);
            var adapter = ((IObjectContextAdapter)context).ObjectContext;
            var metadataWorkspace = adapter.MetadataWorkspace;

            var query = from meta in metadataWorkspace.GetItems(DataSpace.CSpace).Where(x => x.BuiltInTypeKind == BuiltInTypeKind.EntityType)
                        from x in (meta as EntityType).Properties.Where(x => columnNames.Contains(x.Name) && x.TypeUsage.EdmType.Name == "String")
                        select x;

            int temp;

            var queryResult = query.Where(p =>
            {
                var match = p.DeclaringType.Name == entityTypeName;
                if (!match && entType != null)
                {
                    //Is a fully qualified name....
                    match = entType.Name == p.DeclaringType.Name;
                }

                return match;
            }).Select(sel => new { sel.Name, MaxLength = sel.TypeUsage.Facets["MaxLength"].Value }).Where(p => Int32.TryParse(p.MaxLength.ToString(), out temp)).ToDictionary(p => p.Name, p => Convert.ToInt32(p.MaxLength));

            return queryResult;
        }

        /// <summary>
        /// Db Name
        /// </summary>
        /// <param name="context">Context</param>
        /// <returns>String Item</returns>
        public static string DbName(this IDbClientContext context)
        {
            var connection = ((IObjectContextAdapter)context).ObjectContext.Connection as EntityConnection;

            if (connection == null)
                return string.Empty;

            return connection.StoreConnection.Database;
        }

        #endregion Methods
    }
}