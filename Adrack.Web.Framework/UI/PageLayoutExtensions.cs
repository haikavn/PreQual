// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="PageLayoutExtensions.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Infrastructure;
using System.Web.Mvc;

namespace Adrack.Web.Framework.UI
{
    /// <summary>
    /// Represents a Page Layout Extensions
    /// </summary>
    public static class PageLayoutExtensions
    {
        #region Methods

        /// <summary>
        /// Add Page Title
        /// </summary>
        /// <param name="htmlHelper">Html Helper</param>
        /// <param name="name">Name</param>
        public static void AddPageTitle(this HtmlHelper htmlHelper, string name)
        {
            var pageLayoutBuilder = AppEngineContext.Current.Resolve<IPageLayoutBuilder>();

            pageLayoutBuilder.AddPageTitle(name);
        }

        /// <summary>
        /// Append Page Title
        /// </summary>
        /// <param name="htmlHelper">Html Helper</param>
        /// <param name="name">Name</param>
        public static void AppendPageTitle(this HtmlHelper htmlHelper, string name)
        {
            var pageLayoutBuilder = AppEngineContext.Current.Resolve<IPageLayoutBuilder>();

            pageLayoutBuilder.AppendPageTitle(name);
        }

        /// <summary>
        /// Generate Page Title
        /// </summary>
        /// <param name="htmlHelper">Html Helper</param>
        /// <param name="addPageTitle">Add Page Title</param>
        /// <param name="name">Name</param>
        /// <returns>Mvc Html String Item</returns>
        public static MvcHtmlString AppPageTitle(this HtmlHelper htmlHelper, bool addPageTitle, string name = "")
        {
            var pageLayoutBuilder = AppEngineContext.Current.Resolve<IPageLayoutBuilder>();

            htmlHelper.AppendPageTitle(name);

            return MvcHtmlString.Create(htmlHelper.Encode(pageLayoutBuilder.GeneratePageTitle(addPageTitle)));
        }

        /// <summary>
        /// Add Page Meta Keyword
        /// </summary>
        /// <param name="htmlHelper">Html Helper</param>
        /// <param name="name">Name</param>
        public static void AddPageMetaKeyword(this HtmlHelper htmlHelper, string name)
        {
            var pageLayoutBuilder = AppEngineContext.Current.Resolve<IPageLayoutBuilder>();

            pageLayoutBuilder.AddPageMetaKeyword(name);
        }

        /// <summary>
        /// Append Page Meta Keyword
        /// </summary>
        /// <param name="htmlHelper">Html Helper</param>
        /// <param name="name">Name</param>
        public static void AppendPageMetaKeyword(this HtmlHelper htmlHelper, string name)
        {
            var pageLayoutBuilder = AppEngineContext.Current.Resolve<IPageLayoutBuilder>();

            pageLayoutBuilder.AppendPageMetaKeyword(name);
        }

        /// <summary>
        /// Generate Page Meta Keyword
        /// </summary>
        /// <param name="htmlHelper">Html Helper</param>
        /// <param name="name">Name</param>
        /// <returns>Mvc Html String Item</returns>
        public static MvcHtmlString AppPageMetaKeyword(this HtmlHelper htmlHelper, string name = "")
        {
            var pageLayoutBuilder = AppEngineContext.Current.Resolve<IPageLayoutBuilder>();

            htmlHelper.AppendPageMetaKeyword(name);

            return MvcHtmlString.Create(htmlHelper.Encode(pageLayoutBuilder.GeneratePageMetaKeyword()));
        }

        /// <summary>
        /// Add Page Meta Description
        /// </summary>
        /// <param name="htmlHelper">Html Helper</param>
        /// <param name="name">Name</param>
        public static void AddPageMetaDescription(this HtmlHelper htmlHelper, string name)
        {
            var pageLayoutBuilder = AppEngineContext.Current.Resolve<IPageLayoutBuilder>();

            pageLayoutBuilder.AddPageMetaDescription(name);
        }

        /// <summary>
        /// Append Page Meta Description
        /// </summary>
        /// <param name="htmlHelper">Html Helper</param>
        /// <param name="name">Name</param>
        public static void AppendPageMetaDescription(this HtmlHelper htmlHelper, string name)
        {
            var pageLayoutBuilder = AppEngineContext.Current.Resolve<IPageLayoutBuilder>();

            pageLayoutBuilder.AppendPageMetaDescription(name);
        }

        /// <summary>
        /// Generate Page Meta Description
        /// </summary>
        /// <param name="htmlHelper">Html Helper</param>
        /// <param name="name">Name</param>
        /// <returns>Mvc Html String Item</returns>
        public static MvcHtmlString AppPageMetaDescription(this HtmlHelper htmlHelper, string name = "")
        {
            var pageLayoutBuilder = AppEngineContext.Current.Resolve<IPageLayoutBuilder>();

            htmlHelper.AppendPageMetaDescription(name);

            return MvcHtmlString.Create(htmlHelper.Encode(pageLayoutBuilder.GeneratePageMetaDescription()));
        }

        /// <summary>
        /// Add Page Script
        /// </summary>
        /// <param name="htmlHelper">Html Helper</param>
        /// <param name="page">Page</param>
        /// <param name="excludeFromBundle">Exclude From Bundle</param>
        public static void AddPageScript(this HtmlHelper htmlHelper, string page, bool excludeFromBundle = false)
        {
            AddPageScript(htmlHelper, PageLayoutPosition.Head, page, excludeFromBundle);
        }

        /// <summary>
        /// Add Page Script
        /// </summary>
        /// <param name="htmlHelper">Html Helper</param>
        /// <param name="pageLayoutLocation">Page Layout Location</param>
        /// <param name="page">Page</param>
        /// <param name="excludeFromBundle">Exclude From Bundle</param>
        public static void AddPageScript(this HtmlHelper htmlHelper, PageLayoutPosition pageLayoutLocation, string page, bool excludeFromBundle = false)
        {
            var pageLayoutBuilder = AppEngineContext.Current.Resolve<IPageLayoutBuilder>();

            pageLayoutBuilder.AddPageScript(pageLayoutLocation, page, excludeFromBundle);
        }

        /// <summary>
        /// Append Page Script
        /// </summary>
        /// <param name="htmlHelper">Html Helper</param>
        /// <param name="page">Page</param>
        /// <param name="excludeFromBundle">Exclude From Bundle</param>
        public static void AppendPageScript(this HtmlHelper htmlHelper, string page, bool excludeFromBundle = false)
        {
            AppendPageScript(htmlHelper, PageLayoutPosition.Head, page, excludeFromBundle);
        }

        /// <summary>
        /// Append Page Script
        /// </summary>
        /// <param name="htmlHelper">Html Helper</param>
        /// <param name="pageLayoutLocation">Page Layout Location</param>
        /// <param name="page">Page</param>
        /// <param name="excludeFromBundle">Exclude From Bundle</param>
        public static void AppendPageScript(this HtmlHelper htmlHelper, PageLayoutPosition pageLayoutLocation, string page, bool excludeFromBundle = false)
        {
            var pageLayoutBuilder = AppEngineContext.Current.Resolve<IPageLayoutBuilder>();

            pageLayoutBuilder.AppendPageScript(pageLayoutLocation, page, excludeFromBundle);
        }

        /// <summary>
        /// Generate Page Script
        /// </summary>
        /// <param name="htmlHelper">Html Helper</param>
        /// <param name="pageLayoutLocation">Page Layout Location</param>
        /// <param name="urlHelper">Url Helper</param>
        /// <param name="bundleFile">Bundle File</param>
        /// <returns>Mvc Html String Item</returns>
        public static MvcHtmlString AppPageScript(this HtmlHelper htmlHelper, PageLayoutPosition pageLayoutLocation, UrlHelper urlHelper, bool? bundleFile = null)
        {
            var pageLayoutBuilder = AppEngineContext.Current.Resolve<IPageLayoutBuilder>();

            return MvcHtmlString.Create(pageLayoutBuilder.GeneratePageScript(pageLayoutLocation, urlHelper, bundleFile));
        }

        /// <summary>
        /// Add Page Css
        /// </summary>
        /// <param name="htmlHelper">Html Helper</param>
        /// <param name="page">Page</param>
        public static void AddPageCss(this HtmlHelper htmlHelper, string page)
        {
            AddPageCss(htmlHelper, PageLayoutPosition.Head, page);
        }

        /// <summary>
        /// Add Page Css
        /// </summary>
        /// <param name="htmlHelper">Html Helper</param>
        /// <param name="pageLayoutLocation">Page Layout Location</param>
        /// <param name="page">Page</param>
        public static void AddPageCss(this HtmlHelper htmlHelper, PageLayoutPosition pageLayoutLocation, string page)
        {
            var pageLayoutBuilder = AppEngineContext.Current.Resolve<IPageLayoutBuilder>();

            pageLayoutBuilder.AddPageCss(pageLayoutLocation, page);
        }

        /// <summary>
        /// Append Page Css
        /// </summary>
        /// <param name="htmlHelper">Html Helper</param>
        /// <param name="page">Page</param>
        public static void AppendPageCss(this HtmlHelper htmlHelper, string page)
        {
            AppendPageCss(htmlHelper, PageLayoutPosition.Head, page);
        }

        /// <summary>
        /// Append Page Css
        /// </summary>
        /// <param name="htmlHelper">Html Helper</param>
        /// <param name="pageLayoutLocation">Page Layout Location</param>
        /// <param name="page">Page</param>
        public static void AppendPageCss(this HtmlHelper htmlHelper, PageLayoutPosition pageLayoutLocation, string page)
        {
            var pageLayoutBuilder = AppEngineContext.Current.Resolve<IPageLayoutBuilder>();

            pageLayoutBuilder.AppendPageCss(pageLayoutLocation, page);
        }

        /// <summary>
        /// Generate Page Css
        /// </summary>
        /// <param name="htmlHelper">Html Helper</param>
        /// <param name="pageLayoutLocation">Page Layout Location</param>
        /// <param name="urlHelper">Url Helper</param>
        /// <param name="bundleFile">Bundle File</param>
        /// <returns>Mvc Html String Item</returns>
        public static MvcHtmlString AppPageCss(this HtmlHelper htmlHelper, PageLayoutPosition pageLayoutLocation, UrlHelper urlHelper, bool? bundleFile = null)
        {
            var pageLayoutBuilder = AppEngineContext.Current.Resolve<IPageLayoutBuilder>();

            return MvcHtmlString.Create(pageLayoutBuilder.GeneratePageCss(pageLayoutLocation, urlHelper, bundleFile));
        }

        /// <summary>
        /// Add Page Canonical Url
        /// </summary>
        /// <param name="htmlHelper">Html Helper</param>
        /// <param name="name">Name</param>
        public static void AddPageCanonicalUrl(this HtmlHelper htmlHelper, string name)
        {
            var pageLayoutBuilder = AppEngineContext.Current.Resolve<IPageLayoutBuilder>();

            pageLayoutBuilder.AddPageCanonicalUrl(name);
        }

        /// <summary>
        /// Append Page Canonical Url
        /// </summary>
        /// <param name="htmlHelper">Html Helper</param>
        /// <param name="name">Name</param>
        public static void AppendPageCanonicalUrl(this HtmlHelper htmlHelper, string name)
        {
            var pageLayoutBuilder = AppEngineContext.Current.Resolve<IPageLayoutBuilder>();

            pageLayoutBuilder.AppendPageCanonicalUrl(name);
        }

        /// <summary>
        /// Generate Page Canonical Url
        /// </summary>
        /// <param name="htmlHelper">Html Helper</param>
        /// <param name="name">Name</param>
        /// <returns>Mvc Html String Item</returns>
        public static MvcHtmlString AppPageCanonicalUrl(this HtmlHelper htmlHelper, string name = "")
        {
            var pageLayoutBuilder = AppEngineContext.Current.Resolve<IPageLayoutBuilder>();

            htmlHelper.AppendPageCanonicalUrl(name);

            return MvcHtmlString.Create(pageLayoutBuilder.GeneratePageCanonicalUrl());
        }

        /// <summary>
        /// Add Page Custom
        /// </summary>
        /// <param name="htmlHelper">Html Helper</param>
        /// <param name="pageLayoutLocation">Page Layout Location</param>
        /// <param name="name">Name</param>
        public static void AddPageCustom(this HtmlHelper htmlHelper, PageLayoutPosition pageLayoutLocation, string name)
        {
            var pageLayoutBuilder = AppEngineContext.Current.Resolve<IPageLayoutBuilder>();

            pageLayoutBuilder.AddPageCustom(pageLayoutLocation, name);
        }

        /// <summary>
        /// Append Page Custom
        /// </summary>
        /// <param name="htmlHelper">Html Helper</param>
        /// <param name="pageLayoutLocation">Page Layout Location</param>
        /// <param name="name">Name</param>
        public static void AppendPageCustom(this HtmlHelper htmlHelper, PageLayoutPosition pageLayoutLocation, string name)
        {
            var pageLayoutBuilder = AppEngineContext.Current.Resolve<IPageLayoutBuilder>();

            pageLayoutBuilder.AppendPageCustom(pageLayoutLocation, name);
        }

        /// <summary>
        /// Generate Page Custom
        /// </summary>
        /// <param name="htmlHelper">Html Helper</param>
        /// <param name="pageLayoutLocation">Page Layout Location</param>
        /// <returns>Mvc Html String Item</returns>
        public static MvcHtmlString AppPageCustom(this HtmlHelper htmlHelper, PageLayoutPosition pageLayoutLocation)
        {
            var pageLayoutBuilder = AppEngineContext.Current.Resolve<IPageLayoutBuilder>();

            return MvcHtmlString.Create(pageLayoutBuilder.GeneratePageCustom(pageLayoutLocation));
        }

        /// <summary>
        /// Generate Page Custom
        /// </summary>
        /// <param name="htmlHelper">Html Helper</param>
        /// <param name="pageLayoutLocation">Page Layout Location</param>
        /// <param name="newLine">New Line</param>
        /// <returns>Mvc Html String Item</returns>
        public static MvcHtmlString AppPageCustom(this HtmlHelper htmlHelper, PageLayoutPosition pageLayoutLocation, bool newLine)
        {
            var pageLayoutBuilder = AppEngineContext.Current.Resolve<IPageLayoutBuilder>();

            return MvcHtmlString.Create(pageLayoutBuilder.GeneratePageCustom(pageLayoutLocation, newLine));
        }

        /// <summary>
        /// Generate Page Custom
        /// </summary>
        /// <param name="htmlHelper">Html Helper</param>
        /// <param name="pageLayoutLocation">Page Layout Location</param>
        /// <param name="separator">Separator</param>
        /// <returns>Mvc Html String Item</returns>
        public static MvcHtmlString AppPageCustom(this HtmlHelper htmlHelper, PageLayoutPosition pageLayoutLocation, string separator)
        {
            var pageLayoutBuilder = AppEngineContext.Current.Resolve<IPageLayoutBuilder>();

            return MvcHtmlString.Create(pageLayoutBuilder.GeneratePageCustom(pageLayoutLocation, separator));
        }

        #endregion Methods
    }
}