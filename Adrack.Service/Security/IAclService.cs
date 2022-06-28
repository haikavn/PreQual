// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IAclService.cs" company="Adrack.com">
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
    /// Represents a Access Control List Service
    /// </summary>
    public partial interface IAclService
    {
        #region Methods

        /// <summary>
        /// Get Acl By Id
        /// </summary>
        /// <param name="aclId">Acl Identifier</param>
        /// <returns>Acl Item</returns>
        Acl GetAclById(long aclId);

        /// <summary>
        /// Get User Role Ids
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns>Long Item</returns>
        long[] GetUserRoleIds<T>(T entity) where T : BaseEntity, IAclSupported;

        /// <summary>
        /// Get All Acls
        /// </summary>
        /// <returns>Acl Collection Item</returns>
        IList<Acl> GetAllAcls();

        /// <summary>
        /// Get Acls
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns>Acl Collection Item</returns>
        IList<Acl> GetAcls<T>(T entity) where T : BaseEntity, IAclSupported;

        /// <summary>
        /// Insert Acl
        /// </summary>
        /// <param name="acl">Acl</param>
        void InsertAcl(Acl acl);

        /// <summary>
        /// Insert Acl
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="roleId">Role Identifier</param>
        void InsertAcl<T>(T entity, long roleId) where T : BaseEntity, IAclSupported;

        /// <summary>
        /// Update Acl
        /// </summary>
        /// <param name="acl">Acl</param>
        void UpdateAcl(Acl acl);

        /// <summary>
        /// Delete Acl
        /// </summary>
        /// <param name="acl">Acl</param>
        void DeleteAcl(Acl acl);

        #region Authorize

        /// <summary>
        /// Authorize
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns>Boolean Item</returns>
        bool Authorize<T>(T entity) where T : BaseEntity, IAclSupported;

        /// <summary>
        /// Authorize
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="user">User</param>
        /// <returns>Boolean Item</returns>
        bool Authorize<T>(T entity, User user) where T : BaseEntity, IAclSupported;

        #endregion Authorize

        #endregion Methods
    }
}