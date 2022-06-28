// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="AclService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Cache;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Domain.Security;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Adrack.Service.Security
{
    /// <summary>
    /// Represents a Access Control List Service
    /// Implements the <see cref="Adrack.Service.Security.IAclService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Security.IAclService" />
    public partial class AclService : IAclService
    {
        #region Constants

        /// <summary>
        /// Cache Acl By Id Key
        /// </summary>
        private const string CACHE_ACL_BY_ID_KEY = "App.Cache.Acl.By.Id-{0}";

        /// <summary>
        /// Cache Acl By Entity Id Entity Name Key
        /// </summary>
        private const string CACHE_ACL_BY_ENTITY_ID_ENTITY_NAME_KEY = "App.Cache.Acl.By.Entity.Id-Entity.Name-{0}-{1}";

        /// <summary>
        /// Cache Acl All Key
        /// </summary>
        private const string CACHE_ACL_ALL_KEY = "App.Cache.Acl.All";

        /// <summary>
        /// Cache Acl Pattern Key
        /// </summary>
        private const string CACHE_ACL_PATTERN_KEY = "App.Cache.Acl.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Acl
        /// </summary>
        private readonly IRepository<Acl> _aclRepository;

        /// <summary>
        /// Application Context
        /// </summary>
        private readonly IAppContext _appContext;

        /// <summary>
        /// Cache Manager
        /// </summary>
        private readonly ICacheManager _cacheManager;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Acl Service
        /// </summary>
        /// <param name="aclRepository">Acl Repository</param>
        /// <param name="appContext">Application Context</param>
        /// <param name="cacheManager">Cache Manager</param>
        public AclService(IRepository<Acl> aclRepository, IAppContext appContext, ICacheManager cacheManager)
        {
            this._aclRepository = aclRepository;
            this._appContext = appContext;
            this._cacheManager = cacheManager;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get Acl By Id
        /// </summary>
        /// <param name="aclId">Acl Identifier</param>
        /// <returns>Acl Item</returns>
        public virtual Acl GetAclById(long aclId)
        {
            if (aclId == 0)
                return null;

            return _aclRepository.GetById(aclId);
        }

        /// <summary>
        /// Get User Role Ids
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns>Long Item</returns>
        /// <exception cref="ArgumentNullException">entity</exception>
        public virtual long[] GetUserRoleIds<T>(T entity) where T : BaseEntity, IAclSupported
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            long entityId = entity.Id;
            string entityName = typeof(T).Name;

            string key = string.Format(CACHE_ACL_BY_ENTITY_ID_ENTITY_NAME_KEY, entityId, entityName);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _aclRepository.Table
                            where x.EntityId == entityId && x.EntityName == entityName
                            select x.RoleId;

                var result = query.ToArray();

                if (result == null)
                    result = new long[0];

                return result;
            });
        }

        /// <summary>
        /// Get All Acls
        /// </summary>
        /// <returns>Acl Collection Item</returns>
        public virtual IList<Acl> GetAllAcls()
        {
            string key = CACHE_ACL_ALL_KEY;

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _aclRepository.Table
                            orderby x.EntityName, x.Id
                            select x;

                var acls = query.ToList();

                return acls;
            });
        }

        /// <summary>
        /// Get Acls
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns>Acl Collection Item</returns>
        /// <exception cref="ArgumentNullException">entity</exception>
        public virtual IList<Acl> GetAcls<T>(T entity) where T : BaseEntity, IAclSupported
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            long entityId = entity.Id;
            string entityName = typeof(T).Name;

            var query = from x in _aclRepository.Table
                        where x.EntityId == entityId && x.EntityName == entityName
                        select x;

            var acl = query.ToList();

            return acl;
        }

        /// <summary>
        /// Insert Acl
        /// </summary>
        /// <param name="acl">Acl</param>
        /// <exception cref="ArgumentNullException">acl</exception>
        public virtual void InsertAcl(Acl acl)
        {
            if (acl == null)
                throw new ArgumentNullException("acl");

            _aclRepository.Insert(acl);

            _cacheManager.RemoveByPattern(CACHE_ACL_PATTERN_KEY);
        }

        /// <summary>
        /// Insert Acl
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="roleId">Role Identifier</param>
        /// <exception cref="ArgumentNullException">entity</exception>
        /// <exception cref="ArgumentOutOfRangeException">roleId</exception>
        public virtual void InsertAcl<T>(T entity, long roleId) where T : BaseEntity, IAclSupported
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            if (roleId == 0)
                throw new ArgumentOutOfRangeException("roleId");

            long entityId = entity.Id;
            string entityName = typeof(T).Name;

            var acl = new Acl()
            {
                RoleId = roleId,
                EntityId = entityId,
                EntityName = entityName,
            };

            InsertAcl(acl);
        }

        /// <summary>
        /// Update Acl
        /// </summary>
        /// <param name="acl">Acl</param>
        /// <exception cref="ArgumentNullException">acl</exception>
        public virtual void UpdateAcl(Acl acl)
        {
            if (acl == null)
                throw new ArgumentNullException("acl");

            _aclRepository.Update(acl);

            _cacheManager.RemoveByPattern(CACHE_ACL_PATTERN_KEY);
        }

        /// <summary>
        /// Delete Acl
        /// </summary>
        /// <param name="acl">Acl</param>
        /// <exception cref="ArgumentNullException">acl</exception>
        public virtual void DeleteAcl(Acl acl)
        {
            if (acl == null)
                throw new ArgumentNullException("acl");

            _aclRepository.Delete(acl);

            _cacheManager.RemoveByPattern(CACHE_ACL_PATTERN_KEY);
        }

        #region Authorize

        /// <summary>
        /// Authorize
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns>Boolean Item</returns>
        public virtual bool Authorize<T>(T entity) where T : BaseEntity, IAclSupported
        {
            return Authorize(entity, _appContext.AppUser);
        }

        /// <summary>
        /// Authorize
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="user">User</param>
        /// <returns>Boolean Item</returns>
        public virtual bool Authorize<T>(T entity, User user) where T : BaseEntity, IAclSupported
        {
            if (entity == null)
                return false;

            if (user == null)
                return false;

            if (!entity.SubjectToAcl)
                return true;

            foreach (var roleValue1 in user.Roles.Where(x => x.Active && !x.Deleted))
                foreach (var roleValue2 in GetUserRoleIds(entity))
                    if (roleValue1.Id == roleValue2)
                        return true;

            return false;
        }

        #endregion Authorize

        #endregion Methods
    }
}