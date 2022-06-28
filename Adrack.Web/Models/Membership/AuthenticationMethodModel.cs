// ***********************************************************************
// Assembly         : Adrack.Web
// Author           : AdRack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="AuthenticationMethodModel.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Web.Framework.Mvc;
using System.Web.Routing;

namespace Adrack.Web.Models.Membership
{
    /// <summary>
    ///     Represents a Authentication Method Model
    ///     Implements the <see cref="BaseAppModel" />
    /// </summary>
    /// <seealso cref="BaseAppModel" />
    public class AuthenticationMethodModel : BaseAppModel
    {
        #region Properties

        /// <summary>
        ///     Gets or Sets the Action Name
        /// </summary>
        /// <value>The name of the action.</value>
        public string ActionName { get; set; }

        /// <summary>
        ///     Gets or Sets the Controller Name
        /// </summary>
        /// <value>The name of the controller.</value>
        public string ControllerName { get; set; }

        /// <summary>
        ///     Gets or Sets the Route Values
        /// </summary>
        /// <value>The route values.</value>
        public RouteValueDictionary RouteValues { get; set; }

        #endregion Properties
    }
}