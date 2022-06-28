// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="PermissionBuiltIn.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;

namespace Adrack.Core.Domain.Security
{
    /// <summary>
    /// Represents a Permission Built In
    /// </summary>
    public class PermissionBuiltIn
    {
        #region Constructor

        /// <summary>
        /// Permission Built In
        /// </summary>
        public PermissionBuiltIn()
        {
            this.Permissions = new List<Permission>();
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Gets or Sets the Role Name
        /// </summary>
        /// <value>The name of the role.</value>
        public string RoleName { get; set; }

        /// <summary>
        /// Gets or Sets the Role Key
        /// </summary>
        /// <value>The role key.</value>
        public string RoleKey { get; set; }

        /// <summary>
        /// Gets or Sets the Permissions
        /// </summary>
        /// <value>The permissions.</value>
        public IEnumerable<Permission> Permissions { get; set; }

        #endregion Properties
    }
}