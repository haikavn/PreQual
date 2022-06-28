// ***********************************************************************
// Assembly         : Adrack.Web
// Author           : AdRack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="PageSlugRouteProvider.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Web.Framework.Mvc.Route;
using Adrack.Web.Framework.Seo;
using System.Web.Routing;

namespace Adrack.Web.Infrastructure
{
    /// <summary>
    ///     Represents a Page Slug Route Provider
    ///     Implements the <see cref="IRouteProvider" />
    /// </summary>
    /// <seealso cref="IRouteProvider" />
    public class PageSlugRouteProvider : IRouteProvider
    {
        #region Methods

        /// <summary>
        ///     Register Routes
        /// </summary>
        /// <param name="routeCollection">Route Collection</param>
        public void RegisterRoutes(RouteCollection routeCollection)
        {
            #region Common

            // Common Page Slug Url
            routeCollection.MapPageSlugPathRoute("PageSlugUrl", "{page_slug_name}",
                new { controller = "Common", action = "PageSlugUrl" }, new[] { "Adrack.Web.Controllers" });

            #endregion Common

            #region Content

            // Content Artist
            //routeCollection.MapLocalizedRoute("Artist", "{PageSlugSeoName}", new { controller = "Content", action = "Artist" }, new[] { "Adrack.Web.Controllers" });

            //// Content Artist
            //routeCollection.MapLocalizedRoute("Album", "{PageSlugSeoName}", new { controller = "Content", action = "Album" }, new[] { "Adrack.Web.Controllers" });

            #endregion Content
        }

        #endregion Methods

        #region Properties

        /// <summary>
        ///     Priority
        /// </summary>
        /// <value>The priority.</value>
        public int Priority => -1000000;

        #endregion Properties
    }
}