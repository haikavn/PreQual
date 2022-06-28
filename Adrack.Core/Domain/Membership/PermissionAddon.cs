// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 05-04-2021
//
// Last Modified By : Grigori
// Last Modified On : 05-04-2021
// ***********************************************************************
// <copyright file="PermissionAddon.cs" company="Adrack.com">
//     Copyright © 2021
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Security;
using System;
using System.Collections.Generic;

namespace Adrack.Core.Domain.Membership
{
    /// <summary>
    /// Represents a PermissionAddon
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public class PermissionAddon : BaseEntity
    {
        #region Properties

        /// <summary>
        /// Gets or sets the permission identifier.
        /// </summary>
        /// <value>The permission identifier.</value>
        public long PermissionId { get; set; }

        /// <summary>
        /// Gets or sets the Addon identifier.
        /// </summary>
        /// <value>The Addon identifier.</value>
        public long AddonId { get; set; }
        
        #endregion Properties
    }
}