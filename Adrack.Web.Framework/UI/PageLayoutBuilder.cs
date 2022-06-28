// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="PageLayoutBuilder.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Seo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;

namespace Adrack.Web.Framework.UI
{
    /// <summary>
    /// Represents a Page Layout Builder
    /// Implements the <see cref="Adrack.Web.Framework.UI.IPageLayoutBuilder" />
    /// </summary>
    /// <seealso cref="Adrack.Web.Framework.UI.IPageLayoutBuilder" />
    public partial class PageLayoutBuilder : IPageLayoutBuilder
    {
        #region Nested Class

        /// <summary>
        /// Represents a Page Layout Script Referance
        /// </summary>
        private class PageLayoutScriptReferance
        {
            #region Properties

            /// <summary>
            /// Exclude From Bundle
            /// </summary>
            /// <value><c>true</c> if [exclude from bundle]; otherwise, <c>false</c>.</value>
            public bool ExcludeFromBundle { get; set; }

            /// <summary>
            /// Page
            /// </summary>
            /// <value>The page.</value>
            public string Page { get; set; }

            #endregion Properties
        }

        #endregion Nested Class



        #region Fields

        /// <summary>
        /// Static Lock
        /// </summary>
        private static readonly object _sLock = new object();

        /// <summary>
        /// SEO Setting
        /// </summary>
        private readonly SeoSetting _seoSetting;

        /// <summary>
        /// Page Title
        /// </summary>
        private readonly List<string> _pageTitle;

        /// <summary>
        /// Page Meta Keyword
        /// </summary>
        private readonly List<string> _pageMetaKeyword;

        /// <summary>
        /// Page Meta Description
        /// </summary>
        private readonly List<string> _pageMetaDescription;

        /// <summary>
        /// Page Script
        /// </summary>
        private readonly Dictionary<PageLayoutPosition, List<PageLayoutScriptReferance>> _pageScript;

        /// <summary>
        /// Cascading Style Sheets
        /// </summary>
        private readonly Dictionary<PageLayoutPosition, List<string>> _pageCss;

        /// <summary>
        /// Page Canonical Url
        /// </summary>
        private readonly List<string> _pageCanonicalUrl;

        /// <summary>
        /// Page Custom
        /// </summary>
        private readonly Dictionary<PageLayoutPosition, List<string>> _pageCustom;

        #endregion Fields

        #region Utilities

        /// <summary>
        /// Get Bundle Virtual Path
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="prefix">Prefix</param>
        /// <param name="extension">Extension</param>
        /// <returns>String Item</returns>
        /// <exception cref="ArgumentException">page</exception>
        protected virtual string GetBundleVirtualPath(string[] page, string prefix, string extension)
        {
            if (page == null || page.Length == 0)
                throw new ArgumentException("page");

            var urlTokenEncode = "";

            using (SHA256 sha256 = new SHA256Managed())
            {
                var urlTokenEncodeInput = "";

                foreach (var value in page)
                {
                    urlTokenEncodeInput += value;
                    urlTokenEncodeInput += ",";
                }

                byte[] inputByte = sha256.ComputeHash(Encoding.Unicode.GetBytes(urlTokenEncodeInput));

                urlTokenEncode = HttpServerUtility.UrlTokenEncode(inputByte);
            }

            urlTokenEncode = Adrack.Service.Seo.SeoExtensions.GetSeoName(urlTokenEncode);

            var stringBuilder = new StringBuilder(prefix);

            stringBuilder.Append(urlTokenEncode);
            //stringBuilder.Append(extension);

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Get Css Transform
        /// </summary>
        /// <returns>Css Transform Item</returns>
        protected virtual IItemTransform GetCssTransform()
        {
            return new CssRewriteUrlTransform();
        }

        /// <summary>
        /// Generate Page Custom
        /// </summary>
        /// <param name="pageLayoutLocation">Page Layout Location</param>
        /// <param name="newLine">New Line</param>
        /// <param name="separator">Separator</param>
        /// <returns>String Item</returns>
        protected virtual string GeneratePageCustom(PageLayoutPosition pageLayoutLocation, bool newLine, string separator)
        {
            if (!_pageCustom.ContainsKey(pageLayoutLocation) || _pageCustom[pageLayoutLocation] == null)
                return "";

            var distinctParts = _pageCustom[pageLayoutLocation].Distinct().ToList();

            if (distinctParts.Count == 0)
                return "";

            if (newLine)
            {
                var stringBuilder = new StringBuilder();

                foreach (var path in distinctParts)
                {
                    stringBuilder.Append(path);

                    stringBuilder.Append(Environment.NewLine);
                }

                return stringBuilder.ToString();
            }
            else
            {
                if (string.IsNullOrEmpty(separator))
                {
                    var pageCustom = string.Join(" ", distinctParts.AsEnumerable().Reverse().ToArray());

                    return pageCustom;
                }
                else
                {
                    var pageCustom = string.Join(separator, distinctParts.AsEnumerable().Reverse().ToArray());

                    return pageCustom;
                }
            }
        }

        #endregion Utilities

        #region Constructor

        /// <summary>
        /// Page Layout Builder
        /// </summary>
        /// <param name="seoSetting">Seo Setting</param>
        public PageLayoutBuilder(SeoSetting seoSetting)
        {
            this._seoSetting = seoSetting;
            this._pageTitle = new List<string>();
            this._pageMetaKeyword = new List<string>();
            this._pageMetaDescription = new List<string>();
            this._pageScript = new Dictionary<PageLayoutPosition, List<PageLayoutScriptReferance>>();
            this._pageCss = new Dictionary<PageLayoutPosition, List<string>>();
            this._pageCanonicalUrl = new List<string>();
            this._pageCustom = new Dictionary<PageLayoutPosition, List<string>>();
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Add Page Title
        /// </summary>
        /// <param name="name">Name</param>
        public virtual void AddPageTitle(string name)
        {
            if (string.IsNullOrEmpty(name))
                return;

            _pageTitle.Add(name);
        }

        /// <summary>
        /// Append Page Title
        /// </summary>
        /// <param name="name">Name</param>
        public virtual void AppendPageTitle(string name)
        {
            if (string.IsNullOrEmpty(name))
                return;

            _pageTitle.Insert(0, name);
        }

        /// <summary>
        /// Generate Page Title
        /// </summary>
        /// <param name="addPageTitle">Add Page Title</param>
        /// <returns>String Item</returns>
        public virtual string GeneratePageTitle(bool addPageTitle)
        {
            string result = "";

            var pageTitle = string.Join(_seoSetting.PageTitleSeparator, _pageTitle.AsEnumerable().Reverse().ToArray());

            if (!String.IsNullOrEmpty(pageTitle))
            {
                if (addPageTitle)
                {
                    switch (_seoSetting.PageTitleAdjustment)
                    {
                        case PageTitleAdjustment.PageNameAfterAppName:
                            {
                                result = string.Join(_seoSetting.PageTitleSeparator, _seoSetting.PageTitle, pageTitle);
                            }
                            break;

                        case PageTitleAdjustment.AppNameAfterPageName:

                        default:
                            {
                                result = string.Join(_seoSetting.PageTitleSeparator, pageTitle, _seoSetting.PageTitle);
                            }
                            break;
                    }
                }
                else
                {
                    result = pageTitle;
                }
            }
            else
            {
                result = _seoSetting.PageTitle;
            }

            return result;
        }

        /// <summary>
        /// Add Page Meta Keyword
        /// </summary>
        /// <param name="name">Name</param>
        public virtual void AddPageMetaKeyword(string name)
        {
            if (string.IsNullOrEmpty(name))
                return;

            _pageMetaKeyword.Add(name);
        }

        /// <summary>
        /// Append Page Meta Keyword
        /// </summary>
        /// <param name="name">Name</param>
        public virtual void AppendPageMetaKeyword(string name)
        {
            if (string.IsNullOrEmpty(name))
                return;

            _pageMetaKeyword.Insert(0, name);
        }

        /// <summary>
        /// Generate Page Meta Keyword
        /// </summary>
        /// <returns>String Item</returns>
        public virtual string GeneratePageMetaKeyword()
        {
            var pageMetaKeyword = string.Join(", ", _pageMetaKeyword.AsEnumerable().Reverse().ToArray());

            var result = !String.IsNullOrEmpty(pageMetaKeyword) ? pageMetaKeyword : _seoSetting.PageMetaKeyword;

            return result;
        }

        /// <summary>
        /// Add Page Meta Description
        /// </summary>
        /// <param name="name">Name</param>
        public virtual void AddPageMetaDescription(string name)
        {
            if (string.IsNullOrEmpty(name))
                return;

            _pageMetaDescription.Add(name);
        }

        /// <summary>
        /// Append Page Meta Description
        /// </summary>
        /// <param name="name">Name</param>
        public virtual void AppendPageMetaDescription(string name)
        {
            if (string.IsNullOrEmpty(name))
                return;

            _pageMetaDescription.Insert(0, name);
        }

        /// <summary>
        /// Generate Page Meta Description
        /// </summary>
        /// <returns>String Item</returns>
        public virtual string GeneratePageMetaDescription()
        {
            var pageMetaDescription = string.Join(", ", _pageMetaDescription.AsEnumerable().Reverse().ToArray());

            var result = !String.IsNullOrEmpty(pageMetaDescription) ? pageMetaDescription : _seoSetting.PageMetaDescription;

            return result;
        }

        /// <summary>
        /// Add Page Script
        /// </summary>
        /// <param name="pageLayoutLocation">Page Layout Location</param>
        /// <param name="page">Page</param>
        /// <param name="excludeFromBundle">Exclude From Bundle</param>
        public virtual void AddPageScript(PageLayoutPosition pageLayoutLocation, string page, bool excludeFromBundle)
        {
            if (!_pageScript.ContainsKey(pageLayoutLocation))
                _pageScript.Add(pageLayoutLocation, new List<PageLayoutScriptReferance>());

            if (string.IsNullOrEmpty(page))
                return;

            _pageScript[pageLayoutLocation].Add(new PageLayoutScriptReferance
            {
                ExcludeFromBundle = excludeFromBundle,
                Page = page
            });
        }

        /// <summary>
        /// Append Page Script
        /// </summary>
        /// <param name="pageLayoutLocation">Page Layout Location</param>
        /// <param name="page">Page</param>
        /// <param name="excludeFromBundle">Exclude From Bundle</param>
        public virtual void AppendPageScript(PageLayoutPosition pageLayoutLocation, string page, bool excludeFromBundle)
        {
            if (!_pageScript.ContainsKey(pageLayoutLocation))
                _pageScript.Add(pageLayoutLocation, new List<PageLayoutScriptReferance>());

            if (string.IsNullOrEmpty(page))
                return;

            int pageScriptCount = _pageScript[pageLayoutLocation].Count();

            _pageScript[pageLayoutLocation].Insert(pageScriptCount++, new PageLayoutScriptReferance
            {
                ExcludeFromBundle = excludeFromBundle,
                Page = page
            });
        }

        /// <summary>
        /// Generate Page Script
        /// </summary>
        /// <param name="pageLayoutLocation">Page Layout Location</param>
        /// <param name="urlHelper">Url Helper</param>
        /// <param name="bundleFile">Bundle File</param>
        /// <returns>String Item</returns>
        public virtual string GeneratePageScript(PageLayoutPosition pageLayoutLocation, UrlHelper urlHelper, bool? bundleFile = null)
        {
            if (!_pageScript.ContainsKey(pageLayoutLocation) || _pageScript[pageLayoutLocation] == null)
                return "";

            if (_pageScript.Count == 0)
                return "";

            if (!bundleFile.HasValue)
            {
                bundleFile = _seoSetting.EnableScriptBundling && BundleTable.EnableOptimizations;
            }

            if (bundleFile.Value)
            {
                var pageToBundle = _pageScript[pageLayoutLocation]
                    .Where(x => !x.ExcludeFromBundle)
                    .Select(x => x.Page)
                    .Distinct()
                    .ToArray();

                var pageToDontBundle = _pageScript[pageLayoutLocation]
                    .Where(x => x.ExcludeFromBundle)
                    .Select(x => x.Page)
                    .Distinct()
                    .ToArray();

                var stringBuilder = new StringBuilder();

                if (pageToBundle.Length > 0)
                {
                    string bundleVirtualPath = GetBundleVirtualPath(pageToBundle, "~/bundles/scripts/", ".js");

                    lock (_sLock)
                    {
                        var bundleFor = BundleTable.Bundles.GetBundleFor(bundleVirtualPath);

                        if (bundleFor == null)
                        {
                            var bundle = new ScriptBundle(bundleVirtualPath);

                            bundle.Orderer = new PageBundleOrderer();
                            bundle.EnableFileExtensionReplacements = false;
                            bundle.Include(pageToBundle);
                            BundleTable.Bundles.Add(bundle);
                        }
                    }

                    stringBuilder.AppendLine(Scripts.Render(bundleVirtualPath).ToString());
                }

                foreach (var path in pageToDontBundle)
                {
                    stringBuilder.AppendFormat("<script src=\"{0}\" type=\"text/javascript\"></script>", urlHelper.Content(path));

                    stringBuilder.Append(Environment.NewLine);
                }

                return stringBuilder.ToString();
            }
            else
            {
                var stringBuilder = new StringBuilder();

                foreach (var path in _pageScript[pageLayoutLocation].Select(x => x.Page).Distinct())
                {
                    stringBuilder.AppendFormat("<script src=\"{0}\" type=\"text/javascript\"></script>", urlHelper.Content(path));

                    stringBuilder.Append(Environment.NewLine);
                }

                return stringBuilder.ToString();
            }
        }

        /// <summary>
        /// Add Page Cascading Style Sheets
        /// </summary>
        /// <param name="pageLayoutLocation">Page Layout Location</param>
        /// <param name="page">Page</param>
        public virtual void AddPageCss(PageLayoutPosition pageLayoutLocation, string page)
        {
            if (!_pageCss.ContainsKey(pageLayoutLocation))
                _pageCss.Add(pageLayoutLocation, new List<string>());

            if (string.IsNullOrEmpty(page))
                return;

            _pageCss[pageLayoutLocation].Add(page);
        }

        /// <summary>
        /// Append Page Cascading Style Sheets
        /// </summary>
        /// <param name="pageLayoutLocation">Page Layout Location</param>
        /// <param name="page">Page</param>
        public virtual void AppendPageCss(PageLayoutPosition pageLayoutLocation, string page)
        {
            if (!_pageCss.ContainsKey(pageLayoutLocation))
                _pageCss.Add(pageLayoutLocation, new List<string>());

            if (string.IsNullOrEmpty(page))
                return;

            int pageCssCount = _pageCss[pageLayoutLocation].Count();

            _pageCss[pageLayoutLocation].Insert(pageCssCount++, page);
        }

        /// <summary>
        /// Generate Page Cascading Style Sheets
        /// </summary>
        /// <param name="pageLayoutLocation">Page Layout Location</param>
        /// <param name="urlHelper">Url Helper</param>
        /// <param name="bundleFile">Bundle File</param>
        /// <returns>String Item</returns>
        public virtual string GeneratePageCss(PageLayoutPosition pageLayoutLocation, UrlHelper urlHelper, bool? bundleFile = null)
        {
            if (!_pageCss.ContainsKey(pageLayoutLocation) || _pageCss[pageLayoutLocation] == null)
                return "";

            var distinctParts = _pageCss[pageLayoutLocation].Distinct().ToList();

            if (distinctParts.Count == 0)
                return "";

            if (!bundleFile.HasValue)
            {
                bundleFile = _seoSetting.EnableCssBundling && BundleTable.EnableOptimizations;
            }

            if (bundleFile.Value)
            {
                var stringBuilder = new StringBuilder();

                var pageToBundle = distinctParts.ToArray();

                if (pageToBundle.Length > 0)
                {
                    string bundleVirtualPath = GetBundleVirtualPath(pageToBundle, "~/bundles/styles/", ".css");

                    lock (_sLock)
                    {
                        var bundleFor = BundleTable.Bundles.GetBundleFor(bundleVirtualPath);

                        if (bundleFor == null)
                        {
                            var styleBundle = new StyleBundle(bundleVirtualPath);

                            styleBundle.Orderer = new PageBundleOrderer();
                            styleBundle.EnableFileExtensionReplacements = false;

                            foreach (var ptb in pageToBundle)
                            {
                                styleBundle.Include(ptb, GetCssTransform());
                            }

                            BundleTable.Bundles.Add(styleBundle);
                        }
                    }

                    stringBuilder.AppendLine(Styles.Render(bundleVirtualPath).ToString());
                }

                return stringBuilder.ToString();
            }
            else
            {
                var stringBuilder = new StringBuilder();

                foreach (var path in distinctParts)
                {
                    stringBuilder.AppendFormat("<link href=\"{0}\" rel=\"stylesheet\" type=\"text/css\" />", urlHelper.Content(path));

                    stringBuilder.Append(Environment.NewLine);
                }

                return stringBuilder.ToString();
            }
        }

        /// <summary>
        /// Add Page Canonical Url
        /// </summary>
        /// <param name="page">Page</param>
        public virtual void AddPageCanonicalUrl(string page)
        {
            if (string.IsNullOrEmpty(page))
                return;

            _pageCanonicalUrl.Add(page);
        }

        /// <summary>
        /// Append Page Canonical Url
        /// </summary>
        /// <param name="page">The page.</param>
        public virtual void AppendPageCanonicalUrl(string page)
        {
            if (string.IsNullOrEmpty(page))
                return;

            _pageCanonicalUrl.Insert(0, page);
        }

        /// <summary>
        /// Generate Page Canonical Url
        /// </summary>
        /// <returns>String Item</returns>
        public virtual string GeneratePageCanonicalUrl()
        {
            var stringBuilder = new StringBuilder();

            foreach (var pageCanonicalUrl in _pageCanonicalUrl)
            {
                stringBuilder.AppendFormat("<link rel=\"canonical\" href=\"{0}\" />", pageCanonicalUrl);

                stringBuilder.Append(Environment.NewLine);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Add Page Custom
        /// </summary>
        /// <param name="pageLayoutLocation">Page Layout Location</param>
        /// <param name="page">Page</param>
        public virtual void AddPageCustom(PageLayoutPosition pageLayoutLocation, string page)
        {
            if (!_pageCustom.ContainsKey(pageLayoutLocation))
                _pageCustom.Add(pageLayoutLocation, new List<string>());

            if (string.IsNullOrEmpty(page))
                return;

            _pageCustom[pageLayoutLocation].Add(page);
        }

        /// <summary>
        /// Append Page Custom
        /// </summary>
        /// <param name="pageLayoutLocation">Page Layout Location</param>
        /// <param name="page">Page</param>
        public virtual void AppendPageCustom(PageLayoutPosition pageLayoutLocation, string page)
        {
            if (!_pageCustom.ContainsKey(pageLayoutLocation))
                _pageCustom.Add(pageLayoutLocation, new List<string>());

            if (string.IsNullOrEmpty(page))
                return;

            _pageCustom[pageLayoutLocation].Insert(0, page);
        }

        /// <summary>
        /// Generate Page Custom
        /// </summary>
        /// <param name="pageLayoutLocation">Page Layout Location</param>
        /// <returns>String Item</returns>
        public virtual string GeneratePageCustom(PageLayoutPosition pageLayoutLocation)
        {
            return GeneratePageCustom(pageLayoutLocation, false, null);
        }

        /// <summary>
        /// Generate Page Custom
        /// </summary>
        /// <param name="pageLayoutLocation">Page Layout Location</param>
        /// <param name="newLine">New Line</param>
        /// <returns>String Item</returns>
        public virtual string GeneratePageCustom(PageLayoutPosition pageLayoutLocation, bool newLine)
        {
            return GeneratePageCustom(pageLayoutLocation, newLine, null);
        }

        /// <summary>
        /// Generate Page Custom
        /// </summary>
        /// <param name="pageLayoutLocation">Page Layout Location</param>
        /// <param name="separator">Separator</param>
        /// <returns>String Item</returns>
        public virtual string GeneratePageCustom(PageLayoutPosition pageLayoutLocation, string separator)
        {
            return GeneratePageCustom(pageLayoutLocation, false, separator);
        }

        #endregion Methods
    }
}