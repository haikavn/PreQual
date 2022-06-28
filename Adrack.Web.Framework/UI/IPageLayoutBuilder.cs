// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="IPageLayoutBuilder.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Web.Mvc;

namespace Adrack.Web.Framework.UI
{
    /// <summary>
    /// Represents a Page Layout Builder
    /// </summary>
    public partial interface IPageLayoutBuilder
    {
        #region Methods

        /// <summary>
        /// Add Page Title
        /// </summary>
        /// <param name="name">Name</param>
        void AddPageTitle(string name);

        /// <summary>
        /// Append Page Title
        /// </summary>
        /// <param name="name">Name</param>
        void AppendPageTitle(string name);

        /// <summary>
        /// Generate Page Title
        /// </summary>
        /// <param name="addPageTitle">Add Page Title</param>
        /// <returns>String Item</returns>
        string GeneratePageTitle(bool addPageTitle);

        /// <summary>
        /// Add Page Meta Keyword
        /// </summary>
        /// <param name="name">Name</param>
        void AddPageMetaKeyword(string name);

        /// <summary>
        /// Append Page Meta Keyword
        /// </summary>
        /// <param name="name">Name</param>
        void AppendPageMetaKeyword(string name);

        /// <summary>
        /// Generate Page Meta Keyword
        /// </summary>
        /// <returns>String Item</returns>
        string GeneratePageMetaKeyword();

        /// <summary>
        /// Add Page Meta Description
        /// </summary>
        /// <param name="name">Name</param>
        void AddPageMetaDescription(string name);

        /// <summary>
        /// Append Page Meta Description
        /// </summary>
        /// <param name="name">Name</param>
        void AppendPageMetaDescription(string name);

        /// <summary>
        /// Generate Page Meta Description
        /// </summary>
        /// <returns>String Item</returns>
        string GeneratePageMetaDescription();

        /// <summary>
        /// Add Page Script
        /// </summary>
        /// <param name="pageLayoutLocation">Page Layout Location</param>
        /// <param name="page">Page</param>
        /// <param name="excludeFromBundle">Exclude From Bundle</param>
        void AddPageScript(PageLayoutPosition pageLayoutLocation, string page, bool excludeFromBundle);

        /// <summary>
        /// Append Page Script
        /// </summary>
        /// <param name="pageLayoutLocation">Page Layout Location</param>
        /// <param name="page">Page</param>
        /// <param name="excludeFromBundle">Exclude From Bundle</param>
        void AppendPageScript(PageLayoutPosition pageLayoutLocation, string page, bool excludeFromBundle);

        /// <summary>
        /// Generate Page Script
        /// </summary>
        /// <param name="pageLayoutLocation">Page Layout Location</param>
        /// <param name="urlHelper">Url Helper</param>
        /// <param name="bundleFile">Bundle File</param>
        /// <returns>String Item</returns>
        string GeneratePageScript(PageLayoutPosition pageLayoutLocation, UrlHelper urlHelper, bool? bundleFile = null);

        /// <summary>
        /// Add Page Cascading Style Sheets
        /// </summary>
        /// <param name="pageLayoutLocation">Page Layout Location</param>
        /// <param name="page">Page</param>
        void AddPageCss(PageLayoutPosition pageLayoutLocation, string page);

        /// <summary>
        /// Append Page Cascading Style Sheets
        /// </summary>
        /// <param name="pageLayoutLocation">Page Layout Location</param>
        /// <param name="page">Page</param>
        void AppendPageCss(PageLayoutPosition pageLayoutLocation, string page);

        /// <summary>
        /// Generate Page Cascading Style Sheets
        /// </summary>
        /// <param name="pageLayoutLocation">Page Layout Location</param>
        /// <param name="urlHelper">Url Helper</param>
        /// <param name="bundleFile">Bundle File</param>
        /// <returns>String Item</returns>
        string GeneratePageCss(PageLayoutPosition pageLayoutLocation, UrlHelper urlHelper, bool? bundleFile = null);

        /// <summary>
        /// Add Page Canonical Url
        /// </summary>
        /// <param name="page">Page</param>
        void AddPageCanonicalUrl(string page);

        /// <summary>
        /// Append Page Canonical Url
        /// </summary>
        /// <param name="page">The page.</param>
        void AppendPageCanonicalUrl(string page);

        /// <summary>
        /// Generate Page Canonical Url
        /// </summary>
        /// <returns>String Item</returns>
        string GeneratePageCanonicalUrl();

        /// <summary>
        /// Add Page Custom
        /// </summary>
        /// <param name="pageLayoutLocation">Page Layout Location</param>
        /// <param name="page">Page</param>
        void AddPageCustom(PageLayoutPosition pageLayoutLocation, string page);

        /// <summary>
        /// Append Page Custom
        /// </summary>
        /// <param name="pageLayoutLocation">Page Layout Location</param>
        /// <param name="page">Page</param>
        void AppendPageCustom(PageLayoutPosition pageLayoutLocation, string page);

        /// <summary>
        /// Generate Page Custom
        /// </summary>
        /// <param name="pageLayoutLocation">Page Layout Location</param>
        /// <returns>String Item</returns>
        string GeneratePageCustom(PageLayoutPosition pageLayoutLocation);

        /// <summary>
        /// Generate Page Custom
        /// </summary>
        /// <param name="pageLayoutLocation">Page Layout Location</param>
        /// <param name="newLine">New Line</param>
        /// <returns>String Item</returns>
        string GeneratePageCustom(PageLayoutPosition pageLayoutLocation, bool newLine);

        /// <summary>
        /// Generate Page Custom
        /// </summary>
        /// <param name="pageLayoutLocation">Page Layout Location</param>
        /// <param name="separator">Separator</param>
        /// <returns>String Item</returns>
        string GeneratePageCustom(PageLayoutPosition pageLayoutLocation, string separator);

        #endregion Methods
    }
}