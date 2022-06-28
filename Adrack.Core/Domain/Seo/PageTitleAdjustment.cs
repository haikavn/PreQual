// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="PageTitleAdjustment.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Adrack.Core.Domain.Seo
{
    /// <summary>
    /// Represents a Page Title Adjustment Enumeration
    /// </summary>
    public enum PageTitleAdjustment
    {
        #region Enumeration

        /// <summary>
        /// Page Name After Application Name
        /// </summary>
        PageNameAfterAppName = 100,

        /// <summary>
        /// Application Name After Page Name
        /// </summary>
        AppNameAfterPageName = 200

        #endregion Enumeration
    }
}