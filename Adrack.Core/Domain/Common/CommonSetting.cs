// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="CommonSetting.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Infrastructure.Configuration;

namespace Adrack.Core.Domain.Common
{
    /// <summary>
    /// Represents a Common Setting
    /// Implements the <see cref="Adrack.Core.Infrastructure.Configuration.ISetting" />
    /// </summary>
    /// <seealso cref="Adrack.Core.Infrastructure.Configuration.ISetting" />
    public class CommonSetting : ISetting
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the Sitemap Enabled
        /// </summary>
        /// <value><c>true</c> if [sitemap enabled]; otherwise, <c>false</c>.</value>
        public bool SitemapEnabled { get; set; }

        /// <summary>
        /// Gets or Sets the Sitemap Include Content
        /// </summary>
        /// <value><c>true</c> if [sitemap include content]; otherwise, <c>false</c>.</value>
        public bool SitemapIncludeContent { get; set; }

        /// <summary>
        /// Gets or Sets the Breadcrumb Delimiter
        /// </summary>
        /// <value>The breadcrumb delimiter.</value>
        public string BreadcrumbDelimiter { get; set; }

        #endregion Properties
    }
}