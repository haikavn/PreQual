// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="UpdateFrequency.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Adrack.Service.Seo
{
    /// <summary>
    /// Represents a SEO Update Frequency Enumeration
    /// </summary>
    public enum UpdateFrequency
    {
        #region Enumeration

        /// <summary>
        /// Always
        /// </summary>
        Always,

        /// <summary>
        /// Hourly
        /// </summary>
        Hourly,

        /// <summary>
        /// Daily
        /// </summary>
        Daily,

        /// <summary>
        /// Weekly
        /// </summary>
        Weekly,

        /// <summary>
        /// Monthly
        /// </summary>
        Monthly,

        /// <summary>
        /// Yearly
        /// </summary>
        Yearly,

        /// <summary>
        /// Never
        /// </summary>
        Never

        #endregion Enumeration
    }
}