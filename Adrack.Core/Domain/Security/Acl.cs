// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="Acl.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Adrack.Core.Domain.Security
{
    /// <summary>
    /// Represents a Access Control Level
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class Acl : BaseEntity
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the Role Identifier
        /// </summary>
        /// <value>The role identifier.</value>
        public long RoleId { get; set; }

        /// <summary>
        /// Gets or Sets the Entity Identifier
        /// </summary>
        /// <value>The entity identifier.</value>
        public long EntityId { get; set; }

        /// <summary>
        /// Gets or Sets the Entity Name
        /// </summary>
        /// <value>The name of the entity.</value>
        public string EntityName { get; set; }

        #endregion Properties

        #region Navigation Properties

        /// <summary>
        /// Gets or Sets the Role
        /// </summary>
        /// <value>The role.</value>
        public virtual Role Role { get; set; }

        #endregion Navigation Properties
    }
}