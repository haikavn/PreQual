// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="LocalizedRouteExtensions.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace Adrack.Web.Framework.Localization
{
    /// <summary>
    /// Represents a Localized Route Extensions
    /// </summary>
    public static class LocalizedRouteExtensions
    {
        #region Methods

        /// <summary>
        /// Map Localized Route
        /// </summary>
        /// <param name="routeCollection">Route Collection</param>
        /// <param name="name">Name</param>
        /// <param name="url">Url</param>
        /// <returns>Route Item</returns>
        public static Route MapLocalizedRoute(this RouteCollection routeCollection, string name, string url)
        {
            return MapLocalizedRoute(routeCollection, name, url, null /* defaults */, (object)null /* constraints */);
        }

        /// <summary>
        /// Map Localized Route
        /// </summary>
        /// <param name="routeCollection">Route Collection</param>
        /// <param name="name">Name</param>
        /// <param name="url">Url</param>
        /// <param name="defaults">Defaults</param>
        /// <returns>Route Item</returns>
        public static Route MapLocalizedRoute(this RouteCollection routeCollection, string name, string url, object defaults)
        {
            return MapLocalizedRoute(routeCollection, name, url, defaults, (object)null /* constraints */);
        }

        /// <summary>
        /// Map Localized Route
        /// </summary>
        /// <param name="routeCollection">Route Collection</param>
        /// <param name="name">Name</param>
        /// <param name="url">Url</param>
        /// <param name="defaults">Defaults</param>
        /// <param name="constraints">Constraints</param>
        /// <returns>Route Item</returns>
        public static Route MapLocalizedRoute(this RouteCollection routeCollection, string name, string url, object defaults, object constraints)
        {
            return MapLocalizedRoute(routeCollection, name, url, defaults, constraints, null /* namespaces */);
        }

        /// <summary>
        /// Map Localized Route
        /// </summary>
        /// <param name="routeCollection">Route Collection</param>
        /// <param name="name">Name</param>
        /// <param name="url">Url</param>
        /// <param name="namespaces">Namespaces</param>
        /// <returns>Route Item</returns>
        public static Route MapLocalizedRoute(this RouteCollection routeCollection, string name, string url, string[] namespaces)
        {
            return MapLocalizedRoute(routeCollection, name, url, null /* defaults */, null /* constraints */, namespaces);
        }

        /// <summary>
        /// Map Localized Route
        /// </summary>
        /// <param name="routeCollection">RouteCollection</param>
        /// <param name="name">Name</param>
        /// <param name="url">Url</param>
        /// <param name="defaults">Defaults</param>
        /// <param name="namespaces">Namespaces</param>
        /// <returns>Route Item</returns>
        public static Route MapLocalizedRoute(this RouteCollection routeCollection, string name, string url, object defaults, string[] namespaces)
        {
            return MapLocalizedRoute(routeCollection, name, url, defaults, null /* constraints */, namespaces);
        }

        /// <summary>
        /// Map Localized Route
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
        public static Route MapLocalizedRoute(this RouteCollection routeCollection, string name, string url, object defaults, object constraints, string[] namespaces)
        {
            if (routeCollection == null)
            {
                throw new ArgumentNullException("routeCollection");
            }

            if (url == null)
            {
                throw new ArgumentNullException("url");
            }

            var localizedRoute = new LocalizedRoute(url, new MvcRouteHandler())
            {
                Defaults = new RouteValueDictionary(defaults),
                Constraints = new RouteValueDictionary(constraints),
                DataTokens = new RouteValueDictionary()
            };

            if ((namespaces != null) && (namespaces.Length > 0))
            {
                localizedRoute.DataTokens["Namespaces"] = namespaces;
            }

            routeCollection.Add(name, localizedRoute);

            return localizedRoute;
        }

        /// <summary>
        /// Clear Seo Friendly Urls Cached Value For Routes
        /// </summary>
        /// <param name="routeCollection">Route Collection</param>
        /// <exception cref="ArgumentNullException">routeCollection</exception>
        public static void ClearSeoFriendlyUrlsCachedValueForRoutes(this RouteCollection routeCollection)
        {
            if (routeCollection == null)
            {
                throw new ArgumentNullException("routeCollection");
            }

            foreach (var route in routeCollection)
            {
                if (route is LocalizedRoute)
                {
                    ((LocalizedRoute)route).ClearSeoFriendlyUrlsCachedValue();
                }
            }
        }

        #endregion Methods
    }
}