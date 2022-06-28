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
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
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
    public class AppObjectClientContext : DbContext, IDbClientContext
    {
        protected class EntityHistoryLog
        {
            public string EntityName { get; set; }

            public string MainEntityName { get; set; }

            public string EntityFieldIdName { get; set; }

            public DbEntityEntry DbEntityEntry { get; set; }

            public System.Data.Entity.EntityState EntityState { get; set; }

            public DbPropertyValues CurrentValues { get; set; } = null;

            public DbPropertyValues OriginalValues { get; set; } = null;

            public List<PropertyChangeHistory> PropertyChangeLogs { get; set; } = new List<PropertyChangeHistory>();
        }

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
        public AppObjectClientContext(string connectionString)
            : base(connectionString)
        {
            // ((IObjectContextAdapter)this).ObjectContext.ContextOptions.LazyLoadingEnabled = true;
            this.Database.CommandTimeout = 60;
        }

        public AppObjectClientContext(DbConnection dbConnection)
            : base(dbConnection, false)
        {
            // ((IObjectContextAdapter)this).ObjectContext.ContextOptions.LazyLoadingEnabled = true;
            this.Database.CommandTimeout = 60;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Create Database Script
        /// </summary>
        /// <returns>SQL Script</returns>
        public string CreateDatabaseScript()
        {
            return ((IObjectContextAdapter)this).ObjectContext.CreateDatabaseScript();
        }

        /// <summary>
        /// Get DbSet
        /// </summary>
        /// <typeparam name="TEntity">Entity Type</typeparam>
        /// <returns>DbSet</returns>
        public new IDbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
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

            ((IObjectContextAdapter)this).ObjectContext.Detach(entity);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Detect changes when save entity
        /// </summary>
        /// <returns></returns>
        /*public override int SaveChanges()
        {
            int returnResult = 0;

            try
            {
                var entityList = new Dictionary<EntityChangeHistory, DbEntityEntry>();
                var changedEntities = ChangeTracker.Entries().Where(p => p.Entity.GetType().Name != "ScheduleTask" && (
                                                                                 p.State == System.Data.Entity.EntityState.Modified ||
                                                                                 p.State == System.Data.Entity.EntityState.Added ||
                                                                                 p.State == System.Data.Entity.EntityState.Deleted)).ToList();

                //var context = this.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);

                //returnResult = base.SaveChanges();

                if (CanTrackChanges)
                {
                    if (changedEntities.Any())
                    {
                        var userId = GetUserId();
                        var trackedChangedProperties = new List<PropertyChangeHistory>();
                        foreach (var entity in changedEntities)
                        {
                            var entityType = entity.Entity.GetType();

                            var trackedAttrEntity = entityType?.GetCustomAttributes(typeof(Adrack.Core.Attributes.TrackedAttribute));
                            if (trackedAttrEntity == null || !trackedAttrEntity.Any())
                                continue;

                            var entityName = entityType.Name;
                            var index = entityName.IndexOf("_", StringComparison.Ordinal);
                            entityName = index > 0 ? entityName.Remove(index) : entityName;
                            var changedProperties = GetChangedProperties(entity);


                            trackedChangedProperties.Clear();
                            foreach (var changedProperty in changedProperties)
                            {
                                var trackedAttrProperty = entityType?.GetProperty(changedProperty.Name)?.GetCustomAttributes(typeof(Adrack.Core.Attributes.TrackedAttribute));
                                if (trackedAttrProperty != null && trackedAttrProperty.Any())
                                {
                                    var proportyDisplayName = ((Adrack.Core.Attributes.TrackedAttribute[])trackedAttrProperty)[0].DisplayName;
                                    string proportyIdName = null;

                                    var tableName = ((Adrack.Core.Attributes.TrackedAttribute[])trackedAttrProperty)[0].TableName;
                                    if (!string.IsNullOrEmpty(tableName))
                                    {
                                        var objPoportyIdName = this.Database.SqlQuery<string>($"select [Name] from [dbo].{tableName} where id = {changedProperty.NewValue}").ToList();
                                        if (objPoportyIdName.Any())
                                            proportyIdName = objPoportyIdName[0];
                                    }

                                    trackedChangedProperties.Add(new PropertyChangeHistory
                                    {
                                        Name = proportyDisplayName ?? changedProperty.Name,
                                        NewValue = proportyIdName ?? changedProperty.NewValue,
                                        OldValue = changedProperty.OldValue,
                                        Type = changedProperty.Type
                                    });
                                }
                            }

                            var modifiedPropertyHistory = new EntityChangeHistory
                            {
                                Id = 0,
                                ModifiedDate = DateTime.Now,
                                State = entity.State.ToString(),
                                Entity = entityName,
                                EntityId = entity.State == System.Data.Entity.EntityState.Deleted ? entity.OriginalValues.GetValue<long>("Id") :
                                                                                 entity.CurrentValues.GetValue<long>("Id"),
                                UserId = userId,
                                //ChangedData = ObjectSerializeToJson(changedProperties)
                                ChangedData = ObjectSerializeToJson(trackedChangedProperties)
                            };
                            this.Set<EntityChangeHistory>().Add(modifiedPropertyHistory);

                            if (entity.State == System.Data.Entity.EntityState.Added)
                            {
                                entityList.Add(modifiedPropertyHistory, entity);
                            }
                        }
                    }

                    if (entityList.Any())
                    {
                        foreach (var entity in entityList)
                        {
                            var history = this.Set<EntityChangeHistory>().Find(entity.Key.Id);
                            history.EntityId = entity.Value.CurrentValues.GetValue<long>("Id");
                            this.Set<EntityChangeHistory>().AddOrUpdate(history);
                        }
                    }

                    //base.SaveChanges();
                }

                returnResult = base.SaveChanges();

                //context.Commit();

                return returnResult;
            }
            catch (Exception ex)
            {
                if (this.Database.Connection.Database != "{0}")
                {
                    if (ex.InnerException != null && ex.InnerException.Data != null)
                    {
                        string message = "";
                        foreach (string key in ex.InnerException.Data.Keys)
                        {
                            message += ex.InnerException.Data[key].ToString() + "\r\n";
                        }
                        if (string.IsNullOrEmpty(message))
                            message = "Error: " + ex.InnerException.Message;

                        throw new Exception(message);
                    }
                    throw ex;
                }
                else
                    return 0;
            }
        }*/

        /*if (entityType.Name != entityName)
        {
            if (entityId == 0)
                entityId = mainEntityId;

            modifiedPropertyHistory = this.Set<EntityChangeHistory>().Where(x => x.Entity == entityName && x.EntityId == entityId && x.UserId == userId).FirstOrDefault();

            if (modifiedPropertyHistory != null)
            {
                var existingModifiedProperties = ObjectDeserializeToList(modifiedPropertyHistory.ChangedData);
                existingModifiedProperties.AddRange(trackedChangedProperties);
                modifiedPropertyHistory.ChangedData = ObjectSerializeToJson(existingModifiedProperties);
            }
        }*/

        public override int SaveChanges()
        {
            int returnResult = 0;
            var userId = GetUserId();

            List<EntityHistoryLog> entityHistoryLogs = new List<EntityHistoryLog>();

            try
            {
                ChangeTracker.DetectChanges();

                var entityList = new Dictionary<EntityChangeHistory, DbEntityEntry>();
                var allChanges = ChangeTracker.Entries().ToList();
                var changedEntities = ChangeTracker.Entries().Where(p => p.Entity.GetType().Name != "ScheduleTask" && (
                                                                                 p.State == System.Data.Entity.EntityState.Modified ||
                                                                                 p.State == System.Data.Entity.EntityState.Added ||
                                                                                 p.State == System.Data.Entity.EntityState.Deleted)).ToList();

                //var context = this.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);

                //returnResult = base.SaveChanges();

                if (CanTrackChanges)
                {
                    if (changedEntities.Any())
                    {
                        foreach (var entity in changedEntities)
                        {
                            EntityHistoryLog entityHistoryLog = new EntityHistoryLog();

                            var entityType = entity.Entity.GetType();

                            var trackedAttrEntity = entityType?.GetCustomAttributes(typeof(Adrack.Core.Attributes.TrackedAttribute));
                            if (trackedAttrEntity == null || !trackedAttrEntity.Any())
                                continue;

                            var entityName = ((Adrack.Core.Attributes.TrackedAttribute[])trackedAttrEntity)[0].EntityName;
                            var entityFieldId = ((Adrack.Core.Attributes.TrackedAttribute[])trackedAttrEntity)[0].EntityIdField;

                            try
                            {
                                entityHistoryLog.CurrentValues = entity.CurrentValues.Clone();
                            } catch { }

                            try
                            {
                                entityHistoryLog.OriginalValues = entity.OriginalValues.Clone();
                            } catch { }

                            entityHistoryLog.DbEntityEntry = entity;
                            entityHistoryLog.EntityState = entity.State;
                            entityHistoryLog.EntityName = entityHistoryLog.MainEntityName = entityType.Name;
                            entityHistoryLog.EntityFieldIdName = entityFieldId;

                            if (!string.IsNullOrEmpty(entityName))
                            {
                                entityHistoryLog.MainEntityName = entityName;
                            }

                            if (!string.IsNullOrEmpty(entityHistoryLog.EntityName))
                            {
                                var index = entityHistoryLog.EntityName.IndexOf("_", StringComparison.Ordinal);
                                entityHistoryLog.EntityName = index > 0 ? entityHistoryLog.EntityName.Remove(index) : entityHistoryLog.EntityName;
                            }

                            if (!string.IsNullOrEmpty(entityHistoryLog.MainEntityName))
                            {
                                var index = entityHistoryLog.MainEntityName.IndexOf("_", StringComparison.Ordinal);
                                entityHistoryLog.MainEntityName = index > 0 ? entityHistoryLog.MainEntityName.Remove(index) : entityHistoryLog.MainEntityName;
                            }

                            var changedProperties = GetChangedProperties(entity);

                            foreach (var changedProperty in changedProperties)
                            {
                                var property = entityType?.GetProperty(changedProperty.Name);

                                var trackedAttrProperty = property?.GetCustomAttributes(typeof(Adrack.Core.Attributes.TrackedAttribute));
                                if (trackedAttrProperty != null && trackedAttrProperty.Any())
                                {
                                    var proportyDisplayName = ((Adrack.Core.Attributes.TrackedAttribute[])trackedAttrProperty)[0].DisplayName;
                                    string newValue = changedProperty.NewValue;
                                    string oldValue = changedProperty.OldValue;

                                    var tableName = ((Adrack.Core.Attributes.TrackedAttribute[])trackedAttrProperty)[0].TableName;
                                    var tableFieldName = ((Adrack.Core.Attributes.TrackedAttribute[])trackedAttrProperty)[0].TableFieldName;
                                    var displayType = ((Adrack.Core.Attributes.TrackedAttribute[])trackedAttrProperty)[0].DisplayType;
                                    var converType = property.PropertyType;

                                    if (!string.IsNullOrEmpty(tableName))
                                    {
                                        if (string.IsNullOrEmpty(tableFieldName))
                                            tableFieldName = "Name";

                                        if (!string.IsNullOrEmpty(changedProperty.OldValue))
                                        {
                                            var objPoportyIdName = this.Database.SqlQuery<string>($"select [{tableFieldName}] from [dbo].{tableName} where id = {changedProperty.OldValue}").ToList();
                                            if (objPoportyIdName.Any())
                                                oldValue = objPoportyIdName[0];
                                        }

                                        if (!string.IsNullOrEmpty(changedProperty.NewValue))
                                        {
                                            var objPoportyIdName = this.Database.SqlQuery<string>($"select [{tableFieldName}] from [dbo].{tableName} where id = {changedProperty.NewValue}").ToList();
                                            if (objPoportyIdName.Any())
                                                newValue = objPoportyIdName[0];
                                        }
                                    }

                                    if (string.IsNullOrEmpty(oldValue))
                                        oldValue = changedProperty.OldValue;
                                    else if (converType != null && displayType != null)
                                    {
                                        object value = Convert.ChangeType(oldValue, converType);
                                        if (value != null)
                                        {
                                            if (displayType.IsEnum)
                                                value = Enum.ToObject(displayType, value);
                                            else
                                                value = Convert.ChangeType(value, displayType);
                                            if (value != null)
                                                    oldValue = value.ToString();
                                        }
                                    }

                                    if (string.IsNullOrEmpty(newValue))
                                        newValue = changedProperty.NewValue;
                                    else if (converType != null && displayType != null)
                                    {
                                        object value = Convert.ChangeType(newValue, converType);
                                        if (value != null)
                                        {
                                            if (displayType.IsEnum)
                                                value = Enum.ToObject(displayType, value);
                                            else
                                                value = Convert.ChangeType(value, displayType);
                                            if (value != null)
                                                newValue = value.ToString();
                                        }
                                    }

                                    entityHistoryLog.PropertyChangeLogs.Add(new PropertyChangeHistory
                                    {
                                        Name = proportyDisplayName ?? changedProperty.Name,
                                        NewValue = newValue,
                                        OldValue = oldValue,
                                        Type = changedProperty.Type
                                    });
                                }
                            }

                            entityHistoryLogs.Add(entityHistoryLog);
                        }
                    }
                }

                returnResult = base.SaveChanges();

                EntityChangeHistory modifiedPropertyHistory = null;
                System.Data.Entity.EntityState mainEntityState = System.Data.Entity.EntityState.Added;
                string mainEntityStateStr = mainEntityState.ToString();
                bool firstEntry = false;
                if (entityHistoryLogs.Count == 1)
                    firstEntry = true;

                foreach (var entityHistoryLog in entityHistoryLogs)
                {
                    long entityId = 0;
                    DbPropertyValues dbPropertyValues = entityHistoryLog.CurrentValues;
                    if (dbPropertyValues == null)
                        dbPropertyValues = entityHistoryLog.OriginalValues;

                    if (dbPropertyValues == null) continue;

                    entityId = dbPropertyValues.GetValue<long>("Id");
                    if (entityId == 0 && entityHistoryLog.DbEntityEntry != null)
                    {
                        entityId = entityHistoryLog.DbEntityEntry.CurrentValues.GetValue<long>("Id");
                    }

                    if (entityHistoryLog.EntityName != entityHistoryLog.MainEntityName)
                    {
                        entityId = dbPropertyValues.GetValue<long>(entityHistoryLog.EntityFieldIdName);

                        if (entityId == 0 && entityHistoryLog.DbEntityEntry != null)
                        {
                            entityId = entityHistoryLog.DbEntityEntry.CurrentValues.GetValue<long>(entityHistoryLog.EntityFieldIdName);
                        }

                        if (modifiedPropertyHistory != null && modifiedPropertyHistory.Id > 0)
                        {
                            modifiedPropertyHistory = this.Set<EntityChangeHistory>().Where(x => x.Id == modifiedPropertyHistory.Id).FirstOrDefault();
                        }
                        else
                        {
                            DateTime nowDate = DateTime.UtcNow.AddMinutes(-2);

                            modifiedPropertyHistory = this.Set<EntityChangeHistory>().Where(x => x.Entity == entityHistoryLog.MainEntityName && x.EntityId == entityId && x.UserId == userId && x.ModifiedDate >= nowDate).OrderByDescending(x => x.Id).FirstOrDefault();
                        }

                        /*if (modifiedPropertyHistory != null || firstEntry)
                        {
                            if (!firstEntry)
                            {
                                modifiedPropertyHistory = modifiedPropertyHistory != null && modifiedPropertyHistory.Id > 0 ? this.Set<EntityChangeHistory>().Where(x => x.Id == modifiedPropertyHistory.Id).FirstOrDefault() :
                                    this.Set<EntityChangeHistory>().Where(x => x.Entity == entityHistoryLog.MainEntityName && x.EntityId == entityId && x.UserId == userId && x.State == mainEntityStateStr).OrderByDescending(x => x.Id).FirstOrDefault();
                            }
                            else
                            {
                                string entityStateStr = entityHistoryLog.EntityState.ToString();
                                var parentPropertyHistory = this.Set<EntityChangeHistory>().Where(x => x.Entity == entityHistoryLog.MainEntityName && x.EntityId == entityId && x.UserId == userId && x.State == entityStateStr).OrderByDescending(x => x.Id).FirstOrDefault();
                                if (parentPropertyHistory != null && (DateTime.UtcNow - parentPropertyHistory.ModifiedDate).TotalSeconds > 180)
                                {
                                    parentPropertyHistory = null;
                                }
                                
                                if (parentPropertyHistory == null)
                                {
                                    parentPropertyHistory = new EntityChangeHistory
                                    {
                                        Id = 0,
                                        ModifiedDate = DateTime.UtcNow,
                                        State = entityStateStr,
                                        Entity = entityHistoryLog.MainEntityName,
                                        EntityId = entityId,
                                        UserId = userId,
                                        //ChangedData = ObjectSerializeToJson(changedProperties)
                                        ChangedData = ObjectSerializeToJson(new List<object>())
                                    };
                                    this.Set<EntityChangeHistory>().AddOrUpdate(parentPropertyHistory);
                                    base.SaveChanges();

                                    modifiedPropertyHistory = parentPropertyHistory;
                                }
                            }
                        }
                        else if (mainEntityState != System.Data.Entity.EntityState.Deleted)
                        {
                            mainEntityState = System.Data.Entity.EntityState.Modified;
                            mainEntityStateStr = mainEntityState.ToString();
                        }*/
                    }
                    else
                    {
                        if (mainEntityState != entityHistoryLog.EntityState)
                        {
                            mainEntityState = entityHistoryLog.EntityState;
                            mainEntityStateStr = mainEntityState.ToString();
                        }
                    }

                    if (modifiedPropertyHistory == null)
                    {
                        //if (entityHistoryLog.PropertyChangeLogs.Count > 0)
                        {
                            modifiedPropertyHistory = new EntityChangeHistory
                            {
                                Id = 0,
                                ModifiedDate = DateTime.UtcNow,
                                State = mainEntityStateStr,
                                Entity = entityHistoryLog.MainEntityName,
                                EntityId = entityId,
                                UserId = userId,
                                //ChangedData = ObjectSerializeToJson(changedProperties)
                                ChangedData = ObjectSerializeToJson(entityHistoryLog.PropertyChangeLogs)
                            };
                        }
                    }
                    else
                    {
                        var currentChangeList = ObjectDeserializeToList(modifiedPropertyHistory.ChangedData);
                        currentChangeList.AddRange(entityHistoryLog.PropertyChangeLogs);
                        modifiedPropertyHistory.ChangedData = ObjectSerializeToJson(currentChangeList);
                    }

                    if (modifiedPropertyHistory != null)
                    {
                        this.Set<EntityChangeHistory>().AddOrUpdate(modifiedPropertyHistory);
                        base.SaveChanges();
                    }
                    firstEntry = false;
                }

                if (entityHistoryLogs.Count > 0)
                    base.SaveChanges();

                //context.Commit();

                return returnResult;
            }
            catch (Exception ex)
            {
                if (this.Database.Connection.Database != "{0}")
                {
                    if (ex.InnerException != null && ex.InnerException.Data != null)
                    {
                        string message = "";
                        foreach (string key in ex.InnerException.Data.Keys)
                        {
                            message += ex.InnerException.Data[key].ToString() + "\r\n";
                        }
                        if (string.IsNullOrEmpty(message))
                            message = "Error: " + ex.InnerException.Message;

                        throw new Exception(message);
                    }
                    throw ex;
                }
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

        private List<PropertyChangeHistory> ObjectDeserializeToList(string json)
        {
            return JsonConvert.DeserializeObject<List<PropertyChangeHistory>>(json);
        }

        private long GetUserId()
        {
            var user = HttpContext.Current?.User as CustomPrincipal;
            return user != null ? user.User.Id : 0;
        }

        public bool GetCanTrackChanges()
        {
            return CanTrackChanges;
        }

        public void SetCanTrackChanges(bool canTrackChanges)
        {
            CanTrackChanges = canTrackChanges;
        }

        public void TryReconnect()
        {
            if (this.Database.Connection.State != ConnectionState.Open)
                this.Database.Connection.Open();
        }

        public void SetEntityState(BaseEntity entity, System.Data.Entity.EntityState entityState)
        {
            this.Entry(entity).State = entityState;

            var properties = entity.GetType().GetProperties();

            foreach(var p in properties)
            {
                object value = p.GetValue(entity);
                if (this.Entry(entity).CurrentValues.PropertyNames.Contains(p.Name) && 
                    value != this.Entry(entity).OriginalValues[p.Name])
                {
                    this.Entry(entity).CurrentValues[p.Name] = value;
                }
            }
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