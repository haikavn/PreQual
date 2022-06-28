// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="PageSlugEntityNameRequested.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Service.Seo;
using System.Web.Routing;

namespace Adrack.Web.Framework.Seo
{
    /// <summary>
    /// Represents a Page Slug Entity Name Requested
    /// </summary>
    public class PageSlugEntityNameRequested
    {
        #region Constructor

        /// <summary>
        /// Page Slug Entity Name Requested
        /// </summary>
        /// <param name="routeData">Route Data</param>
        /// <param name="pageSlug">Page Slug</param>
        public PageSlugEntityNameRequested(RouteData routeData, PageSlugService.CachePageSlug pageSlug)
        {
            this.RouteData = routeData;
            this.PageSlug = pageSlug;
        }

        #endregion Constructor



        #region Properties

        /// <summary>
        /// Gets or Sets the Route Data
        /// </summary>
        /// <value>The route data.</value>
        public RouteData RouteData { get; private set; }

        /// <summary>
        /// Gets or Sets the Page Slug
        /// </summary>
        /// <value>The page slug.</value>
        public PageSlugService.CachePageSlug PageSlug { get; private set; }

        #endregion Properties
    }
}