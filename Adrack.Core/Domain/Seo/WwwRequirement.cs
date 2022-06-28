// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="WwwRequirement.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Adrack.Core.Domain.Seo
{
    /// <summary>
    /// Represents a World Wide Web Requirement Enumeration
    /// </summary>
    public enum WwwRequirement
    {
        #region Enumeration

        /// <summary>
        /// Use WWW prefix In Page
        /// </summary>
        WithWww = 100,

        /// <summary>
        /// Do Not Use WWW prefix In Page
        /// </summary>
        WithoutWww = 200,

        /// <summary>
        /// It Does Not Matter To  Use WWW prefix In Page
        /// </summary>
        NoMatter = 300

        #endregion Enumeration
    }
}