// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="EfRepository.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Cache;
using Adrack.Core.Helpers;
using Adrack.Core.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace Adrack.Data
{
    /// <summary>
    /// Represents a Entity Framework Repository
    /// Implements the <see cref="Adrack.Core.Infrastructure.Data.IRepository{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Adrack.Core.Infrastructure.Data.IRepository{T}" />
    public partial class EfRepository<T> : IRepository<T> where T : BaseEntity
    {
        #region Fields

        //private IDbClientContext _dbClientContext;

        private ICacheManager _cacheManager;

        /// <summary>
        /// Db Set
        /// </summary>
        private IDbSet<T> _dbSetEntity;

        private IDbContextService _dbContextService;

        #endregion Fields

        #region Utilities

        /// <summary>
        /// Get Full Error Text
        /// </summary>
        /// <param name="dbEntityValidationException">Db Entity Validation Exception</param>
        /// <returns>String Item</returns>
        protected string GetFullErrorText(DbEntityValidationException dbEntityValidationException)
        {
            var errorMessage = string.Empty;

            foreach (var validationErrors in dbEntityValidationException.EntityValidationErrors)
                foreach (var validationError in validationErrors.ValidationErrors)
                    errorMessage += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;

            return errorMessage;
        }

        #endregion Utilities

        #region Constructor

        /// <summary>
        /// Entity Framework Repository
        /// </summary>
        /// <param name="dbContext">Db Context</param>
        public EfRepository(/*IDbClientContext dbClientContext,*/ IDbContextService dbContextService, ICacheManager cacheManager)
        {
            this._dbContextService = dbContextService;
            //this._dbClientContext = dbClientContext;
            this._cacheManager = cacheManager;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Gets a Entity By Identifier
        /// </summary>
        /// <param name="id">Entity Identifier</param>
        /// <returns>Type</returns>
        public virtual T GetById(object id)
        {
            return this.DbSetEntity.Find(id);
        }

        /// <summary>
        /// Insert Entity
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <exception cref="ArgumentNullException">entity</exception>
        /// <exception cref="Exception"></exception>
        public virtual void Insert(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");
                this.DbSetEntity.Add(entity);
                this.DbContext.SetCanTrackChanges(CanTrackChanges);
                this.DbContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEntityValidationException)
            {
                throw new Exception(GetFullErrorText(dbEntityValidationException), dbEntityValidationException);
            }
        }

        public virtual void InsertNoUpdate(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");
                this.DbSetEntity.Add(entity);                
            }
            catch (DbEntityValidationException dbEntityValidationException)
            {
                throw new Exception(GetFullErrorText(dbEntityValidationException), dbEntityValidationException);
            }
        }
        public void TruncateAllStringsOnAllEntitiesToDbSize()
        {
            var objectContext = ((IObjectContextAdapter)(this.DbContext as AppObjectClientContext)).ObjectContext;

            var stringMaxLengthsFromEdmx =
                    objectContext.MetadataWorkspace
                                 .GetItems(DataSpace.CSpace)
                                 .Where(m => m.BuiltInTypeKind == BuiltInTypeKind.EntityType)
                                 .SelectMany(meta => ((EntityType)meta).Properties
                                 .Where(p => p.TypeUsage.EdmType.Name == "String"))
                                 .Select(d => new
                                 {
                                     MaxLength = d.TypeUsage.Facets["MaxLength"].Value,
                                     PropName = d.Name,
                                     EntityName = d.DeclaringType.Name
                                 })
                                 .Where(d => d.MaxLength is int)
                                 .Select(d => new { d.PropName, d.EntityName, MaxLength = Convert.ToInt32(d.MaxLength) })
                                 .ToList();

            var pendingEntities = (this.DbContext as AppObjectClientContext).ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified).Select(x => x.Entity).ToList();
            foreach (var entityObject in pendingEntities)
            {
                var relevantFields = stringMaxLengthsFromEdmx.Where(d => d.EntityName == entityObject.GetType().Name).ToList();

                foreach (var maxLengthString in relevantFields)
                {
                    var prop = entityObject.GetType().GetProperty(maxLengthString.PropName);
                    if (prop == null) continue;

                    var currentValue = prop.GetValue(entityObject);
                    var propAsString = currentValue as string;
                    if (propAsString != null && propAsString.Length > maxLengthString.MaxLength)
                    {
                        prop.SetValue(entityObject, propAsString.Substring(0, maxLengthString.MaxLength));
                    }
                }
            }
        }

        public virtual void SaveChanges()
        {            
            try
            {                
                this.DbContext.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                foreach (var v in ex.Entries)
                {
                    v.State = EntityState.Detached;
                }
                throw new Exception(ex.Message);
            }
            catch (DbEntityValidationException dbEntityValidationException)
            {
                foreach (var v in dbEntityValidationException.EntityValidationErrors)
                {
                    v.Entry.State = EntityState.Detached;
                }
                throw new Exception(GetFullErrorText(dbEntityValidationException), dbEntityValidationException);
            }
        }


        public virtual void Reset()
        {
            var pendingEntities = (this.DbContext as AppObjectClientContext).ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified).Select(x => x.Entity).ToList();
            foreach (var entityObject in pendingEntities)
            {
                this.DbContext.Detach(entityObject);
            }
        }

        public virtual void Truncate()
        {
            //TruncateAllStringsOnAllEntitiesToDbSize();
            
        }
        /// <summary>
        /// Insert Entity
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <exception cref="ArgumentNullException">entity</exception>
        /// <exception cref="Exception"></exception>
        public virtual void Insert(IEnumerable<T> entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                foreach (var dbSet in entity)
                    this.DbSetEntity.Add(dbSet);

                //this._dbContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEntityValidationException)
            {
                throw new Exception(GetFullErrorText(dbEntityValidationException), dbEntityValidationException);
            }
        }

        /// <summary>
        /// Update Entity
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <exception cref="ArgumentNullException">entity</exception>
        /// <exception cref="Exception"></exception>
        public virtual void Update(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                this.DbContext.SetEntityState(entity, EntityState.Modified);

                this.DbContext.SetCanTrackChanges(CanTrackChanges);
                this.DbContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEntityValidationException)
            {
                throw new Exception(GetFullErrorText(dbEntityValidationException), dbEntityValidationException);
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <exception cref="ArgumentNullException">entity</exception>
        /// <exception cref="Exception"></exception>
        public virtual void Update(IEnumerable<T> entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                this.DbContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEntityValidationException)
            {
                throw new Exception(GetFullErrorText(dbEntityValidationException), dbEntityValidationException);
            }
        }

        /// <summary>
        /// Delete Entity
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <exception cref="ArgumentNullException">entity</exception>
        /// <exception cref="Exception"></exception>
        public virtual void Delete(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                this.DbSetEntity.Remove(entity);
                this.DbContext.SetCanTrackChanges(CanTrackChanges);
                this.DbContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEntityValidationException)
            {
                throw new Exception(GetFullErrorText(dbEntityValidationException), dbEntityValidationException);
            }
        }

        /// <summary>
        /// Delete Entity
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <exception cref="ArgumentNullException">entity</exception>
        /// <exception cref="Exception"></exception>
        public virtual void Delete(IEnumerable<T> entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                foreach (var dbSet in entity)
                    this.DbSetEntity.Remove(dbSet);

                this.DbContext.SetCanTrackChanges(CanTrackChanges);
                this.DbContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEntityValidationException)
            {
                throw new Exception(GetFullErrorText(dbEntityValidationException), dbEntityValidationException);
            }
        }

        public bool GetCanTrackChanges()
        {
            return CanTrackChanges;
        }

        public void SetCanTrackChanges(bool canTrackChanges)
        {
            CanTrackChanges = canTrackChanges;
        }

        public string GetSubdomain()
        {
            try
            {
                string subDomain = ConfigurationManager.AppSettings["DataConnectionDefaultDatabase"];

                if (HttpContext.Current == null || HttpContext.Current.Request == null)
                    return "";

                string hostName = HttpContext.Current.Request.Url?.Host;

                if (string.IsNullOrEmpty(hostName))
                    hostName = HttpContext.Current.Request.Headers["referring-url"];

                if (string.IsNullOrEmpty(hostName))
                    hostName = HttpContext.Current.Request.UrlReferrer?.Host;

                string[] domains = hostName.Split(new char[] { '.' });

                string uniqueRequestId = HttpContext.Current.Request.Headers["UniqueRequestId"];

                //if (!hostName.Contains("adrack.xyz"))
                //return subDomain;

                if (domains.Length < 3)
                {
                    if (!string.IsNullOrEmpty(subDomain))
                        return subDomain + "-" + uniqueRequestId;

                    return "";
                }

                return domains[0].Replace("-api", "").Replace("qa-", "dev-") + "-" + uniqueRequestId;
            }
            catch
            {
                return "";
            }
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Get Db Set Entity
        /// </summary>
        /// <value>The database set entity.</value>
        protected virtual IDbSet<T> DbSetEntity
        {
            get
            {
                if (_dbSetEntity == null)
                    _dbSetEntity = DbContext.Set<T>();

                return _dbSetEntity;
            }
        }

        /// <summary>
        /// Gets a Entity Table
        /// </summary>
        /// <value>The table.</value>
        public virtual IQueryable<T> Table
        {
            get
            {
                return this.DbSetEntity;
            }
        }

        /// <summary>
        /// Gets a Entity Table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        /// <value>The table no tracking.</value>
        public virtual IQueryable<T> TableNoTracking
        {
            get
            {
                return this.DbSetEntity.AsNoTracking();
            }
        }

        public bool CanTrackChanges { get; set; } = false;

        public string DbContextName
        {
            get
            {
                string uniqueRequestId = "";
                if (HttpContext.Current.Handler != null)
                    uniqueRequestId = HttpContext.Current?.Request?.Headers["UniqueRequestId"];
                //var clientDbContext = _dbContextService.GetClientContext($"{WebHelper.GetSubdomain()}-{uniqueRequestId}");
                var clientDbContext = _dbContextService.GetClientContext(uniqueRequestId);
                return WebHelper.GetSubdomain() + "-" + WebHelper.GetCurrentUserId() + "-" + clientDbContext.ConnectionString;
            }
        }

        public IDbClientContext DbContext { 
            get
            {
                string uniqueRequestId = "";
                if (HttpContext.Current.Handler != null)
                    uniqueRequestId = HttpContext.Current?.Request?.Headers["UniqueRequestId"];
                var clientDbContext = DbContextService.Instance.GetClientContext($"{WebHelper.GetSubdomain()}-{uniqueRequestId}");
                
                //var clientDbContext = DbContextService.Instance.GetClientContext(WebHelper.GetSubdomain());

                //                if (clientDbContext == null)
                //clientDbContext = this._dbClientContext;
                return clientDbContext;
            }
        }

        public string GetConnectionString()
        {
            return this.DbContextName;
        }

        public List<string> GetAllKeys() 
        {
            return this._cacheManager.GetAllKeys();
        }

        public IDbClientContext GetDbClientContext()
        {
            return DbContext;
        }

        #endregion Properties
    }
}