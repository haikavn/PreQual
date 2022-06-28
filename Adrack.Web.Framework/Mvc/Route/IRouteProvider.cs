// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="IRouteProvider.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Web.Routing;

namespace Adrack.Web.Framework.Mvc.Route
{
    /// <summary>
    /// Represents a Route Provider
    /// </summary>
    public interface IRouteProvider
    {
        #region Methods

        /// <summary>
        /// Register Routes
        /// </summary>
        /// <param name="routeCollection">Route Collection</param>
        void RegisterRoutes(RouteCollection routeCollection);

        #endregion Methods

        #region Properties

        /// <summary>
        /// Priority
        /// </summary>
        /// <value>The priority.</value>
        int Priority { get; }

        #endregion Properties
    }
}