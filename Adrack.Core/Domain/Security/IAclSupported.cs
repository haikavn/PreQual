// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="IAclSupported.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Adrack.Core.Domain.Security
{
    /// <summary>
    /// Represents a Access Control Level Supported
    /// </summary>
    public partial interface IAclSupported
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the Subject To Acl
        /// </summary>
        /// <value><c>true</c> if [subject to acl]; otherwise, <c>false</c>.</value>
        bool SubjectToAcl { get; set; }

        #endregion Properties
    }
}