// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="LocalizationSetting.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Infrastructure.Configuration;

namespace Adrack.Core.Domain.Localization
{
    /// <summary>
    /// Represents a Localization Setting
    /// Implements the <see cref="Adrack.Core.Infrastructure.Configuration.ISetting" />
    /// </summary>
    /// <seealso cref="Adrack.Core.Infrastructure.Configuration.ISetting" />
    public class LocalizationSetting : ISetting
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the Primary Application Language Identifier
        /// </summary>
        /// <value>The primary application language identifier.</value>
        public long PrimaryAppLanguageId { get; set; }

        /// <summary>
        /// Gets or Sets the Content Management Language Identifier
        /// </summary>
        /// <value>The content management language identifier.</value>
        public long ContentManagementLanguageId { get; set; }

        /// <summary>
        /// Gets or Sets the Content Management RTL
        /// </summary>
        /// <value><c>true</c> if [content management RTL]; otherwise, <c>false</c>.</value>
        public bool ContentManagementRtl { get; set; }

        /// <summary>
        /// Gets or Sets the Use Image For Language Selection
        /// </summary>
        /// <value><c>true</c> if [use image for language selection]; otherwise, <c>false</c>.</value>
        public bool UseImageForLanguageSelection { get; set; }

        /// <summary>
        /// Gets or Sets the SEO friendly URLs with multiple languages are enabled
        /// </summary>
        /// <value><c>true</c> if [seo friendly urls for languages enabled]; otherwise, <c>false</c>.</value>
        public bool SeoFriendlyUrlsForLanguagesEnabled { get; set; }

        /// <summary>
        /// Gets or Sets the Detects the current language by a user region (browser settings)
        /// </summary>
        /// <value><c>true</c> if [automatically detect language]; otherwise, <c>false</c>.</value>
        public bool AutomaticallyDetectLanguage { get; set; }

        /// <summary>
        /// Gets or Sets the Load all Localized Strings on application startup
        /// </summary>
        /// <value><c>true</c> if [load all localized strings on startup]; otherwise, <c>false</c>.</value>
        public bool LoadAllLocalizedStringsOnStartup { get; set; }

        /// <summary>
        /// Gets or Sets the Load all Localized Property on application startup
        /// </summary>
        /// <value><c>true</c> if [load all localized properties on startup]; otherwise, <c>false</c>.</value>
        public bool LoadAllLocalizedPropertiesOnStartup { get; set; }

        /// <summary>
        /// Gets or Sets the Load all Page Slug search engine friendly names on application startup
        /// </summary>
        /// <value><c>true</c> if [load all page slugs on startup]; otherwise, <c>false</c>.</value>
        public bool LoadAllPageSlugsOnStartup { get; set; }

        #endregion Properties
    }
}