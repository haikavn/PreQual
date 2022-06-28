// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="PermissionService.cs" company="Adrack.com">
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
using Adrack.Service.Helpers;
using Adrack.Service.Localization;
using Adrack.Service.Membership;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Adrack.Service.Security
{
    /// <summary>
    /// Represents a Permission Service
    /// Implements the <see cref="Adrack.Service.Security.IPermissionService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Security.IPermissionService" />
    public partial class PermissionService : IPermissionService
    {
        #region Constants

        /// <summary>
        /// Cache Permission By Id Key
        /// </summary>
        private const string CACHE_PERMISSION_BY_ID_KEY = "App.Cache.Permission.By.Id-{0}";

        /// <summary>
        /// Cache Permission By Key Key
        /// </summary>
        private const string CACHE_PERMISSION_BY_KEY_KEY = "App.Cache.Permission.By.Key-{0}";

        /// <summary>
        /// Cache Permission By Role Id, Key Key
        /// </summary>
        private const string CACHE_PERMISSION_BY_USER_ID_KEY_KEY = "App.Cache.Permission.By.User.Id-Key-{0}-{1}";

        /// <summary>
        /// Cache Permission By Role Id, Key Key
        /// </summary>
        private const string CACHE_PERMISSION_BY_ROLE_ID_KEY_KEY = "App.Cache.Permission.By.Role.Id-Key-{0}-{1}";

        private const string CACHE_PERMISSION_BY_PARENT_ID = "App.Cache.Permission.By.ParentId-{0}";


        /// <summary>
        /// Cache Permission All Key
        /// </summary>
        private const string CACHE_PERMISSION_ALL_KEY = "App.Cache.Permission.All";

        /// <summary>
        /// Cache Permission Pattern Key
        /// </summary>
        private const string CACHE_PERMISSION_PATTERN_KEY = "App.Cache.Permission.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Permission
        /// </summary>
        private readonly IRepository<Permission> _permissionRepository;

        /// <summary>
        /// Role Permission
        /// </summary>
        private readonly IRepository<RolePermission> _rolePermissionRepository;

        /// <summary>
        /// User Service
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// Role Service
        /// </summary>
        private readonly IRoleService _roleService;

        /// <summary>
        /// Locale String Resource Service
        /// </summary>
        private readonly ILocalizedStringService _localeStringResourceService;

        /// <summary>
        /// Language Service
        /// </summary>
        private readonly ILanguageService _languageService;

        /// <summary>
        /// Application Context
        /// </summary>
        private readonly IAppContext _appContext;

        /// <summary>
        /// Cache Manager
        /// </summary>
        private readonly ICacheManager _cacheManager;

        /// <summary>
        /// The data provider
        /// </summary>
        private readonly IDataProvider _dataProvider;

        #endregion Fields

        #region Utilities

        /// <summary>
        /// Authorize
        /// </summary>
        /// <param name="permissionKey">Permission Key</param>
        /// <param name="role">Role</param>
        /// <returns>Boolean Item</returns>
        protected virtual bool Authorize(string permissionKey, Role role, PermissionState state)
        {
            if (String.IsNullOrEmpty(permissionKey))
                return false;

            string key = string.Format(CACHE_PERMISSION_BY_ROLE_ID_KEY_KEY, role.Id, permissionKey);

            foreach (var permission in role.RolePermissions)
            {
                if (permission.Permission.Key.Equals(permissionKey, StringComparison.InvariantCultureIgnoreCase) && 
                                                                            permission.State == (byte)state)
                    return true;
            }

            return false;
        }

        #endregion Utilities

        #region Constructor

        /// <summary>
        /// Permission Service
        /// </summary>
        /// <param name="permissionRepository">Permission Repository</param>
        /// <param name="rolePermissionRepository">Role Permission Repository</param>
        /// <param name="userService">User Service</param>
        /// <param name="roleService">Role Service</param>
        /// <param name="dataProvider">The data provider.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="localeStringResourceService">Locale String Resource Service</param>
        /// <param name="languageService">Language Service</param>
        /// <param name="appContext">Application Context</param>
        /// <param name="cacheManager">Cache Manager</param>

        public PermissionService(IRepository<Permission> permissionRepository, IRepository<RolePermission> rolePermissionRepository, IUserService userService, IRoleService roleService, IDataProvider dataProvider, ILocalizedStringService localeStringResourceService, ILanguageService languageService, IAppContext appContext, ICacheManager cacheManager)
        {
            this._permissionRepository = permissionRepository;
            this._rolePermissionRepository = rolePermissionRepository;
            this._userService = userService;
            this._roleService = roleService;
            this._localeStringResourceService = localeStringResourceService;
            this._languageService = languageService;
            this._appContext = appContext;
            this._cacheManager = cacheManager;
            this._dataProvider = dataProvider;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get Permission By Id
        /// </summary>
        /// <param name="permissionId">Permission Identifier</param>
        /// <returns>Permission Item</returns>
        public virtual Permission GetPermissionById(long permissionId)
        {
            if (permissionId == 0)
                return new Permission();

            return _permissionRepository.GetById(permissionId);
        }

        /// <summary>
        /// Get Permission By Key
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Permission Item</returns>
        public virtual Permission GetPermissionByKey(string key)
        {
            if (String.IsNullOrWhiteSpace(key))
                return null;

            string permissionKey = string.Format(CACHE_PERMISSION_BY_KEY_KEY, key);

            return _cacheManager.Get(permissionKey, () =>
            {
                var query = from x in _permissionRepository.Table
                            where x.Key == key
                            orderby x.Name, x.Id
                            select x;

                var permissions = query.FirstOrDefault();

                return permissions;
            });
        }

        /// <summary>
        /// Get All Permissions
        /// </summary>
        /// <returns>Permission Collection Item</returns>
        public virtual IList<Permission> GetAllPermissions()
        {
            string key = CACHE_PERMISSION_ALL_KEY;

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _permissionRepository.Table
                            orderby x.EntityName, x.Order
                            select x;

                var permissions = query.ToList();

                return permissions;
            });
        }

        public virtual IList<Permission> GetAllPermissions(long parentId)
        {
            string key = string.Format(CACHE_PERMISSION_BY_PARENT_ID, parentId);
            var permissions = new List<Permission>();
            return _cacheManager.Get(key, () =>
            {
                var query = from x in _permissionRepository.Table
                            where x.ParentId == parentId
                            orderby x.EntityName, x.Order
                            select x;

                permissions = query.ToList();

                return permissions;
            });
        }

        /// <summary>
        /// Insert Permission
        /// </summary>
        /// <param name="permission">Permission</param>
        /// <exception cref="ArgumentNullException">permission</exception>
        public virtual void InsertPermission(Permission permission)
        {
            if (permission == null)
                throw new ArgumentNullException("permission");

            _permissionRepository.Insert(permission);

            _cacheManager.RemoveByPattern(CACHE_PERMISSION_PATTERN_KEY);
        }

        /// <summary>
        /// Update Permission
        /// </summary>
        /// <param name="permission">Permission</param>
        /// <exception cref="ArgumentNullException">permission</exception>
        public virtual void UpdatePermission(Permission permission)
        {
            if (permission == null)
                throw new ArgumentNullException("permission");

            _permissionRepository.Update(permission);

            _cacheManager.RemoveByPattern(CACHE_PERMISSION_PATTERN_KEY);
        }

        /// <summary>
        /// Delete Permission
        /// </summary>
        /// <param name="permission">Permission</param>
        /// <param name="isPermanently">Is Permanently</param>
        /// <exception cref="ArgumentNullException">permission</exception>
        public virtual void DeletePermission(Permission permission, bool isPermanently = false)
        {
            if (permission == null)
                throw new ArgumentNullException("permission");

            if (!isPermanently)
            {
                permission.Deleted = true;
                permission.Active = false;

                if (!String.IsNullOrEmpty(permission.Name))
                    permission.Name += "-DELETED";

                if (!String.IsNullOrEmpty(permission.Description))
                    permission.Description += "-DELETED";

                UpdatePermission(permission);
            }
            else
            {
                _permissionRepository.Delete(permission);

                _cacheManager.RemoveByPattern(CACHE_PERMISSION_PATTERN_KEY);
            }
        }

        #region Authorize

        /// <summary>
        /// Authorize
        /// </summary>
        /// <param name="permission">Permission</param>
        /// <returns>Boolean Item</returns>
        public virtual bool Authorize(Permission permission, PermissionState state)
        {
            return Authorize(permission, _appContext.AppUser, state);
        }

        /// <summary>
        /// Authorize
        /// </summary>
        /// <param name="permission">Permission</param>
        /// <param name="user">User</param>
        /// <returns>Boolean Item</returns>
        public virtual bool Authorize(Permission permission, User user, PermissionState state)
        {
            if (permission == null)
                return false;

            if (user == null)
                return false;

            if (!permission.Active && permission.Deleted)
                return false;

            return Authorize(permission.Key, user, state);
        }

        /// <summary>
        /// Authorize
        /// </summary>
        /// <param name="permissionKey">Permission Key</param>
        /// <returns>Boolean Item</returns>
        public virtual bool Authorize(string permissionKey, PermissionState state)
        {
            return Authorize(permissionKey, _appContext.AppUser, state);
        }

        /// <summary>
        /// Authorize
        /// </summary>
        /// <param name="permissionKey">Permission Key</param>
        /// <param name="user">User</param>
        /// <returns>Boolean Item</returns>
        public virtual bool Authorize(string permissionKey, User user, PermissionState state)
        {
            if (user == null) return false;
            if (user.UserType == UserTypes.Super) return true;

            if (String.IsNullOrEmpty(permissionKey))
                return false;

            var roles = user.Roles.Where(x => x.Active && !x.Deleted);

            string key = string.Format(CACHE_PERMISSION_BY_USER_ID_KEY_KEY, user.Id, permissionKey);

            foreach (var role in roles)
                if (Authorize(permissionKey, role, state))
                    return true;

            foreach (var permission in user.Permissions)
                if (permission.Key.Equals(permissionKey, StringComparison.InvariantCultureIgnoreCase))
                    return true;

            return false;
        }

        /// <summary>
        /// Clear Role Permissions
        /// </summary>
        /// <param name="roleid">The roleid.</param>
        public virtual void ClearRolePermissions(long roleid)
        {
            var items = _rolePermissionRepository.Table.Where(x => x.RoleId == roleid).ToList();

            foreach (var item in items)
                _rolePermissionRepository.Delete(item);

            /*
            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "roleid";
            startParam.Value = roleid;
            startParam.DbType = DbType.Int64;

            _dbContext.SqlQuery<int>("EXECUTE [dbo].[ClearRolePermissions] @roleid", startParam).FirstOrDefault();
            */
        }



        /// <summary>
        /// Get RolePermission
        /// </summary>
        /// <param name="roleId">Role Identifier</param>
        /// <param name="permissionId">Permission Identifier</param>
        /// <returns>RolePermission Item</returns>
        public virtual RolePermission GetRolePermission(long roleId, long permissionId)
        {
            var query = from x in _rolePermissionRepository.Table
                where x.RoleId == roleId && x.PermissionId == permissionId
                select x;

            return query.FirstOrDefault();
        }

        public List<RolePermission> GetRolePermissionsByPermissionId(long permissionId)
        {
            var query = from x in _rolePermissionRepository.Table
                        where x.PermissionId == permissionId
                        select x;

            return query.ToList();
        }

        public List<Permission> GetPermissionsByRoleId(long roleId)
        {
            var query = from p in _permissionRepository.Table
                join pr in _rolePermissionRepository.Table on p.Id equals pr.PermissionId
                        where pr.RoleId == roleId && p.Active && !p.Deleted
                        select p;

            return query.ToList();
        }

        /// <summary>
        /// Add Role Permission
        /// </summary>
        /// <param name="roleid">The roleid.</param>
        /// <param name="permissionid">The permissionid.</param>
        public virtual void AddRolePermission(long roleid, long permissionid, byte state)
        {
            var rolePermission = new RolePermission()
            {
                RoleId = roleid,
                PermissionId = permissionid,
                State = state
            };

            _rolePermissionRepository.Insert(rolePermission);

            /*
            var roleIdParam = _dataProvider.GetParameter();
            roleIdParam.ParameterName = "roleid";
            roleIdParam.Value = roleid;
            roleIdParam.DbType = DbType.Int64;

            var permissionIdParam = _dataProvider.GetParameter();
            permissionIdParam.ParameterName = "permissionid";
            permissionIdParam.Value = permissionid;
            permissionIdParam.DbType = DbType.Int64;

            var stateParam = _dataProvider.GetParameter();
            stateParam.ParameterName = "state";
            stateParam.Value = state;
            stateParam.DbType = DbType.Byte;

           _permissionRepository.GetDbClientContext().SqlQuery<int>("EXECUTE [dbo].[AddRolePermission] @roleid,@permissionid,@state", roleIdParam, permissionIdParam, stateParam).FirstOrDefault();
            */
        }

        /// <summary>
        /// Get RolePermission 
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="permissionId"></param>
        /// <param name="state"></param>
        public virtual void UpdateRolePermission(long roleId, long permissionId, byte state)
        {
            var rolePermission = GetRolePermission(roleId, permissionId);
            if (rolePermission == null)
                throw new ArgumentNullException("RolePermission");

            rolePermission.State = state;
            _rolePermissionRepository.Update(rolePermission);

            /*
            var roleIdParam = _dataProvider.GetParameter();
            roleIdParam.ParameterName = "roleid";
            roleIdParam.Value = roleId;
            roleIdParam.DbType = DbType.Int64;

            var permissionIdParam = _dataProvider.GetParameter();
            permissionIdParam.ParameterName = "permissionid";
            permissionIdParam.Value = permissionId;
            permissionIdParam.DbType = DbType.Int64;

            var stateParam = _dataProvider.GetParameter();
            stateParam.ParameterName = "state";
            stateParam.Value = state;
            stateParam.DbType = DbType.Byte;

            _permissionRepository.GetDbClientContext().SqlQuery<int>("EXECUTE [dbo].[UpdateRolePermissionState] @roleid,@permissionid,@state", roleIdParam, permissionIdParam, stateParam).FirstOrDefault();
            */
        }

        /// <summary>
        /// Check Navigation Permissions
        /// </summary>
        /// <param name="permissions">The permissions.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public virtual bool CheckNavigationPermissions(string permissions, PermissionState state)
        {
            if (_appContext.AppUser != null && (_appContext.AppUser.UserType == SharedData.BuiltInUserTypeId /*AZ || _appContext.AppUser.UserTypeId == SharedData.NetowrkUserTypeId */ )) return true;

            string[] str = permissions.Split(new char[1] { ',' });

            List<long> str1 = new List<long>();

            for (int i = 0; i < str.Length; i++)
            {
                if (!str[i].Contains("!"))
                {
                    Permission p = this.GetPermissionById(long.Parse(str[i].Replace(" ", "").Replace("!", "")));
                    if (!this.Authorize(p, state)) return false;
                }
                else
                {
                    str1.Add(long.Parse(str[i].Replace(" ", "").Replace("!", "")));
                }
            }

            if (str1.Count > 0)
            {
                for (int i = 0; i < str1.Count; i++)
                {
                    Permission p = this.GetPermissionById(str1[i]);
                    if (this.Authorize(p, state)) return true;
                }

                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets the application context.
        /// </summary>
        /// <returns>IAppContext.</returns>
        public virtual IAppContext GetAppContext()
        {
            return _appContext;
        }

        #endregion Authorize

        #endregion Methods
    }
}