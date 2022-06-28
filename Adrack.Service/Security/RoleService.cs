// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="RoleService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Cache;
using Adrack.Core.Domain.Security;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using Adrack.Service.Infrastructure.ApplicationEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using Adrack.Core.Infrastructure;

namespace Adrack.Service.Security
{
    /// <summary>
    /// Represents a Role Service
    /// Implements the <see cref="Adrack.Service.Security.IRoleService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Security.IRoleService" />
    public partial class RoleService : IRoleService
    {
        #region Constants

        /// <summary>
        /// Cache Role By Id Key
        /// </summary>
        private const string CACHE_ROLE_BY_ID_KEY = "App.Cache.Role.By.Id-{0}";

        /// <summary>
        /// Cache Role By Name Key
        /// </summary>
        private const string CACHE_ROLE_BY_NAME_KEY = "App.Cache.Role.By.Name-{0}";

        /// <summary>
        /// Cache Role By Key Key
        /// </summary>
        private const string CACHE_ROLE_BY_KEY_KEY = "App.Cache.Role.By.Key-{0}";

        /// <summary>
        /// Cache Role All Key
        /// </summary>
        private const string CACHE_ROLE_ALL_KEY = "App.Cache.Role.All";

        /// <summary>
        /// Cache Role All By User Type Id
        /// </summary>
        private const string CACHE_ROLE_ALL_BY_USER_TYPE_KEY = "App.Cache.Role.By.User.Type-{0}-{1}-{2}";

        /// <summary>
        /// Cache Role Pattern Key
        /// </summary>
        private const string CACHE_ROLE_PATTERN_KEY = "App.Cache.Role.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Role
        /// </summary>
        private readonly IRepository<Role> _roleRepository;

        /// <summary>
        /// Cache Manager
        /// </summary>
        private readonly ICacheManager _cacheManager;

        /// <summary>
        /// Application Event Publisher
        /// </summary>
        private readonly IAppEventPublisher _appEventPublisher;


        #endregion Fields

        #region Constructor

        /// <summary>
        /// Role Service
        /// </summary>
        /// <param name="roleRepository">Role Repository</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        public RoleService(IRepository<Role> roleRepository, ICacheManager cacheManager, IAppEventPublisher appEventPublisher)
        {
            this._roleRepository = roleRepository;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Generate System Roles
        /// </summary>
        public virtual void GenerateSystemRoles()
        {
            var permissionService = AppEngineContext.Current.Resolve<IPermissionService>();
            if (permissionService == null)
                throw new AppException("No IPermissionService found");

            IList<Permission> permissions = permissionService.GetAllPermissions();
            if (permissions == null)
                throw new AppException("No Permissions found");
            
            CreateSystemRoles("Administrator", UserTypes.Network, "all", permissionService, permissions);
            CreateSystemRoles("Affiliate Manager", UserTypes.Network, "affiliate", permissionService, permissions);
            CreateSystemRoles("Buyer Manager", UserTypes.Network, "buyer", permissionService, permissions);
            CreateSystemRoles("Affiliate portal", UserTypes.Affiliate, "affiliate", permissionService, permissions);
            CreateSystemRoles("Buyer portal", UserTypes.Buyer, "buyer", permissionService, permissions);
        }


        /// <summary>
        /// Get Role By Id
        /// </summary>
        /// <param name="roleId">Role Identifier</param>
        /// <returns>Role Item</returns>
        public virtual Role GetRoleById(long roleId)
        {
            if (roleId == 0)
                return null;

            return _roleRepository.GetById(roleId);
        }

        /// <summary>
        /// Get Role By Name
        /// </summary>
        /// <param name="name">Name</param>
        /// <returns>Role Item</returns>
        public virtual Role GetRoleByName(string name)
        {
            //if (String.IsNullOrWhiteSpace(name))
            //  return null;

            string key = string.Format(CACHE_ROLE_BY_NAME_KEY, name);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _roleRepository.Table
                            where x.Name == name
                            select x;

                var role = query.FirstOrDefault();

                return role;
            });
        }

        /// <summary>
        /// Get Role By Name
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Role Item</returns>
        public virtual Role GetRoleByKey(string key)
        {
            if (String.IsNullOrWhiteSpace(key))
                return null;

            string keyName = string.Format(CACHE_ROLE_BY_KEY_KEY, key);

            return _cacheManager.Get(keyName, () =>
            {
                var query = from x in _roleRepository.Table
                            orderby x.Id
                            where x.Key == key
                            select x;

                var role = query.FirstOrDefault();

                return role;
            });
        }

        /// <summary>
        /// Get All Roles
        /// </summary>
        /// <param name="userTypeId">The user type identifier.</param>
        /// <returns>Role Collection Item</returns>
        public virtual IList<Role> GetAllRoles(UserTypes? userType, bool withUsers = false, short systemRole = -1)
        {
            string key = string.Format(CACHE_ROLE_ALL_BY_USER_TYPE_KEY, userType, withUsers, systemRole);

            return _cacheManager.Get(key, () =>
            {
                    return (_roleRepository.Table.Where(x =>
                                ((userType.HasValue && x.UserType == userType) || !userType.HasValue) &&
                                    (systemRole == -1 || 
                                     (systemRole == 0 && ((x.IsSystemRole.HasValue && !x.IsSystemRole.Value) || !x.IsSystemRole.HasValue)) ||
                                     (systemRole == 1 && x.IsSystemRole.HasValue && x.IsSystemRole.Value)
                                    )  
                              )
                        .OrderBy(x => x.Name)
                        .ThenBy(x => x.Id)).ToList();
            });
        }

        /// <summary>
        /// Insert Role
        /// </summary>
        /// <param name="role">Role</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">role</exception>
        public virtual long InsertRole(Role role)
        {
            if (role == null)
                throw new ArgumentNullException("role");

            _roleRepository.Insert(role);

            _cacheManager.RemoveByPattern(CACHE_ROLE_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(role);

            return role.Id;
        }

        /// <summary>
        /// Update Role
        /// </summary>
        /// <param name="role">Role</param>
        /// <exception cref="ArgumentNullException">role</exception>
        public virtual void UpdateRole(Role role)
        {
            if (role == null)
                throw new ArgumentNullException("role");

            _roleRepository.Update(role);

            _cacheManager.RemoveByPattern(CACHE_ROLE_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityUpdated(role);
        }

        /// <summary>
        /// Delete Role
        /// </summary>
        /// <param name="role">Role</param>
        /// <param name="isPermanently">Is Permanently</param>
        /// <exception cref="ArgumentNullException">role</exception>
        /// <exception cref="AppException">Buil-In user account ({0}) could not be deleted</exception>
        public virtual void DeleteRole(Role role, bool isPermanently = false)
        {
            if (role == null)
                throw new ArgumentNullException("role");

            if (role.BuiltIn)
                throw new AppException("Buil-In user account ({0}) could not be deleted", role.Name);

            if (!isPermanently)
            {
                role.Deleted = true;
                role.Active = false;

                if (!String.IsNullOrEmpty(role.Name))
                    role.Name += "-DELETED";

                if (!String.IsNullOrEmpty(role.Description))
                    role.Description += "-DELETED";

                UpdateRole(role);
            }
            else
            {
                _roleRepository.Delete(role);

                _cacheManager.RemoveByPattern(CACHE_ROLE_PATTERN_KEY);

                _appEventPublisher.EntityDeleted(role);
            }
        }

        #endregion Methods

        #region private methods
        private void CreateSystemRoles(string roleName, UserTypes userType, string userKind, IPermissionService permissionService, IList<Permission> permissions)
        {
            var role = GetRoleByKey(roleName);
            if (role == null)
            {
                role = new Role
                {
                    Id = 0,
                    Name = roleName,
                    Key = roleName,
                    Deleted = false,
                    BuiltIn = false,
                    UserType = userType,
                    Active = true,
                    IsSystemRole = true
                };
                var roleId = InsertRole(role);


                foreach (var p in permissions)
                {
                    if (p.Key.Contains(userKind) || userKind == "all")
                    {
                        permissionService.AddRolePermission(roleId, p.Id, state: (byte)1);
                    }
                }
            }
        }
        #endregion
    }
}