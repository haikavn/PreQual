// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IPermissionService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Domain.Security;
using System.Collections.Generic;

namespace Adrack.Service.Security
{
    /// <summary>
    /// Represents a Permission Service
    /// </summary>
    public partial interface IPermissionService
    {
        #region Methods

        /// <summary>
        /// Get Permission By Id
        /// </summary>
        /// <param name="permissionId">Permission Identifier</param>
        /// <returns>Permission Item</returns>
        Permission GetPermissionById(long permissionId);

        /// <summary>
        /// Get Permission By Key
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Permission Item</returns>
        Permission GetPermissionByKey(string key);

        /// <summary>
        /// Get All Permissions
        /// </summary>
        /// <returns>Permission Collection Item</returns>
        IList<Permission> GetAllPermissions();

        IList<Permission> GetAllPermissions(long parentId);


        /// <summary>
        /// Insert Permission
        /// </summary>
        /// <param name="permission">Permission</param>
        void InsertPermission(Permission permission);

        /// <summary>
        /// Update Permission
        /// </summary>
        /// <param name="permission">Permission</param>
        void UpdatePermission(Permission permission);

        /// <summary>
        /// Delete Permission
        /// </summary>
        /// <param name="permission">Permission</param>
        /// <param name="isPermanently">Is Permanently</param>
        void DeletePermission(Permission permission, bool isPermanently = false);

        #region Authorize

        /// <summary>
        /// Authorize
        /// </summary>
        /// <param name="permission">Permission</param>
        /// <returns>Boolean Item</returns>
        bool Authorize(Permission permission, PermissionState state = PermissionState.Access);

        /// <summary>
        /// Authorize
        /// </summary>
        /// <param name="permission">Permission</param>
        /// <param name="user">User</param>
        /// <returns>Boolean Item</returns>
        bool Authorize(Permission permission, User user, PermissionState state = PermissionState.Access);

        /// <summary>
        /// Authorize
        /// </summary>
        /// <param name="permissionKey">Permission Key</param>
        /// <returns>Boolean Item</returns>
        bool Authorize(string permissionKey, PermissionState state = PermissionState.Access);

        /// <summary>
        /// Authorize
        /// </summary>
        /// <param name="permissionKey">Permission Key</param>
        /// <param name="user">User</param>
        /// <returns>Boolean Item</returns>
        bool Authorize(string permissionKey, User user, PermissionState state = PermissionState.Access);

        /// <summary>
        /// Clear Role Permissions
        /// </summary>
        /// <param name="roleid">The roleid.</param>
        void ClearRolePermissions(long roleid);

        /// <summary>
        /// Add Role Permission
        /// </summary>
        /// <param name="roleid">The roleid.</param>
        /// <param name="permissionid">The permissionid.</param>
        void AddRolePermission(long roleid, long permissionid, byte state);


        /// <summary>
        /// Get RolePermission
        /// </summary>
        /// <param name="roleId">Role Identifier</param>
        /// <param name="permissionId">Permission Identifier</param>
        /// <returns>RolePermission Item</returns>
        RolePermission GetRolePermission(long roleId, long permissionId);

        List<RolePermission> GetRolePermissionsByPermissionId(long permissionId);

        List<Permission> GetPermissionsByRoleId(long roleId);

        /// <summary>
        /// Update Role Permission State
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="permissionId"></param>
        /// <param name="state"></param>
        void UpdateRolePermission(long roleId, long permissionId, byte state);

        /// <summary>
        /// Check Navigation Permissions
        /// </summary>
        /// <param name="permissions">The permissions.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool CheckNavigationPermissions(string permissions, PermissionState state = PermissionState.Access);

        /// <summary>
        /// GetAppContext
        /// </summary>
        /// <returns>IAppContext.</returns>
        IAppContext GetAppContext();

        #endregion Authorize

        #endregion Methods
    }
}