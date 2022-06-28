// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="UserRegistrationType.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Adrack.Core.Domain.Security
{
    /// <summary>
    /// Role permission state enum
    /// </summary>
    public enum PermissionState: byte
    {
        /// <summary>
        /// no access
        /// </summary>
        NoAccess = 0,
        /// <summary>
        /// Access
        /// </summary>
        Access = 1
    }
}

