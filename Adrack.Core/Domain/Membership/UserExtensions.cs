// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="UserExtensions.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Security;
using System;
using System.Linq;

namespace Adrack.Core.Domain.Membership
{
    /// <summary>
    /// Represents a User Extensions
    /// </summary>
    public static class UserExtensions
    {
        #region Methods

        /// <summary>
        /// Is User In Role
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="userRoleKey">User Role Key</param>
        /// <returns>Boolean Item</returns>
        /// <exception cref="ArgumentNullException">
        /// user
        /// or
        /// userRoleKey
        /// </exception>
        public static bool IsUserInRole(this User user, string userRoleKey)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (String.IsNullOrEmpty(userRoleKey))
                throw new ArgumentNullException("userRoleKey");

            var result = user.Roles.FirstOrDefault(x => (x.Active) && (!x.Deleted) && (x.Key == userRoleKey)) != null;

            return result;
        }

        /// <summary>
        /// Is User In Permission
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="userPermissionKey">User Permission Key</param>
        /// <returns>Boolean Item</returns>
        /// <exception cref="ArgumentNullException">
        /// user
        /// or
        /// userPermissionKey
        /// </exception>
        public static bool IsUserInPermission(this User user, string userPermissionKey)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (String.IsNullOrEmpty(userPermissionKey))
                throw new ArgumentNullException("userPermissionKey");

            var result = user.Permissions.FirstOrDefault(x => (x.Active) && (!x.Deleted) && (x.Key == userPermissionKey)) != null;

            return result;
        }

        /// <summary>
        /// Is Global Administrator
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Boolean Item</returns>
        public static bool IsGlobalAdministrator(this User user)
        {
            return IsUserInRole(user, RoleBuiltIn.GlobalAdministrators);
        }

        /// <summary>
        /// Is Content Manager
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Boolean Item</returns>
        public static bool IsContentManager(this User user)
        {
            return IsUserInRole(user, RoleBuiltIn.ContentManagers);
        }

        /// <summary>
        /// Is Network User
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Boolean Item</returns>
        public static bool IsNetworkUser(this User user)
        {
            return IsUserInRole(user, RoleBuiltIn.NetworkUsers);
        }

        #endregion Methods
    }
}