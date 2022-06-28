// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="RoutePublisher.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;

namespace Adrack.Web.Framework.Mvc.Route
{
    /// <summary>
    /// Represents a Route Publisher
    /// Implements the <see cref="Adrack.Web.Framework.Mvc.Route.IRoutePublisher" />
    /// </summary>
    /// <seealso cref="Adrack.Web.Framework.Mvc.Route.IRoutePublisher" />
    public class RoutePublisher : IRoutePublisher
    {
        #region Fields

        /// <summary>
        /// Type Finder
        /// </summary>
        protected readonly ITypeFinder typeFinder;

        #endregion Fields



        #region Constructor

        /// <summary>
        /// Route Publisher
        /// </summary>
        /// <param name="typeFinder">Type Finder</param>
        public RoutePublisher(ITypeFinder typeFinder)
        {
            this.typeFinder = typeFinder;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Register Routes
        /// </summary>
        /// <param name="routeCollection">Route Collection</param>
        public virtual void RegisterRoutes(RouteCollection routeCollection)
        {
            var routeProviderTypes = typeFinder.FindClassesOfType<IRouteProvider>();

            var routeProviders = new List<IRouteProvider>();

            foreach (var providerType in routeProviderTypes)
            {
                var provider = Activator.CreateInstance(providerType) as IRouteProvider;

                routeProviders.Add(provider);
            }

            routeProviders = routeProviders.OrderByDescending(rp => rp.Priority).ToList();

            routeProviders.ForEach(rp => rp.RegisterRoutes(routeCollection));
        }

        #endregion Methods
    }
}