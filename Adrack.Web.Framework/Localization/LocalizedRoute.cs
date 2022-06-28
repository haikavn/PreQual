// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="LocalizedRoute.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Localization;
using Adrack.Core.Infrastructure;
using System.Web;
using System.Web.Routing;

namespace Adrack.Web.Framework.Localization
{
    /// <summary>
    /// Represents a Localized Route
    /// Implements the <see cref="System.Web.Routing.Route" />
    /// </summary>
    /// <seealso cref="System.Web.Routing.Route" />
    public class LocalizedRoute : Route
    {
        #region Fields

        /// <summary>
        /// SEO Friendly Urls For Languages Enabled
        /// </summary>
        private bool? _seoFriendlyUrlsForLanguagesEnabled;

        #endregion Fields



        #region Constructor

        /// <summary>
        /// Localized Route
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="routeHandler">Route Handler</param>
        public LocalizedRoute(string url, IRouteHandler routeHandler)
            : base(url, routeHandler)
        {
        }

        /// <summary>
        /// Localized Route
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="defaults">Defaults</param>
        /// <param name="routeHandler">Route Handler</param>
        public LocalizedRoute(string url, RouteValueDictionary defaults, IRouteHandler routeHandler)
            : base(url, defaults, routeHandler)
        {
        }

        /// <summary>
        /// Localized Route
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="defaults">Defaults</param>
        /// <param name="constraints">Constraints</param>
        /// <param name="routeHandler">Route Handler</param>
        public LocalizedRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, IRouteHandler routeHandler)
            : base(url, defaults, constraints, routeHandler)
        {
        }

        /// <summary>
        /// Localized Route
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="defaults">Defaults</param>
        /// <param name="constraints">Constraints</param>
        /// <param name="dataTokens">Data Tokens</param>
        /// <param name="routeHandler">Route Handler</param>
        public LocalizedRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, RouteValueDictionary dataTokens, IRouteHandler routeHandler)
            : base(url, defaults, constraints, dataTokens, routeHandler)
        {
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get Route Data
        /// </summary>
        /// <param name="httpContextBase">Http Context Base</param>
        /// <returns>Route Data Items</returns>
        public override RouteData GetRouteData(HttpContextBase httpContextBase)
        {
            if (this.SeoFriendlyUrlsForLanguagesEnabled)
            {
                string virtualPath = httpContextBase.Request.AppRelativeCurrentExecutionFilePath;

                string applicationPath = httpContextBase.Request.ApplicationPath;

                if (virtualPath.IsLocalizedUrl(applicationPath, false))
                {
                    string rawUrl = httpContextBase.Request.RawUrl;

                    var newVirtualPath = rawUrl.RemoveLanguageSeoCultureFromRawUrl(applicationPath);

                    if (string.IsNullOrEmpty(newVirtualPath))
                        newVirtualPath = "/";

                    newVirtualPath = newVirtualPath.RemoveApplicationPathFromRawUrl(applicationPath);

                    newVirtualPath = "~" + newVirtualPath;

                    httpContextBase.RewritePath(newVirtualPath, true);
                }
            }

            RouteData routeData = base.GetRouteData(httpContextBase);

            return routeData;
        }

        /// <summary>
        /// Get Virtual Path
        /// </summary>
        /// <param name="requestContext">Request Context</param>
        /// <param name="routeValueDictionary">Route Value Dictionary</param>
        /// <returns>Virtual Path Data Item</returns>
        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary routeValueDictionary)
        {
            VirtualPathData virtualPathData = base.GetVirtualPath(requestContext, routeValueDictionary);

            if (virtualPathData != null && this.SeoFriendlyUrlsForLanguagesEnabled)
            {
                string rawUrl = requestContext.HttpContext.Request.RawUrl;

                string applicationPath = requestContext.HttpContext.Request.ApplicationPath;

                if (rawUrl.IsLocalizedUrl(applicationPath, true))
                {
                    virtualPathData.VirtualPath = string.Concat(rawUrl.GetLanguageSeoCultureFromUrl(applicationPath, true), "/", virtualPathData.VirtualPath);
                }
            }

            return virtualPathData;
        }

        /// <summary>
        /// Clear Seo Friendly Urls Cached Value
        /// </summary>
        public virtual void ClearSeoFriendlyUrlsCachedValue()
        {
            _seoFriendlyUrlsForLanguagesEnabled = null;
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// SEO Friendly Urls For Languages Enabled
        /// </summary>
        /// <value><c>true</c> if [seo friendly urls for languages enabled]; otherwise, <c>false</c>.</value>
        protected bool SeoFriendlyUrlsForLanguagesEnabled
        {
            get
            {
                if (!_seoFriendlyUrlsForLanguagesEnabled.HasValue)
                    _seoFriendlyUrlsForLanguagesEnabled = AppEngineContext.Current.Resolve<LocalizationSetting>().SeoFriendlyUrlsForLanguagesEnabled;

                return _seoFriendlyUrlsForLanguagesEnabled.Value;
            }
        }

        #endregion Properties
    }
}