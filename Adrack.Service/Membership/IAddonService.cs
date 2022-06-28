// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-09-2021
//
// Last Modified By : Grigori D.
// Last Modified On : 03-09-2021
// ***********************************************************************
// <copyright file="IAddonService.cs" company="Adrack.com">
//     Copyright © 2021
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Membership;
using System;
using System.Collections.Generic;

namespace Adrack.Service.Membership
{
    /// <summary>
    /// Represents a Profile Service
    /// </summary>
    public partial interface IAddonService
    {
        #region Methods

        /// <summary>
        /// Get All Addons
        /// </summary>
        /// <param name="deleted">The deleted.</param>
        /// <returns>Addons Collection Item</returns>
        IList<Addon> GetAllAddons(short deleted = 0);

        /// <summary>
        /// Get Addon By Id
        /// </summary>
        /// <param name="id">Addon Id</param>
        /// <returns>Addon Item</returns>
        Addon GetAddonById(long id);

        /// <summary>
        /// Insert Addon
        /// </summary>
        /// <param name="addon">The addon.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">addon</exception>
        long InsertAddon(Addon addon);

        /// <summary>
        /// Update Addon
        /// </summary>
        /// <param name="addon">The addon.</param>
        /// <exception cref="ArgumentNullException">addon</exception>
        void UpdateAddon(Addon addon);


        /// <summary>
        /// Delete Addon
        /// </summary>
        /// <param name="addon">The addon.</param>
        /// <exception cref="ArgumentNullException">addon</exception>
        void DeleteAddon(Addon addon);



        /// <summary>
        /// Get All User Addons
        /// </summary>
        /// <returns>UserAddons Collection Item</returns>
        IList<UserAddons> GetAllUserAddons();

        /// <summary>
        /// Get Addons By UserId
        /// </summary>
        /// <returns>UserAddons Collection Item</returns>
        IList<UserAddons> GetAddonsByUserId(long id);

        /// <summary>
        /// Get Addons By UserId
        /// </summary>
        /// <returns>UserAddons Collection Item</returns>
        IList<Addon> GetActivatedAddonsByUserId(long id);


        /// <summary>
        /// Get UserAddon
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="addonId">Addon Id</param>
        /// <returns>UserAddon Item</returns>
        UserAddons GetUserAddon(long userId, long addonId);

        /// <summary>
        /// Update UserAddon
        /// </summary>
        /// <param name="userAddon">The userAddon.</param>
        /// <exception cref="ArgumentNullException">userAddon</exception>
        void UpdateUserAddon(UserAddons userAddon);

        /// <summary>
        /// Insert User Addon
        /// </summary>
        /// <param name="userAddon">The user's addon.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">userAddon</exception>
        long InsertUserAddon(UserAddons userAddon);


        /// <summary>
        /// Delete User Addon
        /// </summary>
        /// <param name="userAddon">The user's addon.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">userAddon</exception>
        void DeleteUserAddon(UserAddons userAddon);

        #endregion Methods


        #region Methods PermissionAddon

        /// <summary>
        /// Get All Permission Addons
        /// </summary>
        /// <returns>PermissionAddon Collection Item</returns>
        IList<PermissionAddon> GetAllPermissionAddons();


        /// <summary>
        /// Get Addons By PermissionId
        /// </summary>
        /// <returns>PermissionAddon Collection Item</returns>
        IList<PermissionAddon> GetAddonsByPermissionId(long id);

        IList<long> GetPermissionIdsByAddonId(long id);


        /// <summary>
        /// Get PermissionAddon
        /// </summary>
        /// <param name="permissionId">Permission Id</param>
        /// <param name="addonId">Addon Id</param>
        /// <returns>PermissionAddon Item</returns>
        PermissionAddon GetPermissionAddon(long permissionId, long addonId);


        /// <summary>
        /// Insert Permission Addon
        /// </summary>
        /// <param name="permissionAddon">The permission's addon.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">permissionAddon</exception>
        long InsertPermissionAddon(PermissionAddon permissionAddon);


        /// <summary>
        /// Delete Permission Addon
        /// </summary>
        /// <param name="permissionAddon">The permission's addon.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">permissionAddon</exception>
        void DeletePermissionAddon(PermissionAddon permissionAddon);



        #endregion Methods
    }
}