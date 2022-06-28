// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="IRoutePublisher.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Web.Routing;

namespace Adrack.Web.Framework.Mvc.Route
{
    /// <summary>
    /// Represents a Route Publisher
    /// </summary>
    public interface IRoutePublisher
    {
        #region Methods

        /// <summary>
        /// Register Routes
        /// </summary>
        /// <param name="routeCollection">Route Collection</param>
        void RegisterRoutes(RouteCollection routeCollection);

        #endregion Methods
    }
}