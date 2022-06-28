// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IRoleService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Domain.Security;
using System.Collections.Generic;

namespace Adrack.Service.Security
{
    /// <summary>
    /// Represents a Role Service
    /// </summary>
    public partial interface IRoleService
    {
        #region Methods

        /// <summary>
        /// Generate System Roles
        /// </summary>
        void GenerateSystemRoles();

        /// <summary>
        /// Get Role By Id
        /// </summary>
        /// <param name="roleId">Role Identifier</param>
        /// <returns>Role Item</returns>
        Role GetRoleById(long roleId);

        /// <summary>
        /// Get Role By Name
        /// </summary>
        /// <param name="name">Name</param>
        /// <returns>Role Item</returns>
        Role GetRoleByName(string name);

        /// <summary>
        /// Get Role By Name
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Role Item</returns>
        Role GetRoleByKey(string key);

        /// <summary>
        /// Get All Roles
        /// </summary>
        /// <param name="userTypeId">The user type identifier.</param>
        /// <param name="withUsers"></param>
        /// <param name="systemRole"></param>
        /// <returns>Role Collection Item</returns>
        IList<Role> GetAllRoles(UserTypes? userType = null, bool withUsers = false, short systemRole = -1);

        /// <summary>
        /// Insert Role
        /// </summary>
        /// <param name="role">Role</param>
        /// <returns>System.Int64.</returns>
        long InsertRole(Role role);

        /// <summary>
        /// Update Role
        /// </summary>
        /// <param name="role">Role</param>
        void UpdateRole(Role role);

        /// <summary>
        /// Delete Role
        /// </summary>
        /// <param name="role">Role</param>
        /// <param name="isPermanently">Is Permanently</param>
        void DeleteRole(Role role, bool isPermanently = false);

        #endregion Methods
    }
}