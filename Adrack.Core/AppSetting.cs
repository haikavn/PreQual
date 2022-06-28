// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="AppSetting.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Infrastructure.Configuration;

namespace Adrack.Core
{
    /// <summary>
    /// Represents a Application Setting
    /// Implements the <see cref="Adrack.Core.Infrastructure.Configuration.ISetting" />
    /// </summary>
    /// <seealso cref="Adrack.Core.Infrastructure.Configuration.ISetting" />
    public class AppSetting : ISetting
    {
        #region Properties

        /// <summary>
        /// Gets the Version
        /// </summary>
        /// <value>The version.</value>
        public static string Version
        {
            get
            {
                return "1.0.0";
            }
        }

        /// <summary>
        /// Gets or Sets the Name
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets the Url
        /// </summary>
        /// <value>The URL.</value>
        public string Url { get; set; }

        /// <summary>
        /// Gets or Sets the Host
        /// </summary>
        /// <value>The host.</value>
        public string Host { get; set; }

        /// <summary>
        /// Gets or Sets the Secure Url
        /// </summary>
        /// <value>The secure URL.</value>
        public string SecureUrl { get; set; }

        /// <summary>
        /// Gets or Sets the Ssl Enabled
        /// </summary>
        /// <value><c>true</c> if [SSL enabled]; otherwise, <c>false</c>.</value>
        public bool SslEnabled { get; set; }

        /// <summary>
        /// Gets or Sets the Site Maintenance
        /// </summary>
        /// <value><c>true</c> if [site maintenance]; otherwise, <c>false</c>.</value>
        public bool SiteMaintenance { get; set; }

        /// <summary>
        /// Gets or Sets the value indicating whether responsive design supported (a graphical theme should also support it)
        /// </summary>
        /// <value><c>true</c> if [responsive design supported]; otherwise, <c>false</c>.</value>
        public bool ResponsiveDesignSupported { get; set; }

        /// <summary>
        /// Gets or Sets the value of Facebook page URL of the site
        /// </summary>
        /// <value>The facebook link.</value>
        public string FacebookLink { get; set; }

        /// <summary>
        /// Gets or Sets the value of Twitter page URL of the site
        /// </summary>
        /// <value>The twitter link.</value>
        public string TwitterLink { get; set; }

        /// <summary>
        /// Gets or Sets the value of YouTube channel URL of the site
        /// </summary>
        /// <value>The youtube link.</value>
        public string YoutubeLink { get; set; }

        #endregion Properties
    }
}