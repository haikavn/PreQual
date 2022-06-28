// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="GuidRouteConstraint.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Web;
using System.Web.Routing;

namespace Adrack.Web.Framework.Mvc.Route
{
    /// <summary>
    /// Represents a Guid Route Constraint
    /// Implements the <see cref="System.Web.Routing.IRouteConstraint" />
    /// </summary>
    /// <seealso cref="System.Web.Routing.IRouteConstraint" />
    public class GuidRouteConstraint : IRouteConstraint
    {
        #region Fields

        /// <summary>
        /// Allow Empty
        /// </summary>
        protected readonly bool _allowEmpty;

        #endregion Fields



        #region Constructor

        /// <summary>
        /// Guid Route Constraint
        /// </summary>
        /// <param name="allowEmpty">Allow Empty</param>
        public GuidRouteConstraint(bool allowEmpty)
        {
            this._allowEmpty = allowEmpty;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Match
        /// </summary>
        /// <param name="httpContextBase">Http Context Base</param>
        /// <param name="route">Route</param>
        /// <param name="parameterName">Parameter Name</param>
        /// <param name="routeValueDictionary">Route Value Dictionary</param>
        /// <param name="routeDirection">Route Direction</param>
        /// <returns>Bool Item</returns>
        public bool Match(HttpContextBase httpContextBase, System.Web.Routing.Route route, string parameterName, RouteValueDictionary routeValueDictionary, RouteDirection routeDirection)
        {
            if (routeValueDictionary.ContainsKey(parameterName))
            {
                string stringValue = routeValueDictionary[parameterName] != null ? routeValueDictionary[parameterName].ToString() : null;

                if (!string.IsNullOrEmpty(stringValue))
                {
                    Guid guidValue;

                    return Guid.TryParse(stringValue, out guidValue) && (_allowEmpty || guidValue != Guid.Empty);
                }
            }

            return false;
        }

        #endregion Methods
    }
}