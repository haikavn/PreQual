// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="Role.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Directory;
using Adrack.Core.Domain.Membership;
using System.Collections.Generic;

namespace Adrack.Core.Domain.Security
{
    /// <summary>
    /// Represents a Role
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class Role : BaseEntity
    {
        #region Fields

        /// <summary>
        /// User
        /// </summary>
        private ICollection<User> _users;

        /// <summary>
        /// RolePermission
        /// </summary>
        private ICollection<RolePermission> _rolePermissions;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or Sets the Name
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets the Key
        /// </summary>
        /// <value>The key.</value>
        public string Key { get; set; }

        /// <summary>
        /// Gets or Sets the Active
        /// </summary>
        /// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or Sets the Deleted
        /// </summary>
        /// <value><c>true</c> if deleted; otherwise, <c>false</c>.</value>
        public bool Deleted { get; set; }

        /// <summary>
        /// Gets or Sets the Built In
        /// </summary>
        /// <value><c>true</c> if [built in]; otherwise, <c>false</c>.</value>
        public bool BuiltIn { get; set; }

        /// <summary>
        /// Gets or Sets the Description
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        public UserTypes UserType { get; set; }

        /// <summary>
        /// Gets or Sets the IsSystemRole
        /// </summary>
        public bool? IsSystemRole { get; set; }

        #endregion Properties

        #region Navigation Properties

        /// <summary>
        /// Gets or Sets the User
        /// </summary>
        /// <value>The users.</value>
        public virtual ICollection<User> Users
        {
            get { return _users ?? (_users = new List<User>()); }
            protected set { _users = value; }
        }


        public virtual ICollection<RolePermission> RolePermissions
        {
            get { return _rolePermissions ?? (_rolePermissions = new List<RolePermission>()); }
            protected set { _rolePermissions = value; }
        }

        #endregion Navigation Properties
    }
}