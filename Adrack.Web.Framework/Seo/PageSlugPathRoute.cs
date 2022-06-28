// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="PageSlugPathRoute.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Infrastructure;
using Adrack.Service.Infrastructure.ApplicationEvent;
using Adrack.Service.Seo;
using Adrack.Web.Framework.Localization;
using System;
using System.Web;
using System.Web.Routing;

namespace Adrack.Web.Framework.Seo
{
    /// <summary>
    /// Represents a Page Slug Path Route
    /// Implements the <see cref="Adrack.Web.Framework.Localization.LocalizedRoute" />
    /// </summary>
    /// <seealso cref="Adrack.Web.Framework.Localization.LocalizedRoute" />
    public partial class PageSlugPathRoute : LocalizedRoute
    {
        #region Constructor

        /// <summary>
        /// Page Slug Path Route
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="routeHandler">Route Handler</param>
        public PageSlugPathRoute(string url, IRouteHandler routeHandler)
            : base(url, routeHandler)
        {
        }

        /// <summary>
        /// Page Slug Path Route
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="defaults">Defaults</param>
        /// <param name="routeHandler">Route Handler</param>
        public PageSlugPathRoute(string url, RouteValueDictionary defaults, IRouteHandler routeHandler)
            : base(url, defaults, routeHandler)
        {
        }

        /// <summary>
        /// Page Slug Path Route
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="defaults">Defaults</param>
        /// <param name="constraints">Constraints</param>
        /// <param name="routeHandler">Route Handler</param>
        public PageSlugPathRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, IRouteHandler routeHandler)
            : base(url, defaults, constraints, routeHandler)
        {
        }

        /// <summary>
        /// Page Slug Path Route
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="defaults">Defaults</param>
        /// <param name="constraints">Constraints</param>
        /// <param name="dataTokens">Data Tokens</param>
        /// <param name="routeHandler">Route Handler</param>
        public PageSlugPathRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, RouteValueDictionary dataTokens, IRouteHandler routeHandler)
            : base(url, defaults, constraints, dataTokens, routeHandler)
        {
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get Route Data
        /// </summary>
        /// <param name="httpContextBase">Http Context Base</param>
        /// <returns>Route Data Item</returns>
        public override RouteData GetRouteData(HttpContextBase httpContextBase)
        {
            RouteData routeData = base.GetRouteData(httpContextBase);

            if (routeData != null)
            {
                var pageSlugService = AppEngineContext.Current.Resolve<IPageSlugService>();
                var pageSlugCache = routeData.Values["page_slug_name"] as string;
                var pageSlug = pageSlugService.GetCachePageSlugByName(pageSlugCache);

                if (pageSlug == null)
                {
                    routeData.Values["controller"] = "Common";
                    routeData.Values["action"] = "PageNotFound";

                    return routeData;
                }

                if (!pageSlug.Active)
                {
                    var activePageSlug = pageSlugService.GetActivePageSlug(pageSlug.LanguageId, pageSlug.EntityId, pageSlug.EntityName);

                    if (string.IsNullOrWhiteSpace(activePageSlug))
                    {
                        routeData.Values["controller"] = "Common";
                        routeData.Values["action"] = "PageNotFound";

                        return routeData;
                    }

                    var commonHelper = AppEngineContext.Current.Resolve<ICommonHelper>();
                    var response = httpContextBase.Response;

                    response.Status = "301 Moved Permanently";
                    response.RedirectLocation = string.Format("{0}{1}", commonHelper.GetAppLocation(false), activePageSlug);
                    response.End();

                    return null;
                }

                var appContext = AppEngineContext.Current.Resolve<IAppContext>();
                var pageSlugAppLanguage = SeoExtensions.GetSeoName(appContext.AppLanguage.Id, pageSlug.EntityId, pageSlug.EntityName);

                if (!String.IsNullOrEmpty(pageSlugAppLanguage) && !pageSlugAppLanguage.Equals(pageSlugCache, StringComparison.InvariantCultureIgnoreCase))
                {
                    var commonHelper = AppEngineContext.Current.Resolve<ICommonHelper>();
                    var response = httpContextBase.Response;

                    response.Status = "302 Moved Temporarily";
                    response.RedirectLocation = string.Format("{0}{1}", commonHelper.GetAppLocation(false), pageSlugAppLanguage);
                    response.End();

                    return null;
                }

                switch (pageSlug.EntityName.ToLowerInvariant())
                {
                    case "artist":
                        {
                            routeData.Values["controller"] = "Content";
                            routeData.Values["action"] = "Artist";
                            routeData.Values["artistid"] = pageSlug.EntityId;
                            routeData.Values["PageSlugSeoName"] = pageSlug.Name;
                        }
                        break;

                    case "album":
                        {
                            routeData.Values["controller"] = "Content";
                            routeData.Values["action"] = "Album";
                            routeData.Values["albumid"] = pageSlug.EntityId;
                            routeData.Values["PageSlugSeoName"] = pageSlug.Name;
                        }
                        break;

                    default:
                        {
                            // generate an event this way developers could insert their own types
                            AppEngineContext.Current.Resolve<IAppEventPublisher>().Publish(new PageSlugEntityNameRequested(routeData, pageSlug));
                        }
                        break;
                }
            }

            return routeData;
        }

        #endregion Methods
    }
}