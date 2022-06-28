// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="AppObjectContext.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Cache;
using Adrack.Core.Domain.Content;
using Adrack.Core.Domain.Security;
using Adrack.Core.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Adrack.Data
{
    /// <summary>
    /// Represents a Application Object Context
    /// Implements the <see cref="System.Data.Entity.DbContext" />
    /// Implements the <see cref="Adrack.Data.IDbContext" />
    /// </summary>
    /// <seealso cref="System.Data.Entity.DbContext" />
    /// <seealso cref="Adrack.Data.IDbContext" />
    public class AppObjectContext : DbContext, IDbContext
    {
        #region Utilities

        /// <summary>
        /// On Model Creating
        /// </summary>
        /// <param name="dbModelBuilder">Db Model Builder</param>
        protected override void OnModelCreating(DbModelBuilder dbModelBuilder)
        {
            // dynamically load all configuration
            // System.Type configType = typeof(LanguageMap);

            // any of your configuration classes here
            // var typesToRegister = Assembly.GetAssembly(configType).GetTypes()

            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => !String.IsNullOrEmpty(type.Namespace))
            .Where(type => type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(AppEntityTypeConfiguration<>));

            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);

                dbModelBuilder.Configurations.Add(configurationInstance);
            }

            // dbModelBuilder.Configurations.Add(new LanguageMap());

            base.OnModelCreating(dbModelBuilder);
        }

        /// <summary>
        /// Attach an entity to the context or return an already attached entity (if it was already attached)
        /// </summary>
        /// <typeparam name="TEntity">Entity Type</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns>Attached Entity Type</returns>
        protected virtual TEntity AttachEntityToContext<TEntity>(TEntity entity) where TEntity : BaseEntity, new()
        {
            // Little hack here until Entity Framework really supports stored procedures
            // Otherwise, navigation properties of loaded entities are not loaded until an entity is attached to the context
            var alreadyAttached = Set<TEntity>().Local.FirstOrDefault(x => x.Id == entity.Id);

            if (alreadyAttached == null)
            {
                // Attach new entity
                Set<TEntity>().Attach(entity);

                return entity;
            }
            else
            {
                // Entity is already loaded.
                return alreadyAttached;
            }
        }

        #endregion Utilities

        #region Constructor

        /// <summary>
        /// Application Object Context
        /// </summary>
        /// <param name="connectionString">Connection String</param>
        public AppObjectContext(string connectionString)
            : base(connectionString)
        {
            // ((IObjectContextAdapter)this).ObjectContext.ContextOptions.LazyLoadingEnabled = true;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Create Database Script
        /// </summary>
        /// <returns>SQL Script</returns>
        public string CreateDatabaseScript()
        {
            SetCachedConnectionString();
            return ((IObjectContextAdapter)this).ObjectContext.CreateDatabaseScript();
        }

        /// <summary>
        /// Get DbSet
        /// </summary>
        /// <typeparam name="TEntity">Entity Type</typeparam>
        /// <returns>DbSet</returns>
        public new IDbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            SetCachedConnectionString();
            return base.Set<TEntity>();
        }

        /// <summary>
        /// Execute stores procedure and load a list of entities at the end
        /// </summary>
        /// <typeparam name="TEntity">Entity Type</typeparam>
        /// <param name="sqlCommandText">SQL Command Text</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>Entities</returns>
        /// <exception cref="Exception">Not support parameter type</exception>
        public IList<TEntity> ExecuteStoredProcedureList<TEntity>(string sqlCommandText, params object[] parameters) where TEntity : BaseEntity, new()
        {
            SetCachedConnectionString();

            if (parameters != null && parameters.Length > 0)
            {
                for (int i = 0; i <= parameters.Length - 1; i++)
                {
                    var dbParameter = parameters[i] as DbParameter;

                    if (dbParameter == null)
                        throw new Exception("Not support parameter type");

                    sqlCommandText += i == 0 ? " " : ", ";

                    sqlCommandText += "@" + dbParameter.ParameterName;

                    if (dbParameter.Direction == ParameterDirection.InputOutput || dbParameter.Direction == ParameterDirection.Output)
                    {
                        sqlCommandText += " output";
                    }
                }
            }

            var result = this.Database.SqlQuery<TEntity>(sqlCommandText, parameters).ToList();

            bool autoDetectChangesEnabled = this.Configuration.AutoDetectChangesEnabled;

            try
            {
                this.Configuration.AutoDetectChangesEnabled = false;

                for (int i = 0; i < result.Count; i++)
                    result[i] = AttachEntityToContext(result[i]);
            }
            finally
            {
                this.Configuration.AutoDetectChangesEnabled = autoDetectChangesEnabled;
            }

            return result;
        }

        /// <summary>
        /// Creates a raw SQL query that will return elements of the given generic type. The type can be any type that has properties that match the names of the columns returned from the query, or can be a simple primitive type. The type does not have to be an entity type. The results of this query are never tracked by the context even if the type of object returned is an entity type.
        /// </summary>
        /// <typeparam name="TElement">The type of object returned by the query</typeparam>
        /// <param name="sqlQuery">SQL Query</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>Result</returns>
        public IEnumerable<TElement> SqlQuery<TElement>(string sqlQuery, params object[] parameters)
        {
            SetCachedConnectionString();

            DbRawSqlQuery<TElement> res = null;
            try
            {
                this.Database.CommandTimeout = 120000;
                res = this.Database.SqlQuery<TElement>(sqlQuery, parameters);
            }
            catch
            {
                //Assert.True(false, sqlQuery + " query failed");
            }
            return res;
        }

        /// <summary>
        /// Executes the given DDL/DML command against the database
        /// </summary>
        /// <param name="sqlCommandText">SQL Command Text</param>
        /// <param name="doNotEnsureTransaction">false - the transaction creation is not ensured; true - the transaction creation is ensured</param>
        /// <param name="timeout">Timeout value, in seconds. A null value indicates that the default value of the underlying provider will be used</param>
        /// <param name="parameters">Parameter</param>
        /// <returns>The result returned by the database after executing the command</returns>
        public int ExecuteSqlCommandText(string sqlCommandText, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters)
        {
            SetCachedConnectionString();

            int? previousTimeout = null;

            if (timeout.HasValue)
            {
                previousTimeout = ((IObjectContextAdapter)this).ObjectContext.CommandTimeout;

                ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = timeout;
            }

            var transactionalBehavior = doNotEnsureTransaction ? TransactionalBehavior.DoNotEnsureTransaction : TransactionalBehavior.EnsureTransaction;

            var result = this.Database.ExecuteSqlCommand(transactionalBehavior, sqlCommandText, parameters);

            if (timeout.HasValue)
            {
                ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = previousTimeout;
            }

            return result;
        }

        /// <summary>
        /// Detach an entity
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <exception cref="ArgumentNullException">entity</exception>
        public void Detach(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            SetCachedConnectionString();

            ((IObjectContextAdapter)this).ObjectContext.Detach(entity);
        }


        /// <summary>
        /// Detect changes when save entity
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            try
            {
                SetCachedConnectionString();
                var entityList = new Dictionary<EntityChangeHistory, DbEntityEntry>();
                var changedEntities = ChangeTracker.Entries().Where(p => p.Entity.GetType().Name != "ScheduleTask" && (
                                                                                 p.State == System.Data.Entity.EntityState.Modified ||
                                                                                 p.State == System.Data.Entity.EntityState.Added ||
                                                                                 p.State == System.Data.Entity.EntityState.Deleted)).ToList();

                if (CanTrackChanges && changedEntities.Any())
                {
                    var userId = GetUserId();
                    foreach (var entity in changedEntities)
                    {
                        var entityName = entity.Entity.GetType().Name;
                        var index = entityName.IndexOf("_", StringComparison.Ordinal);
                        entityName = index > 0 ? entityName.Remove(index) : entityName;
                        var changedProperties = GetChangedProperties(entity);

                        var modifiedPropertyHistory = new EntityChangeHistory
                        {
                            Id = 0,
                            ModifiedDate = DateTime.UtcNow,
                            State = entity.State.ToString(),
                            Entity = entityName,
                            EntityId = entity.State == System.Data.Entity.EntityState.Deleted ? entity.OriginalValues.GetValue<long>("Id") : 
                                                                             entity.CurrentValues.GetValue<long>("Id"),
                            UserId = userId,
                            ChangedData = ObjectSerializeToJson(changedProperties)
                        };
                        this.Set<EntityChangeHistory>().Add(modifiedPropertyHistory);
                        
                        if (entity.State == System.Data.Entity.EntityState.Added)
                        {
                            entityList.Add(modifiedPropertyHistory,entity);
                        }
                    }
                } 
                base.SaveChanges();

                if (entityList.Any() && CanTrackChanges)
                    foreach (var entity in entityList)
                    {
                        var history = this.Set<EntityChangeHistory>().Find(entity.Key.Id);
                        history.EntityId = entity.Value.CurrentValues.GetValue<long>("Id");
                        this.Set<EntityChangeHistory>().AddOrUpdate(history);
                    }

                return base.SaveChanges();
            }
            catch (Exception ex)
            {
                if (this.Database.Connection.Database != "{0}")
                    throw ex;
                else
                    return 0;
            }
        }

        private List<PropertyChangeHistory> GetChangedProperties(DbEntityEntry entity)
        {
            var changedProperties = new List<PropertyChangeHistory>();
            switch (entity.State)
            {
                case System.Data.Entity.EntityState.Modified:
                    foreach (var property in entity.OriginalValues.PropertyNames)
                    {
                        var originalProperty = entity.OriginalValues[property];
                        var currentProperty = entity.CurrentValues[property];

                        if (originalProperty != currentProperty &&
                            (originalProperty == null || !originalProperty.Equals(currentProperty)))
                        {
                            changedProperties.Add(new PropertyChangeHistory
                            {
                                Name = property,
                                NewValue = currentProperty != null ? currentProperty.ToString() : string.Empty,
                                OldValue = originalProperty != null ? originalProperty.ToString() : string.Empty,
                                Type = entity.CurrentValues[property] != null ? entity.CurrentValues[property]?.GetType().Name
                                                                              : entity.OriginalValues[property]?.GetType().Name
                            });
                        }
                    }
                    break;
                case System.Data.Entity.EntityState.Added:
                    foreach (var property in entity.CurrentValues.PropertyNames)
                    {
                        changedProperties.Add(new PropertyChangeHistory
                        {
                            Name = property,
                            NewValue = entity.CurrentValues[property]?.ToString(),
                            OldValue = string.Empty,
                            Type = entity.CurrentValues[property]?.GetType().Name
                        });
                    }
                    break;
                case System.Data.Entity.EntityState.Deleted:
                    foreach (var property in entity.OriginalValues.PropertyNames)
                    {
                        changedProperties.Add(new PropertyChangeHistory
                        {
                            Name = property,
                            NewValue = string.Empty,
                            OldValue = entity.OriginalValues[property]?.ToString(),
                            Type = entity.OriginalValues[property]?.GetType().Name
                        });
                    }
                    break;
            }
            return changedProperties;
        }

        private string ObjectSerializeToJson(object entity)
        {
            return JsonConvert.SerializeObject(entity, Formatting.Indented,
                                                new JsonSerializerSettings
                                                {
                                                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                                });
        }

        private long GetUserId()
        {
            var user = HttpContext.Current?.User as CustomPrincipal;
            return user != null ? user.User.Id : 0;
        }

        private void SetCachedConnectionString()
        {
        }

        public bool GetCanTrackChanges()
        {
            return CanTrackChanges;
        }

        public void SetCanTrackChanges(bool canTrackChanges)
        {
            CanTrackChanges = canTrackChanges;
        }

        #endregion Methods

        #region Properties

        public bool CanTrackChanges { get; set; } = false;

        /// <summary>
        /// Gets or Sets the value indicating whether proxy creation setting is enabled (used in EF)
        /// </summary>
        /// <value><c>true</c> if [proxy creation enabled]; otherwise, <c>false</c>.</value>
        public virtual bool ProxyCreationEnabled
        {
            get
            {
                return this.Configuration.ProxyCreationEnabled;
            }
            set
            {
                this.Configuration.ProxyCreationEnabled = value;
            }
        }

        /// <summary>
        /// Gets or Sets the value indicating whether auto detect changes setting is enabled (used in EF)
        /// </summary>
        /// <value><c>true</c> if [automatic detect changes enabled]; otherwise, <c>false</c>.</value>
        public virtual bool AutoDetectChangesEnabled
        {
            get
            {
                return this.Configuration.AutoDetectChangesEnabled;
            }
            set
            {
                this.Configuration.AutoDetectChangesEnabled = value;
            }
        }

        /// <summary>
        /// Gets or Sets the Connection String
        /// </summary>
        /// <value>The connection string.</value>
        public string ConnectionString
        {
            get
            {
                return this.Database.Connection.ConnectionString;
            }
            set
            {
                this.Database.Connection.ConnectionString = value;
            }
        }

        #endregion Properties
    }
}
