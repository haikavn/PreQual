// ***********************************************************************
// Assembly         : Adrack.Web
// Author           : AdRack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="AllowCrossSiteJsonAttribute.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Web.Mvc;

namespace Adrack.Web
{
    /// <summary>
    ///     Class AllowCrossSiteJsonAttribute.
    ///     Implements the <see cref="ActionFilterAttribute" />
    /// </summary>
    /// <seealso cref="ActionFilterAttribute" />
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class AllowCrossSiteJsonAttribute : ActionFilterAttribute
    {
        /// <summary>
        ///     Called when [action executing].
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Origin", "*");
            base.OnActionExecuting(filterContext);
        }
    }
}