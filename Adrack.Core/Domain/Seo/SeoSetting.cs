// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="SeoSetting.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Infrastructure.Configuration;
using System.Collections.Generic;

namespace Adrack.Core.Domain.Seo
{
    /// <summary>
    /// Represents a Seo Setting
    /// Implements the <see cref="Adrack.Core.Infrastructure.Configuration.ISetting" />
    /// </summary>
    /// <seealso cref="Adrack.Core.Infrastructure.Configuration.ISetting" />
    public class SeoSetting : ISetting
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the Page Title
        /// </summary>
        /// <value>The page title.</value>
        public string PageTitle { get; set; }

        /// <summary>
        /// Gets or Sets the Page Title Adjustment
        /// </summary>
        /// <value>The page title adjustment.</value>
        public PageTitleAdjustment PageTitleAdjustment { get; set; }

        /// <summary>
        /// Gets or Sets the Page Title Separator
        /// </summary>
        /// <value>The page title separator.</value>
        public string PageTitleSeparator { get; set; }

        /// <summary>
        /// Gets or Sets the Page Meta Keyword
        /// </summary>
        /// <value>The page meta keyword.</value>
        public string PageMetaKeyword { get; set; }

        /// <summary>
        /// Gets or Sets the Page Meta Description
        /// </summary>
        /// <value>The page meta description.</value>
        public string PageMetaDescription { get; set; }

        /// <summary>
        /// Gets a sets a value of "X-UA-Compatible" meta tag
        /// </summary>
        /// <value>The page xua compatible.</value>
        public string PageXuaCompatible { get; set; }

        /// <summary>
        /// Gets or Sets the Page View Port
        /// </summary>
        /// <value>The page view port.</value>
        public string PageViewPort { get; set; }

        /// <summary>
        /// Gets or Sets the Page Unicode Character In Url
        /// </summary>
        /// <value><c>true</c> if [page unicode character in URL]; otherwise, <c>false</c>.</value>
        public bool PageUnicodeCharacterInUrl { get; set; }

        /// <summary>
        /// Gets or Sets the Page Convert Non-Western Characters
        /// </summary>
        /// <value><c>true</c> if [page convert non western characters]; otherwise, <c>false</c>.</value>
        public bool PageConvertNonWesternCharacters { get; set; }

        /// <summary>
        /// Gets or Sets the Page Canonical Url
        /// </summary>
        /// <value><c>true</c> if [page canonical URL]; otherwise, <c>false</c>.</value>
        public bool PageCanonicalUrl { get; set; }

        /// <summary>
        /// Gets or Sets the Generate Page Meta Description
        /// </summary>
        /// <value><c>true</c> if [generate content page meta description]; otherwise, <c>false</c>.</value>
        public bool GenerateContentPageMetaDescription { get; set; }

        /// <summary>
        /// Gets or Sets the Render X-UA-Compatible (indicating whether we should render <meta http-equiv="X-UA-Compatible" content="IE=edge" /> tag)
        /// </summary>
        /// <value><c>true</c> if [render xua compatible]; otherwise, <c>false</c>.</value>
        public bool RenderXuaCompatible { get; set; }

        /// <summary>
        /// Gets or Sets the value indicating whether Script file bundling and minification is enabled
        /// </summary>
        /// <value><c>true</c> if [enable script bundling]; otherwise, <c>false</c>.</value>
        public bool EnableScriptBundling { get; set; }

        /// <summary>
        /// Gets or Sets the value indicating whether Cascading Style Sheets file bundling and minification is enabled
        /// </summary>
        /// <value><c>true</c> if [enable CSS bundling]; otherwise, <c>false</c>.</value>
        public bool EnableCssBundling { get; set; }

        /// <summary>
        /// Gets or Sets the World Wide Web Requirement Attribute
        /// </summary>
        /// <value>The WWW requirement.</value>
        public WwwRequirement WwwRequirement { get; set; }

        /// <summary>
        /// Gets or Sets the Reserved Page Slug
        /// </summary>
        /// <value>The reserved page slug.</value>
        public List<string> ReservedPageSlug { get; set; }

        #endregion Properties
    }
}