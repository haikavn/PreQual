// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="PageSlugPathRouteExtensions.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace Adrack.Web.Framework.Seo
{
    /// <summary>
    /// Represents a Page Slug Path Route Extensions
    /// </summary>
    public static class PageSlugPathRouteExtensions
    {
        #region Methods

        /// <summary>
        /// Map Page Slug Path Route
        /// </summary>
        /// <param name="routeCollection">Route Collection</param>
        /// <param name="name">Name</param>
        /// <param name="url">Url</param>
        /// <returns>Route Item</returns>
        public static Route MapPageSlugPathRoute(this RouteCollection routeCollection, string name, string url)
        {
            return MapPageSlugPathRoute(routeCollection, name, url, null /* defaults */, (object)null /* constraints */);
        }

        /// <summary>
        /// Map Page Slug Path Route
        /// </summary>
        /// <param name="routeCollection">Route Collection</param>
        /// <param name="name">Name</param>
        /// <param name="url">Url</param>
        /// <param name="defaults">Defaults</param>
        /// <returns>Route Item</returns>
        public static Route MapPageSlugPathRoute(this RouteCollection routeCollection, string name, string url, object defaults)
        {
            return MapPageSlugPathRoute(routeCollection, name, url, defaults, (object)null /* constraints */);
        }

        /// <summary>
        /// Map Page Slug Path Route
        /// </summary>
        /// <param name="routeCollection">Route Collection</param>
        /// <param name="name">Name</param>
        /// <param name="url">Url</param>
        /// <param name="defaults">Defaults</param>
        /// <param name="constraints">Constraints</param>
        /// <returns>Route Item</returns>
        public static Route MapPageSlugPathRoute(this RouteCollection routeCollection, string name, string url, object defaults, object constraints)
        {
            return MapPageSlugPathRoute(routeCollection, name, url, defaults, constraints, null /* namespaces */);
        }

        /// <summary>
        /// Map Page Slug Path Route
        /// </summary>
        /// <param name="routeCollection">Route Collection</param>
        /// <param name="name">Name</param>
        /// <param name="url">Url</param>
        /// <param name="namespaces">Namespaces</param>
        /// <returns>Route Item</returns>
        public static Route MapPageSlugPathRoute(this RouteCollection routeCollection, string name, string url, string[] namespaces)
        {
            return MapPageSlugPathRoute(routeCollection, name, url, null /* defaults */, null /* constraints */, namespaces);
        }

        /// <summary>
        /// Map Page Slug Path Route
        /// </summary>
        /// <param name="routeCollection">RouteCollection</param>
        /// <param name="name">Name</param>
        /// <param name="url">Url</param>
        /// <param name="defaults">Defaults</param>
        /// <param name="namespaces">Namespaces</param>
        /// <returns>Route Item</returns>
        public static Route MapPageSlugPathRoute(this RouteCollection routeCollection, string name, string url, object defaults, string[] namespaces)
        {
            return MapPageSlugPathRoute(routeCollection, name, url, defaults, null /* constraints */, namespaces);
        }

        /// <summary>
        /// Map Page Slug Path Route
        /// </summary>
        /// <param name="routeCollection">RouteCollection</param>
        /// <param name="name">Name</param>
        /// <param name="url">Url</param>
        /// <param name="defaults">Defaults</param>
        /// <param name="constraints">Constraints</param>
        /// <param name="namespaces">Namespaces</param>
        /// <returns>Route Item</returns>
        /// <exception cref="ArgumentNullException">
        /// routeCollection
        /// or
        /// url
        /// </exception>
        public static Route MapPageSlugPathRoute(this RouteCollection routeCollection, string name, string url, object defaults, object constraints, string[] namespaces)
        {
            if (routeCollection == null)
            {
                throw new ArgumentNullException("routeCollection");
            }

            if (url == null)
            {
                throw new ArgumentNullException("url");
            }

            var pageSlugPathRoute = new PageSlugPathRoute(url, new MvcRouteHandler())
            {
                Defaults = new RouteValueDictionary(defaults),
                Constraints = new RouteValueDictionary(constraints),
                DataTokens = new RouteValueDictionary()
            };

            if ((namespaces != null) && (namespaces.Length > 0))
            {
                pageSlugPathRoute.DataTokens["Namespaces"] = namespaces;
            }

            routeCollection.Add(name, pageSlugPathRoute);

            return pageSlugPathRoute;
        }

        #endregion Methods
    }
}