// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="SslRequirement.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Adrack.Web.Framework.Security
{
    /// <summary>
    /// Represents a Secure Sockets Layer Enumeration
    /// </summary>
    public enum SslRequirement
    {
        #region Enumeration

        /// <summary>
        /// Use Ssl In Page
        /// </summary>
        Yes,

        /// <summary>
        /// Do Not Use Ssl In Page
        /// </summary>
        No,

        /// <summary>
        /// It Does Not Matter To Use Ssl In Page
        /// </summary>
        NoMatter

        #endregion Enumeration
    }
}