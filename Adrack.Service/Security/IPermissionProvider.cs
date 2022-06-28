// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IPermissionProvider.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Security;
using System.Collections.Generic;

namespace Adrack.Service.Security
{
    /// <summary>
    /// Represents a Permission Provider
    /// </summary>
    public partial interface IPermissionProvider
    {
        #region Methods

        /// <summary>
        /// Get Permissions
        /// </summary>
        /// <returns>Permission Collection Item</returns>
        IEnumerable<Permission> GetPermissions();

        #endregion Methods
    }
}